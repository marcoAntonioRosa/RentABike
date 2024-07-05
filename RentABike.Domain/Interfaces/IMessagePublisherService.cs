namespace RentABike.Domain.Interfaces;

public interface IMessagePublisherService
{

    Task PublishNotificationAsync(string message);
    Task PublishCreationAsync<T>(T message);
}