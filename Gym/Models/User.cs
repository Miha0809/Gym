using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Gym.Models;

public class User : IdentityUser
{
    [Key]
    public int Id { get; set; }
    
    [StringLength(16, MinimumLength = 2)]
    [DataType(DataType.Text)]
    public string? Name { get; set; }
    
    public virtual Subscription Subscription { get; set; }
    
    public virtual Address? Address { get; set; }
    public virtual List<Image>? Images { get; set; } 
    public virtual List<Trainer>? Trainers { get; set; }
}