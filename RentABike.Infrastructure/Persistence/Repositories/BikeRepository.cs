using Microsoft.EntityFrameworkCore;
using RentABike.Domain.Entities;
using RentABike.Domain.Interfaces;

namespace RentABike.Infrastructure.Persistence.Repositories;

public class BikeRepository(PostgreSqlDbContext dbContext) : IBikeRepository
{
    public async Task<Bike> Add(Bike bike)
    {
        await dbContext.Bike.AddAsync(bike);
        await dbContext.SaveChangesAsync();
        return bike;
    }

    public async Task<IEnumerable<Bike>> GetAll()
    {
        return await dbContext.Bike.ToListAsync();
    }

    public async Task<Bike?> GetByLicensePlate(string licensePlate)
    {
        return await dbContext.Bike.Where(w => w.LicensePlate == licensePlate).FirstOrDefaultAsync();
    }

    public async Task<bool> CheckLicensePlate(string licensePlate)
    {
        return await dbContext.Bike.Where(w => w.LicensePlate == licensePlate).AnyAsync();
    }

    public async Task<bool> CheckId(int id)
    {
        return await dbContext.Bike.Where(w => w.Id == id).AnyAsync();
    }

    public async Task<Bike?> GetById(int id)
    {
        return await dbContext.Bike.Where(w => w.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Bike> Update(Bike bike)
    {
        dbContext.Bike.Update(bike);
        await dbContext.SaveChangesAsync();
        return bike;
    }

    public async Task Delete(int id)
    {
        await dbContext.Bike.Where(w => w.Id == id).ExecuteDeleteAsync();
    }
}