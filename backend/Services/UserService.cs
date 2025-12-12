using Eventra.Data;
using Eventra.DTOs;
using Eventra.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Eventra.Services
{
    public class UserService(AppDbContext context, JwtService jwtService) : IUserService
    {
        private readonly AppDbContext _context = context;
        private readonly JwtService _jwtService = jwtService;


        // register user
        public async Task<User> RegisterAsync(RegisterUserDTO dto)
        {
            // Check if email already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);
            
            if (existingUser != null)
                throw new InvalidOperationException("Email is already registered.");

            // Create the appropriate user type based on UserType enum
            User user = dto.UserType switch
            {
                Enums.UserType.Admin => new Admin
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
                },
                Enums.UserType.Organizer => new Organizer
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
                },
                Enums.UserType.Participant => new Participant
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
                },
                _ => throw new InvalidOperationException("Invalid user type") // Nunca será atingido (validado no DTO)
            };

            // Add to DbSet
            await _context.Users.AddAsync(user);
            
            // Save changes to database
            await _context.SaveChangesAsync();
            
            // Return the entity with generated Id
            return user;
        }


        // login user
        public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            // Generate JWT token
            string accessToken = _jwtService.GenerateToken(
                user.Id, 
                user.Email, 
                user.UserType.ToString()
            );

            // Generate refresh token
            var refreshToken = _jwtService.GenerateRefreshToken(user.Id);
            
            // Save refresh token to database
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            var userResponse = new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType
            };

            return new AuthResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                User = userResponse
            };
        }

        // refresh token - generate new tokens
        public async Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken)
        {
            // Find the refresh token in database
            var storedToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            // Validate token
            if (storedToken == null)
                throw new UnauthorizedAccessException("Invalid refresh token.");

            if (!storedToken.IsActive)
                throw new UnauthorizedAccessException("Token expired or revoked.");

            // Revoke old token (rotation - each token can only be used once)
            storedToken.RevokedAt = DateTime.UtcNow;

            // Generate new tokens
            var user = storedToken.User;
            var newAccessToken = _jwtService.GenerateToken(user.Id, user.Email, user.UserType.ToString());
            var newRefreshToken = _jwtService.GenerateRefreshToken(user.Id);

            // Save new refresh token
            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            return new AuthResponseDTO
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token,
                User = new UserResponseDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    UserType = user.UserType
                }
            };
        }

        // logout - revoke refresh token
        public async Task LogoutAsync(string refreshToken)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (token != null)
            {
                token.RevokedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        // user by id
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        // all users
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }


        // update user
        public async Task UpdateAsync(int id, UpdateUserDTO dto)
        {
            var user = await _context.Users.FindAsync(id) 
                ?? throw new KeyNotFoundException("User not found.");
            
            if (!string.IsNullOrWhiteSpace(dto.Name))
                user.Name = dto.Name;
            
            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                // Check if new email is already taken by another user
                var emailExists = await _context.Users
                    .AnyAsync(u => u.Email == dto.Email && u.Id != id);
                
                if (emailExists)
                    throw new InvalidOperationException("Email is already in use.");
                
                user.Email = dto.Email;
            }
            
            if (!string.IsNullOrWhiteSpace(dto.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            
            await _context.SaveChangesAsync();
        }

        // delete user
        public async Task DeleteAsync(int id)
        {
            // start transaction - all or nothing
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                var user = await _context.Users.FindAsync(id) 
                    ?? throw new KeyNotFoundException("User not found.");

                // If user is a Participant, delete all their registrations
                if (user is Participant participant)
                {
                    var registrations = await _context.Registrations
                        .Where(r => r.ParticipantId == id)
                        .ToListAsync();

                    foreach (var registration in registrations)
                    {
                        // Decrement occupied slots for each event
                        var eventEntity = await _context.Events.FindAsync(registration.EventId);
                        if (eventEntity != null && eventEntity.OccupiedSlots > 0)
                        {
                            eventEntity.OccupiedSlots--;
                        }
                    }

                    _context.Registrations.RemoveRange(registrations);
                }

                // If user is an Organizer, delete all their events (cascade will handle registrations)
                if (user is Organizer organizer)
                {
                    var events = await _context.Events
                        .Include(e => e.Registrations)
                        .Where(e => e.OrganizerId == id)
                        .ToListAsync();

                    // Remove all registrations for these events first
                    foreach (var eventEntity in events)
                    {
                        if (eventEntity.Registrations.Count != 0)
                        {
                            _context.Registrations.RemoveRange(eventEntity.Registrations);
                        }
                    }

                    _context.Events.RemoveRange(events);
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync(); // Confirm the transaction
            }
            catch
            {
                await transaction.RollbackAsync(); // Rollback the transaction if an error occurs
                throw ;
            }
        }
    }
}