using AutoMapper;
using Gym.Models;
using Gym.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gym.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{nameof(Roles.Seller)}")]
public class TrainerController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper) : Controller
{
    /// <summary>
    /// Поміняти данні тренера.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="trainerDto"></param>
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