using System.ComponentModel.DataAnnotations;

namespace RentABike.Domain.Dtos;

public class UserForRegistrationDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string? Email { get; set; }
    public required string? Password { get; set; }
    
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public required string ConfirmPassword { get; set; }
}