using Microsoft.AspNetCore.Identity;

namespace RentABike.Domain.Entities;

public class Role : IdentityRole
{
    public string? Description { get; set; }
}