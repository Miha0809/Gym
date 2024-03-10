using AutoMapper;
using Gym.Models;
using Gym.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gym.Controllers;

/// <summary>
/// Контроллер для інформації про спортивний клуб.
/// </summary>
/// <param name="userManager">Менеджер користувачів.</param>
/// <param name="mapper">Мапер об'єктів.</param>
[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class CompanyController(UserManager<User> userManager, IMapper mapper) : Controller
{
    /// <summary>
    /// Інформація про спортивний клуб.
    /// </summary>
    /// <returns></returns>
    [HttpGet("company_information")]
    public async Task<IActionResult> GetCompanyInformation()
    {
        var user = await userManager.GetUsersInRoleAsync(nameof(Roles.Seller));
        return Ok(mapper.Map<CompanyDto>(user[0]));
    }

    /// <summary>
    /// Галерея спортивного клубу.
    /// </summary>
    /// <returns></returns>
    [HttpGet("galley")]
    public async Task<IActionResult> GetImages()
    {
        var user = await userManager.GetUsersInRoleAsync(nameof(Roles.Seller));
        return Ok(mapper.Map<List<ImageDto>>(user[0].Images));
    }

    /// <summary>
    /// Тренери спортивного клубу.
    /// </summary>
    /// <returns></returns>
    [HttpGet("trainers")]
    public async Task<IActionResult> GetTrainers()
    {
        var trainers = await userManager.GetUsersInRoleAsync($"{nameof(Roles.Trainer)}");
        return Ok(mapper.Map<List<TrainerDto>>(trainers));
    }
}