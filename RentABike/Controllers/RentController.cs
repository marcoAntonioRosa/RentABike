using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentABike.Domain.Dtos;
using RentABike.Domain.Entities;
using RentABike.Domain.Interfaces;

namespace RentABike.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "DeliveryPerson")]
public class RentController(UserManager<User> userManager, IRentService rentService) : ControllerBase
{
    [HttpPost]
    public async Task<RentalDto> Rent(RentDto rentDto)
    {
        var UserEmailAddress = HttpContext.User.Identity!.Name;
        var user = await userManager.FindByNameAsync(UserEmailAddress!);
        rentDto.DeliveryPersonId = user.Id;
        
        return await rentService.Rent(rentDto);
    }
}