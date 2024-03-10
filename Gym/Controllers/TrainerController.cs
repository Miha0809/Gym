using AutoMapper;
using Gym.Models;
using Gym.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gym.Controllers;

/// <summary>
/// Контроллер для взаємодії із тренерами.
/// </summary>
/// <param name="userManager">Менеджер користувачів.</param>
/// <param name="mapper">Мапер об'єктів.</param>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{nameof(Roles.Seller)}")]
public class TrainerController(UserManager<User> userManager, IMapper mapper) : Controller
{
    /// <summary>
    /// Поміняти дані тренера.
    /// </summary>
    /// <param name="email">Мейл тренера.</param>
    /// <param name="trainerDto">Нові дані тренера.</param>
    /// <returns></returns>
    [HttpPut("{email}")]
    public async Task<IActionResult> Change(string email, TrainerDto trainerDto)
    {
        var trainer = await userManager.FindByEmailAsync(email);
        var roles = await userManager.GetRolesAsync(trainer!);
        
        if (roles[0].Equals(nameof(Roles.Trainer)))
        {
            var images = trainerDto.Images!.Select(imageDto => new Image 
            {
                Link = imageDto.Link
            }).ToList();
            
            trainer.Name = trainerDto.Name;
            trainer.Images = images;
            
            var result = await userManager.UpdateAsync(trainer);

            if (result.Succeeded)
            {
                return Ok(mapper.Map<TrainerDto>(trainer));
            }

            return BadRequest("User update failed");
        }

        return BadRequest($"Could not find a user with an email address {email}");
    }
}