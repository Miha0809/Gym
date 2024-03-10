using AutoMapper;
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
[ApiController]
[Route("api/[controller]")]
public class AuthorizationController(GymDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper) : Controller
{
    /// <summary>
    /// Реєстрація користувача.
    /// </summary>
    /// <remarks>
    /// Roles: Trainer, Buyer
    ///
    /// Sample example:
    ///
    ///     {
    ///         "name": "Witold Stepin",
    ///         "email": "trainer@gmail.com",
    ///         "password": "Test1234,",
    ///         "role": "Trainer",
    ///         "subscribeMonth": 6
    ///     }
    /// 
    /// </remarks>
    /// <param name="registerDto">Поля для реєстрації.</param>
    /// <returns></returns>
    [HttpPost("register_user")]
    [Authorize(Roles = $"{nameof(Roles.Seller)}")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        User user = null;

        if (ModelState.IsValid && !registerDto.Role.Equals(nameof(Roles.Seller)))
        {
            var sub = new Subscription()
            {
                End = DateTime.UtcNow.AddMonths(registerDto.SubscribeMonth).ToShortDateString()
            };

            await context.Subscriptions.AddAsync(sub);

            user = new User
            {
                Name = registerDto.Name,
                UserName = registerDto.Email,
                Email = registerDto.Email,
                Subscription = sub
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, registerDto.Role);
                await context.SaveChangesAsync();
                
                return Ok(mapper.Map<UserDto>(user));
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
    ///         "email": "buyer@gmail.com",
    ///         "password": "Test1234,"
    ///     }
    ///
    ///     {
    ///         "email": "trainer@gmail.com",
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