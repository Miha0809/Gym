using System.ComponentModel.DataAnnotations;

namespace Gym.Models;

public class Image
{
    [Key]
    public int Id { get; set; }

    public string Link { get; set; }
}