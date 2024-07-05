namespace RentABike.Domain.Exceptions;

public class DeliveryPersonNotFoundIdException(string id) : Exception
{
    private string Id { get; } = id;
    public override string Message => $"Couldn't find a user with the id '{id}'";
}