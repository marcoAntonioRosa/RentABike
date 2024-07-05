namespace RentABike.Domain.Exceptions;

public class RentalPeriodDefaultException(double rentalPeriod) : Exception
{
    private double RentalPeriod { get; } = rentalPeriod;
    public override string Message => $"";
}