using Amazon.S3;
using Amazon.SQS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentABike.Domain.Interfaces;
using RentABike.Infrastructure.BackgroundServices;
using RentABike.Infrastructure.Persistence;
using RentABike.Infrastructure.Persistence.Repositories;

namespace RentABike.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddImageStorage();
        services.AddMessageQueue();
    }

    private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        var connectionString = configuration.GetConnectionString("PostgreSql");
        services.AddDbContext<PostgreSqlDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IBikeRepository, BikeRepository>();
        services.AddScoped<IRentRepository, RentRepository>();
        services.AddScoped<IDeliveryPersonRepository, DeliveryPersonRepository>();
    }

    private static void AddImageStorage(this IServiceCollection services)
    {
        services.AddSingleton<IAmazonS3, AmazonS3Client>();
    }

    private static void AddMessageQueue(this IServiceCollection services)
    {
        services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
        services.AddHostedService<SqsConsumerService>();
    }
}