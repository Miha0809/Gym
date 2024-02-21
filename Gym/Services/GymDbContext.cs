using Gym.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gym.Services;

public class GymDbContext : IdentityDbContext<User>
{
    public DbSet<User> Users { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Trainer> Trainers { get; set; }
    
    public DbSet<Image> Images { get; set; }
    public DbSet<Address> Addresses { get; set; }

    public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
    {
    }
}