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
    /// Roles: Seller, Buyer
    /// Sample request:
    ///
    ///     {
    ///         "name": "Adam Zawatski",
    ///         "SubscribeMonth": 1,
    ///         "email": "buyer@gmail.com",
    ///         "password": "Test1234,",
    ///         "role": "Buyer"
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

        if (ModelState.IsValid)
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
                
                return Ok("User registered successfully.");
            }

            return BadRequest(result.Errors);
        }
        
        return Ok(user);
    }

    /// <summary>
    /// Реєстрація компанії. Її можна виконати тільки 1 раз.
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("register_company")]
    public async Task<IActionResult> RegisterCompany()
    {
        var email = "company@company.com";
        var password = "Test1234,";

        if (await userManager.FindByEmailAsync(email) is null)
        {
            var subscription = new Subscription()
            {
                End = DateTime.UtcNow.AddMonths(12).ToShortDateString()
            };
            
            var address = new Address
            {
                City = "Nadvirna",
                Street = "Mazepy 9/17"
            };

            var images = new List<Image>
            {
                new()
                {
                    Link =
                        "https://images.unsplash.com/photo-1534438327276-14e5300c3a48?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
                },
                new()
                {
                    Link =
                        "https://images.unsplash.com/photo-1571902943202-507ec2618e8f?q=80&w=1975&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
                },
                new()
                {
                    Link =
                        "https://images.unsplash.com/photo-1571388208497-71bedc66e932?q=80&w=2072&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
                }
            };

            var company = new User
            {
                UserName = email,
                Email = email,
                Name = "GorillaGym",
                Subscription = subscription,
                Address = address,
                Images = images
            };
            
            await context.Subscriptions.AddRangeAsync(subscription);
            await context.Addresses.AddAsync(address);
            await context.Images.AddRangeAsync(images);
            await context.SaveChangesAsync();
            await userManager.CreateAsync(company, password);
            await userManager.AddToRoleAsync(company, nameof(Roles.Seller));
            
            return Ok(mapper.Map<CompanyDto>(company));
        }

        return BadRequest("Such a company already exists");
    }
    
    /// <summary>
    /// Авотризація.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///         "email": "company@company.com",
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