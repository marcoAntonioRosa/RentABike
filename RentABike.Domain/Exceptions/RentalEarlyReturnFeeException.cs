namespace RentABike.Domain.Exceptions;

public class RentalEarlyReturnFeeException(decimal planCategoryPrice) : Exception
{
    private decimal PlanCategoryPrice { get; } = planCategoryPrice;
    public override string Message => $"The following category price '{PlanCategoryPrice}' could not be mapped into a valid percentage fee";
}