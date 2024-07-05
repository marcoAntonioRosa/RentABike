namespace RentABike.Domain.Exceptions;

public class RentalPeriodTooLowException(double rentalPeriod) : Exception
{
    private double RentalPeriod { get; } = rentalPeriod;
    public override string Message => $"Rental period '{RentalPeriod}' cannot be under 7 days";
}