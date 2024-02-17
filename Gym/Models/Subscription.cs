using System.ComponentModel.DataAnnotations;

namespace Gym.Models;

public class Subscription
{
    [Key]
    public int Id { get; set; }

    [DataType(DataType.Text)]
    public string Start { get; set; } = DateTime.UtcNow.ToShortDateString();

    [DataType(DataType.Text)]
    public string End { get; set; }
}