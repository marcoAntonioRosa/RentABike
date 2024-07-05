using System.Net;
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RentABike.Domain.Dtos;
using RentABike.Domain.Entities;
using RentABike.Domain.Interfaces;

namespace RentABike.Infrastructure.BackgroundServices;

public class SqsConsumerService(IAmazonSQS sqs, 
    IConfiguration configuration, 
    IServiceScopeFactory serviceScopeFactory,
    IMessagePublisherService messagePublisherService) : BackgroundService
{
    private readonly string bikeCreationQueue = configuration.GetSection("AWS:MessageQueues:BikeCreation").Value ?? throw new InvalidOperationException();
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queueUrlResponse = await sqs.GetQueueUrlAsync(bikeCreationQueue, stoppingToken);
        var receiveRequest = new ReceiveMessageRequest
        {
            QueueUrl = queueUrlResponse.QueueUrl,
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            var messageResponse = await sqs.ReceiveMessageAsync(receiveRequest, stoppingToken);
            if (messageResponse.HttpStatusCode != HttpStatusCode.OK)
                throw new Exception("Error in background services while trying poll message queue");

            foreach (var message in messageResponse.Messages)
            {
                var bikeDTO = JsonSerializer.Deserialize<BikeDto>(message.Body);
                var bike = bikeDTO.Adapt<Bike>();

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    IBikeRepository bikeRepository = scope.ServiceProvider.GetRequiredService<IBikeRepository>();

                    await bikeRepository.Add(bike);
                }
                
                await sqs.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle, stoppingToken);
                
                if (bike.Year == 2024)
                    messagePublisherService.PublishNotificationAsync(
                        $"A bike launched in 2024 was registered! " +
                                $"Model: {bike.Model} License plate: {bike.LicensePlate}");
            }
        }
    }
}