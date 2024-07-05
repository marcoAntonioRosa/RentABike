using System.ComponentModel.DataAnnotations;

namespace RentABike.Domain.Entities;

public class Bike
{
    [Key]
    public int Id { get; set; }
    public int Year { get; set; }
    public string Model { get; set; } = null!;
    public string LicensePlate { get; set; } = null!;
}