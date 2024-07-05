using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentABike.Domain.Dtos;
using RentABike.Domain.Entities;
using RentABike.Domain.Interfaces;

namespace RentABike.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController(UserManager<User> userManager, IJwtService jwtService) : ControllerBase
{
    private readonly IJwtService _jwtService = jwtService;

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistrationDto)
    {
        var user = userForRegistrationDto.Adapt<User>();
        user.PasswordHash = userManager.PasswordHasher.HashPassword(user, userForRegistrationDto.Password!);
        var result = await userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(s => s.Description);

            return BadRequest(new RegistrationResponseDto { Errors = errors });
        }

        await userManager.AddToRoleAsync(user, "Admin");

        return StatusCode(201);
    }

    [HttpPost]
    [Route("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto userForAuthenticationDto)
    {
        var user = await userManager.FindByNameAsync(userForAuthenticationDto.Email!);
        if (user is null || !await userManager.CheckPasswordAsync(user, userForAuthenticationDto.Password!))
            return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication"});

        var roles = await userManager.GetRolesAsync(user);
        var token = jwtService.CreateToken(user, roles);

        return Ok(new AuthResponseDto
        {
            IsAuthSuccessful = true,
            Token = token
        });
    }
}