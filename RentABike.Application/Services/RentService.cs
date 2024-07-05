using Mapster;
using RentABike.Domain.Dtos;
using RentABike.Domain.Entities;
using RentABike.Domain.Enums;
using RentABike.Domain.Exceptions;
using RentABike.Domain.Interfaces;

namespace RentABike.Application.Services;

public class RentService(
    IRentRepository rentRepository,
    IDeliveryPersonRepository deliveryPersonRepository,
    IBikeRepository bikeRepository) : IRentService
{
    public async Task<RentalDto> Rent(RentDto rentDto)
    {
        var rent = rentDto.Adapt<Rent>();
        await ValidateDeliveryPerson(rent, rentDto);
        await ValidateBike(rent, rentDto);

        // Calculate rental price base on plan
        rent.StartDate = DateTime.Today.AddDays(1);
        var rentalPeriodInDays = (rent.EndDate - rent.StartDate).TotalDays;
        var planCategoryPrice = GetPlanCategoryPrice(rentalPeriodInDays);
        rent.RentalValue = (decimal)rentalPeriodInDays * planCategoryPrice;

        // Calculate rental penalty fee based on ExpectedEndDate
        rent.PenaltyValue =
            CalculateRentalPenaltyFee(rent.EndDate, rent.InformedEndDate, rentalPeriodInDays, planCategoryPrice);

        await rentRepository.Add(rent);
        var rental = rent.Adapt<RentalDto>();
        
        return rental;
    }

    private async Task ValidateDeliveryPerson(Rent rent, RentDto rentDto)
    {
        // Check if person registered
        var deliveryPersonExists = await deliveryPersonRepository.CheckId(rentDto.DeliveryPersonId);
        if (!deliveryPersonExists)
            throw new DeliveryPersonNotFoundIdException(rentDto.DeliveryPersonId);

        // Load
        rent.DeliveryPerson = await deliveryPersonRepository.GetById(rentDto.DeliveryPersonId);
        
        // Validate license type
        var licenseType = rent.DeliveryPerson.DriverLicenseType;
        var isAllowedToRent = licenseType == LicenseType.A || licenseType == LicenseType.AB;
        if (!isAllowedToRent)
            throw new DeliveryPersonNotAllowedToRentException(rentDto.DeliveryPersonId);
    }

    private async Task ValidateBike(Rent rent, RentDto rentDto)
    {
        // Check if bike exists
        var bikeExists = await bikeRepository.CheckId(rentDto.BikeId);
        if (!bikeExists)
            throw new BikeIdNotFoundException(rentDto.BikeId);

        // Load
        rent.Bike = await bikeRepository.GetById(rentDto.BikeId);

    }

    private static decimal GetPlanCategoryPrice(double rentalPeriod)
    {
        return rentalPeriod switch
        {
            < 7 => throw new RentalPeriodTooLowException(rentalPeriod),
            >= 7 and <= 14 => 30,
            >= 15 and <= 29 => 28,
            >= 30 and <= 44 => 22,
            >= 45 and <= 49 => 20,
            >= 50 => 18,
            _ => throw new Exception(
                $"The following rental period '{rentalPeriod}' caused an exception while trying to get the category price")
        };
    }

    private static decimal CalculateRentalPenaltyFee(DateTime endDate, DateTime informedEndDate,
        double rentalPeriodInDays, decimal planCategoryPrice)
    {
        var remainingDays = (endDate - informedEndDate).TotalDays;

        if (remainingDays < 0)
            return (decimal)Math.Abs(remainingDays * 50);

        if (remainingDays > 0)
        {
            var basePercentageFee = GetEarlyReturnFeePercentage(planCategoryPrice);
            var remainingDaysValue = (decimal)remainingDays * planCategoryPrice;
            return remainingDaysValue + (remainingDaysValue * (decimal)basePercentageFee);
        }

        return 0;
    }

    private static double GetEarlyReturnFeePercentage(decimal planCategoryPrice)
    {
        return planCategoryPrice switch
        {
            30 => 0.2,
            28 => 0.4,
            // 22 => 0.6,
            // 20 => 0.8,
            // 18 => 1.0,
            _ => throw new RentalEarlyReturnFeeException(planCategoryPrice)
        };
    }
}