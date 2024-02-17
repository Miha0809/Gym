using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Gym.Models;

public class User : IdentityUser
{
    [StringLength(16, MinimumLength = 2)]
    [DataType(DataType.Text)]
    public string? FirstName { get; set; }
    
    [StringLength(32, MinimumLength = 2)]
    [DataType(DataType.Text)]
    public string? LastName { get; set; }
    
    public virtual Subscription Subscription { get; set; } 
}