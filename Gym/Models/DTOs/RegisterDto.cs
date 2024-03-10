namespace Gym.Models.DTOs;

public class RegisterDto
{
    /// <example>Adam Zawatski</example>
    public required string Name { get; set; }
    /// <example>buyer@gmail.com</example>
    public required string Email { get; set; }
    /// <example>Test1234,</example>
    public required string Password { get; set; }
    /// <example>Buyer</example>
    public required string Role { get; set; }
    
    /// <example>1</example>
    public required int SubscribeMonth { get; set; }
}