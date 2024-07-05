using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentABike.Domain.Dtos;
using RentABike.Domain.Entities;
using RentABike.Domain.Interfaces;

namespace RentABike.Controllers;

[ApiController]
[Route("user")]
public class DeliveryPersonController(
    UserManager<User> userManager,
    IDeliveryPersonService deliveryPersonService,
    IImageService imageService) : ControllerBase
{
    [HttpPost]
    [Route("create")]
    [Authorize(Roles = "DeliveryPerson")]
    public async Task<IActionResult> Create(DeliveryPersonDto deliveryPersonDto)
    {
        var UserEmailAddress = HttpContext.User.Identity!.Name;
        var user = await userManager.FindByNameAsync(UserEmailAddress!);
        deliveryPersonDto.DeliveryPersonId = user.Id;
        
        await deliveryPersonService.Create(deliveryPersonDto);
        return Created();
    }

    [HttpPost("upload")]
    [Authorize(Roles = "DeliveryPerson")]
    public async Task<IActionResult> Upload([FromForm(Name = "Data")] IFormFile file)
    {
        var UserEmailAddress = HttpContext.User.Identity!.Name;
        var user = await userManager.FindByNameAsync(UserEmailAddress!);
        
        await imageService.UploadImageAsync(user.Id, file);
        return Ok();
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IEnumerable<DeliveryPersonDto>> GetAll()
    {
        return await deliveryPersonService.GetAll();
    }

    [HttpGet]
    [Route("search")]
    [Authorize(Roles = "Admin, DeliveryPerson")]
    public async Task<IActionResult> GetByCnpj([FromBody] JsonElement body)
    {
        var cnpj = body.GetProperty("cnpj").GetString();
        var userFromDb = await deliveryPersonService.GetByCnpj(cnpj);
        
        var userRole = HttpContext.User.FindFirstValue(ClaimTypes.Role);
        if (string.Equals(userRole, "DeliveryPerson"))
        {
            var UserEmailAddress = HttpContext.User.Identity!.Name;
            var user = await userManager.FindByNameAsync(UserEmailAddress!);
            if (!string.Equals(userFromDb.DeliveryPersonId, user.Id))
                return NotFound();
        }
        
        return Ok(userFromDb);
    }

    [HttpGet("image")]
    [Authorize(Roles = "DeliveryPerson")]
    public async Task<IActionResult> GetImage()
    {
        var UserEmailAddress = HttpContext.User.Identity!.Name;
        var user = await userManager.FindByNameAsync(UserEmailAddress!);
        
        var response = await imageService.GetImageAsync(user.Id);
        return File(response.ResponseStream, response.Headers.ContentType);
    }

    [HttpPut]
    [Authorize(Roles = "DeliveryPerson")]
    public async Task Update([FromBody] DeliveryPersonDto deliveryPersonDto)
    {
        var UserEmailAddress = HttpContext.User.Identity!.Name;
        var user = await userManager.FindByNameAsync(UserEmailAddress!);
        deliveryPersonDto.DeliveryPersonId = user.Id;
        
        await deliveryPersonService.Update(deliveryPersonDto);
    }

    [HttpDelete]
    [Authorize(Roles = "DeliveryPerson")]
    public async Task Delete()
    {
        var UserEmailAddress = HttpContext.User.Identity!.Name;
        var user = await userManager.FindByNameAsync(UserEmailAddress!);
        
        await deliveryPersonService.DeleteById(user.Id);
    }
}