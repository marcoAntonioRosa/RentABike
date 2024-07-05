namespace RentABike.Domain.Exceptions;

public class BikeLicensePlateNotFoundException(string licensePlate) : Exception
{
    private string LicensePlate { get; } = licensePlate;
    public override string Message => $"Couldn't find a bike with the license plate '{LicensePlate}'";
}