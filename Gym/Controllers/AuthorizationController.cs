using Gym.Models;
using Gym.Models.DTOs;
using Gym.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gym.Controllers;

/// <summary>
/// Котроллер для авторизації.
/// </summary>
[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class AuthorizationController(GymDbContext context, UserManager<User> userManager, SignInManager<User> signInManager) : Controller
{
    /// <summary>
    /// Реєстрація користувача.
    /// </summary>
    /// <remarks>
    /// Roles: Seller, Buyer
    /// Sample request:
    /// 
    ///     {
    ///         "firstName": "Adam",
    ///         "lastName": "Zawatski",
    ///         "SubscribeMonth": 12,
    ///         "email": "seller@gmail.com",
    ///         "password": "Test1234,",
    ///         "role": "Seller"
    ///     }
    ///
    ///     {
    ///         "firstName": "Adam",
    ///         "lastName": "Zawatski",
    ///         "SubscribeMonth": 1,
    ///         "email": "buyer@gmail.com",
    ///         "password": "Test1234,",
    ///         "role": "Buyer"
    ///     }
    /// 
    /// </remarks>
    /// <param name="registerDto">Поля для реєстрації.</param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        User user = null;

        if (ModelState.IsValid)
        {
            var sub = new Subscription()
            {
                End = DateTime.UtcNow.AddMonths(registerDto.SubscribeMonth).ToShortDateString()
            };

            await context.Subscriptions.AddAsync(sub);

            user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.Email,
                Email = registerDto.Email,
                Subscription = sub
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, registerDto.Role);
                await context.SaveChangesAsync();
                
                return Ok("User registered successfully.");
            }

            return BadRequest(result.Errors);
        }


        return Ok(user);
    }

    /// <summary>
    /// Авотризація.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///         "email": "seller@gmail.com",
    ///         "password": "Test1234,"
    ///     }
    ///
    ///     {
    ///         "email": "buyer@gmail.com",
    ///         "password": "Test1234,"
    ///     }
    /// 
    /// </remarks>
    /// <param name="loginDto">Поля для авторизації.</param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var result = await signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, isPersistent: false, lockoutOnFailure: false);
        
        if (result.Succeeded)
        {
            return Ok("Logged in successfully");
        }

        return Unauthorized("Invalid email or password");
    }
}