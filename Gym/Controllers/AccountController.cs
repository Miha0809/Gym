using AutoMapper;
using Gym.Models;
using Gym.Models.DTOs;
using Gym.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gym.Controllers;

/// <summary>
/// Власний кабінет корисутвача.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AccountController(GymDbContext context, UserManager<User> userManager, IMapper mapper) : Controller
{
    /// <summary>
    /// Інформація про користувача.
    /// </summary>
    /// <returns></returns>
    [HttpGet("personal_information")]
    [Authorize(Roles = $"{nameof(Roles.Buyer)}")]
    public async Task<IActionResult> GetPersonalInformation()
    {
        var user = await userManager.GetUserAsync(User);
        return Ok(mapper.Map<UserDto>(user));
    }
    
    /// <summary>
    /// Вийти із облікового запису.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Response.Cookies.Delete(".AspNetCore.Identity.Application");
        return Ok("The user has logged out of the account");
    }
}