namespace RentABike.Domain.Dtos;

public class BikeDto
{
    public required int Year { get; set; }
    public required string Model { get; set; } = null!;
    public required string LicensePlate { get; set; } = null!;
}