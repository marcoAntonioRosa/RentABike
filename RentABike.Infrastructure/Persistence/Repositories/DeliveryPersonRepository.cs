using Microsoft.EntityFrameworkCore;
using RentABike.Domain.Entities;
using RentABike.Domain.Enums;
using RentABike.Domain.Interfaces;

namespace RentABike.Infrastructure.Persistence.Repositories;

public class DeliveryPersonRepository(PostgreSqlDbContext dbContext) : IDeliveryPersonRepository
{
    
    public async Task<DeliveryPerson> Add(DeliveryPerson deliveryPerson)
    {
        await dbContext.DeliveryPerson.AddAsync(deliveryPerson);
        await dbContext.SaveChangesAsync();
        return deliveryPerson;
    }
    
    public async Task<IEnumerable<DeliveryPerson>> GetAll()
    {
        return await dbContext.DeliveryPerson.ToListAsync();
    }

    public async Task<DeliveryPerson?> GetById(string id)
    {
        return await dbContext.DeliveryPerson
            .Where(w => w.DeliveryPersonId == id)
            .FirstOrDefaultAsync();
    }

    public async Task<DeliveryPerson?> GetByCnpj(string cnpj)
    {
        return await dbContext.DeliveryPerson
            .Where(w => w.Cnpj == cnpj)
            .FirstOrDefaultAsync();
    }

    public async Task<LicenseType> GetDriverLicenseTypeById(string id)
    {
        return await dbContext.DeliveryPerson
            .Where(w => w.DeliveryPersonId == id)
            .Select(s => s.DriverLicenseType)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> CheckId(string id)
    {
        return await dbContext.DeliveryPerson.Where(w => w.DeliveryPersonId == id).AnyAsync();
    }
    
    public async Task<bool> CheckCnpj(string cnpj)
    {
        return await dbContext.DeliveryPerson.Where(w => w.Cnpj == cnpj).AnyAsync();
    }

    public async Task<bool> CheckDriverLicense(string driverLicense)
    {
        return await dbContext.DeliveryPerson.Where(w => w.DriverLicenseNumber == driverLicense).AnyAsync();
    }


    public async Task<DeliveryPerson> Update(DeliveryPerson deliveryPerson)
    {
        dbContext.DeliveryPerson.Update(deliveryPerson);
        await dbContext.SaveChangesAsync();
        return deliveryPerson;
    }

    public async Task Delete(string id)
    {
        await dbContext.DeliveryPerson.Where(w => w.DeliveryPersonId == id).ExecuteDeleteAsync();
    }
}