namespace RentABike.Domain.Exceptions;

public class BikeIdNotFoundException(int id) : Exception
{
    private int Id { get; } = id;
    public override string Message => $"Couldn't find a bike with the id '{Id}'";
}