using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentABike.Domain.Entities;

public class Rent
{
    [Key]
    public int Id { get; set; }
    public DeliveryPerson DeliveryPerson { get; set; }
    public Bike Bike { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime InformedEndDate { get; set; }
    
    [Column(TypeName = "decimal(6, 2)")]
    public decimal RentalValue { get; set; }
    
    [Column(TypeName = "decimal(6, 2)")]
    public decimal PenaltyValue { get; set; }
    
    [Column(TypeName = "decimal(6, 2)")]
    public decimal TotalAmount => RentalValue + PenaltyValue;
}