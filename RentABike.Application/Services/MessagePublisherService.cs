using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;
using RentABike.Domain.Interfaces;

namespace RentABike.Application.Services;

public class MessagePublisherService(IAmazonSQS sqs, IConfiguration configuration) : IMessagePublisherService
{
    private readonly string bikeCreationQueue = configuration.GetSection("AWS:MessageQueues:BikeCreation").Value ?? throw new InvalidOperationException();
    private readonly string bikeNotificationQueue = configuration.GetSection("AWS:MessageQueues:BikeNotification").Value ?? throw new InvalidOperationException();

    public async Task PublishNotificationAsync(string message)
    {
        var queueUrlResponse = await sqs.GetQueueUrlAsync(bikeNotificationQueue);
        await PublishAsync(message, queueUrlResponse.QueueUrl);
    }

    public async Task PublishCreationAsync<T>(T message)
    {
        var queueUrlResponse = await sqs.GetQueueUrlAsync(bikeCreationQueue);
        await PublishAsync(message, queueUrlResponse.QueueUrl);
    }

    private async Task PublishAsync<T>(T message, string queueUrl)
    {
        var request = new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = JsonSerializer.Serialize(message)
        };

        await sqs.SendMessageAsync(request);
    }
}