namespace RentABike.Domain.Exceptions;

public class DeliveryPersonDriverLicenseAlreadyExistsException(string driverLicense) : Exception
{
    private string DriverLicense { get; } = driverLicense;
    public override string Message => $"A user with the driver's license '{DriverLicense}' already exists";
}