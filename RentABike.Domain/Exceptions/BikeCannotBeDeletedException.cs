namespace RentABike.Domain.Exceptions;

public class BikeCannotBeDeletedException() : Exception
{
    public override string Message => $"Cannot delete bike as it was rented before.";
}