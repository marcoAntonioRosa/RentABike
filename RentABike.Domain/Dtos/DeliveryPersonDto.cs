using RentABike.Domain.Enums;

namespace RentABike.Domain.Dtos;

public class DeliveryPersonDto
{
    public string? DeliveryPersonId { get; set; }
    public required string Name { get; set; } = null!;
    public required string Cnpj { get; set; } = null!;
    public required string DriverLicenseNumber { get; set; } = null!;
    public required DateOnly Birthday { get; set; }
    public required LicenseType DriverLicenseType { get; set; }
}