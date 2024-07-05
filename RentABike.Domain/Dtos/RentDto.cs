namespace RentABike.Domain.Dtos;

public class RentDto
{
    public string? DeliveryPersonId { get; set; }
    public required int BikeId { get; set; }
    public required DateTime EndDate { get; set; }
    public required DateTime InformedEndDate { get; set; }
}