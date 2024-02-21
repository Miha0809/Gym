namespace Gym.Models.DTOs;

public class UserDto
{
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required SubscriptionDto Subscription { get; set; }
}