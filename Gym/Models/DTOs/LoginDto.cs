namespace Gym.Models.DTOs;

public class LoginDto
{
    /// <example>company@company.com</example>
    public required string Email { get; set; }
    /// <example>Test1234,</example>
    public required string Password { get; set; }
}