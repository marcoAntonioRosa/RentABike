using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentABike.Domain.Dtos;
using RentABike.Domain.Interfaces;

namespace RentABike.Controllers;


[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Admin")]
public class BikeController(IBikeService bikeService) : ControllerBase
{
    
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateBike(BikeDto bikeDto)
    {
        await bikeService.CreateBike(bikeDto);
        return Created();
    }
    
    [HttpGet]
    public async Task<IEnumerable<BikeDto>> GetAllBikes()
    {
        return await bikeService.GetAllBikes();
    }
    
    [HttpPost]
    [Route("search")]
    public async Task<BikeDto?> GetBikeByLicensePlate([FromBody] JsonElement body)
    {
        var licensePlate = body.GetProperty("licensePlate").GetString();
        return await bikeService.GetBikeByLicensePlate(licensePlate);
    }
    
    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> UpdateBikeLicensePlate([FromBody] JsonElement body)
    {
        var oldLicensePlate = body.GetProperty("oldLicensePlate").GetString();
        var newLicensePlate = body.GetProperty("newLicensePlate").GetString();
        await bikeService.UpdateBikeLicensePlate(oldLicensePlate, newLicensePlate);
        return NoContent();
    }
    
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteBikeById(int id)
    {
        await bikeService.DeleteBikeById(id);
        return NoContent();
    }
}

