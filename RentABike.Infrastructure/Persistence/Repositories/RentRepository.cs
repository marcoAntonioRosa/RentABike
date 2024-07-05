using Microsoft.EntityFrameworkCore;
using RentABike.Domain.Entities;
using RentABike.Domain.Interfaces;

namespace RentABike.Infrastructure.Persistence.Repositories;

public class RentRepository(PostgreSqlDbContext dbContext) : IRentRepository
{
    public async Task<Rent> Add(Rent rent)
    {
        await dbContext.Rent.AddAsync(rent);
        await dbContext.SaveChangesAsync();
        return rent;
    }

    public async Task<bool> CheckIfItWasRented(int id)
    {
        return await dbContext.Rent
            .Where(w => w.Bike.Id == id)
            .AnyAsync();
    }
}