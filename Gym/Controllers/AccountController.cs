using AutoMapper;
using Gym.Models;
using Gym.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gym.Controllers;

/// <summary>
/// Власний кабінет корисутвача.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountController(UserManager<User> userManager, IMapper mapper) : Controller
{
    /// <summary>
    /// Інформація про користувача.
    /// </summary>
    /// <returns></returns>
    [HttpGet("personal_information")]
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
    public async Task<IActionResult> Logout()
    {
        HttpContext.Response.Cookies.Delete(".AspNetCore.Identity.Application");
        return Ok("The user has logged out of the account");
    }
}