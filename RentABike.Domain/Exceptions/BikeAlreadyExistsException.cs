namespace RentABike.Domain.Exceptions;

public class BikeAlreadyExistsException(string licensePlate) : Exception
{
    private string LicensePlate { get; } = licensePlate;
    public override string Message => $"A bike with the license plate '{LicensePlate}' already exists";
}