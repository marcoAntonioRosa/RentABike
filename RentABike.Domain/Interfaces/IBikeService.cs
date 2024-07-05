using RentABike.Domain.Dtos;

namespace RentABike.Domain.Interfaces;

public interface IBikeService
{
    Task CreateBike(BikeDto bikeDto);
    Task<IEnumerable<BikeDto>> GetAllBikes();
    Task<BikeDto?> GetBikeByLicensePlate(string licensePlate);
    Task<BikeDto?> UpdateBikeLicensePlate(string oldLicensePlate, string newLicensePlate);
    Task DeleteBikeById(int id);

}