using Eventra.Models;
using Eventra.Enums;
using Microsoft.EntityFrameworkCore;

namespace Eventra.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Organizer> Organizers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Registration> Registrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura herança TPH usando o enum UserType
            modelBuilder.Entity<User>()
                .HasDiscriminator(u => u.UserType)
                .HasValue<Admin>(UserType.Admin)
                .HasValue<Organizer>(UserType.Organizer)
                .HasValue<Participant>(UserType.Participant);

            // Seed do Admin padrão
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = 1,
                    Name = "Administrator",
                    Email = "admin@Eventra.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123")
                }
            );
        }
    }
}