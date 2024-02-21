namespace Gym.Models.DTOs;

public class TrainerDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public List<ImageDto>? Images { get; set; } 
}