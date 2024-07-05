using Microsoft.AspNetCore.Identity;

namespace RentABike.Domain.Entities;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public virtual DeliveryPerson? DeliveryPerson { get; set; }
}