namespace RentABike.Domain.Exceptions;

public class DeliveryPersonNotAllowedToRentException(string id) : Exception
{
    private string Id { get; } = id;
    public override string Message => $"Invalid licenseType for user with id {Id}. Rental is only allowed for users with a driver license of type 'A' or 'AB'";
}