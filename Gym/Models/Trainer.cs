using System.ComponentModel.DataAnnotations;

namespace Gym.Models;

public class Trainer
{
    [Key]
    public int Id { get; set; }

    public string UserId { get; set; }
}