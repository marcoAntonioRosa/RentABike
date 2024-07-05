using Mapster;
using RentABike.Domain.Dtos;
using RentABike.Domain.Entities;
using RentABike.Domain.Exceptions;
using RentABike.Domain.Interfaces;

namespace RentABike.Application.Services;

public class BikeService(
    IBikeRepository bikeRepository, 
    IRentRepository rentRepository,
    IMessagePublisherService messagePublisherService) : IBikeService
{
    public async Task CreateBike(BikeDto bikeDto)
    {
        var bikeAlreadyExists = await bikeRepository.CheckLicensePlate(bikeDto.LicensePlate);
        if (bikeAlreadyExists)
            throw new BikeAlreadyExistsException(bikeDto.LicensePlate);

        await messagePublisherService.PublishCreationAsync(bikeDto);
    }

    public async Task<IEnumerable<BikeDto>> GetAllBikes()
    {
        var bikes = await bikeRepository.GetAll();
        return bikes.Adapt<IEnumerable<BikeDto>>();
    }

    public async Task<BikeDto?> GetBikeByLicensePlate(string licensePlate)
    {
        var bike = await bikeRepository.GetByLicensePlate(licensePlate);
        if (bike == null)
            throw new BikeLicensePlateNotFoundException(licensePlate);

        return bike.Adapt<BikeDto>();
    }

    public async Task<BikeDto?> UpdateBikeLicensePlate(string oldLicensePlate, string newLicensePlate)
    {
        var bike = await bikeRepository.GetByLicensePlate(newLicensePlate);
        if (bike != null)
            throw new BikeAlreadyExistsException(newLicensePlate);

        bike = await bikeRepository.GetByLicensePlate(oldLicensePlate);
        if (bike == null)
            throw new BikeLicensePlateNotFoundException(oldLicensePlate);

        bike.LicensePlate = newLicensePlate;
        var updatedBike = await bikeRepository.Update(bike);
        return updatedBike.Adapt<BikeDto>();
    }

    public async Task DeleteBikeById(int id)
    {
        var wasRented = await rentRepository.CheckIfItWasRented(id);

        if (wasRented)
            throw new BikeCannotBeDeletedException();

        await bikeRepository.Delete(id);
    }
}