using RentABike.Domain.Entities;

namespace RentABike.Domain.Interfaces;

public interface IRentRepository
{
    Task<Rent> Add(Rent rent);
    Task<bool> CheckIfItWasRented(int id);
}