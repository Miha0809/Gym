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
/// Контроллер для взаємодії з клієнтами.
/// </summary>
/// <param name="context">Контекст бази даних.</param>
/// <param name="userManager">Менеджер користувачів.</param>
/// <param name="mapper">Мапер об'єктів.</param>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{nameof(Roles.Seller)}")]
public class UserController(GymDbContext context, UserManager<User> userManager, IMapper mapper) : Controller
{
    /// <summary>
    /// Всі користувачі із роллю "Buyer" (відвідувачі).
    /// </summary>
    /// <returns></returns>
    [HttpGet("buyers")]
    public async Task<IActionResult> GetBuyers()
    {
        var users = await userManager.GetUsersInRoleAsync(nameof(Roles.Buyer));
        return Ok(mapper.Map<List<UserDto>>(users));
    }

    /// <summary>
    /// Інформація про певного користувача.
    /// </summary>
    /// <param name="email">Електрона пошта відвідувача.</param>
    /// <returns></returns>
    [HttpGet("buyer/{email}")]
    public async Task<IActionResult> GetBuyerByEmail(string email)
    {
        var user = await context.Users.FirstOrDefaultAsync(user => user.Email!.Equals(email));
        
        if (user is not null)
        {
            return Ok(mapper.Map<UserDto>(user));
        }

        return BadRequest("The user's email is incorrect.");
    }

    /// <summary>
    /// Надати/продовжити абонимент користувачу.
    /// </summary>
    /// <param name="emailBuyer">Електрона пошта відвідувача.</param>
    /// <param name="month">Продовжити на кількість місяців.</param>
    /// <returns></returns>
    [HttpPut("provide_subscription/{emailBuyer}/{month}")]
    public async Task<IActionResult> ProvideSubscription(string emailBuyer, int month)
    {
        var user = await context.Users.FirstOrDefaultAsync(user => user.Email.Equals(emailBuyer));

        if (user is not null)
        {
            DateTime.TryParse(user.Subscription.End,  out var date);
            user.Subscription.End = date.AddMonths(month).ToShortDateString();

            context.Users.Update(user);
            await context.SaveChangesAsync();

            return Ok(mapper.Map<UserDto>(user));
        }

        return BadRequest("The user's email is incorrect");
    }
}