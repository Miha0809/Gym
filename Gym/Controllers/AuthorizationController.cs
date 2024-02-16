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
[AllowAnonymous]
public class AuthorizationController(UserManager<User> userManager, SignInManager<User> signInManager) : Controller
{
    /// <summary>
    /// Реєстрація користувача.
    /// </summary>
    /// <remarks>
    /// Roles: Seller, Buyer
    /// Sample request:
    /// 
    ///     {
    ///         "email": "seller@gmail.com",
    ///         "password": "Test1234,",
    ///         "role": "Seller"
    ///     }
    ///
    ///     {
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
            user = new User
            {
                UserName = registerDto.Email,
                Email = registerDto.Email
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, registerDto.Role);
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