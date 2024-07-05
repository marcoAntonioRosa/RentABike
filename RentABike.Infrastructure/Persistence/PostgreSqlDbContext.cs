using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentABike.Domain.Entities;
using RentABike.Infrastructure.Persistence.Seeds;

namespace RentABike.Infrastructure.Persistence;

public class PostgreSqlDbContext(DbContextOptions options) : IdentityDbContext<User, Role, string>(options)
{
    public DbSet<DeliveryPerson> DeliveryPerson { get; set; }
    public DbSet<Bike> Bike { get; set; }
    public DbSet<Rent> Rent { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DeliveryPerson>()
            .HasIndex(b => b.Cnpj)
            .IsUnique();
        
        modelBuilder.Entity<DeliveryPerson>()
            .HasIndex(b => b.DriverLicenseNumber)
            .IsUnique();
        
        modelBuilder.Entity<Bike>()
            .HasIndex(b => b.LicensePlate)
            .IsUnique();
        
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        // modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
    }

}