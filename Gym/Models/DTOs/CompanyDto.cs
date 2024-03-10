namespace Gym.Models.DTOs;

public class CompanyDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required AddressDto Address { get; set; }
    public required List<ImageDto> Images { get; set; }
    public List<TrainerDto>? Trainers { get; set; }
}