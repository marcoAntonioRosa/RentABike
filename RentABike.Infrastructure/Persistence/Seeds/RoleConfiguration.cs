using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentABike.Domain.Entities;

namespace RentABike.Infrastructure.Persistence.Seeds;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasData(
            new Role
            {
                Id = "55578adb-54fa-4ccc-bae9-ea94bdae63e2",
                Name = "DeliveryPerson",
                NormalizedName = "DELIVERYPERSON",
                Description = "The delivery person role for the user"
            },
            new Role
            {
                Id = "80d80873-6f2c-4657-88f2-d39c4ca80de0",
                Name = "Admin",
                NormalizedName = "ADMIN",
                Description = "The admin role for the user"
            }
        );
    }
}