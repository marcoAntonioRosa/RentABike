using System.ComponentModel.DataAnnotations.Schema;
using RentABike.Domain.Enums;

namespace RentABike.Domain.Entities;

public class DeliveryPerson
{
    [ForeignKey("User")]
    public string DeliveryPersonId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Cnpj { get; set; } = null!;
    public string DriverLicenseNumber { get; set; } = null!;
    public LicenseType DriverLicenseType { get; set; }
    public DateOnly Birthday { get; set; }
    public virtual User? User { get; set; }
}