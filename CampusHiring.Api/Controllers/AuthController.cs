using CampusHiring.Api.Application.DTOs.Auth;
using CampusHiring.Api.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CampusHiring.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(UserManager<User> userManager, CampusHiringDbContext context) : ControllerBase
{
    //[HttpGet]
    //public async Task<ActionResult<RegisteredUserDto>> Register(RegisterUserDto)
    //{

    //}
}
