using RentABike.Domain.Entities;

namespace RentABike.Domain.Interfaces;

public interface IBikeRepository
{
    Task<Bike> Add(Bike bike);
    Task<IEnumerable<Bike>> GetAll();
    Task<Bike?> GetByLicensePlate(string licensePlate);
    Task<bool> CheckLicensePlate(string licensePlate);
    Task<bool> CheckId(int id);
    Task<Bike?> GetById(int id);
    Task<Bike> Update(Bike bike);
    Task Delete(int id);
}