using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Domain.Models.Users;
using System.Security.Claims;

namespace DataLayer
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                    .UseSqlServer(Environment.GetEnvironmentVariable("ConnectionString"))
                    .LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
            .HasIndex(e => e.Email)
            .IsUnique();
        }

        public DbSet<User> Users { get; set; }
    }
}