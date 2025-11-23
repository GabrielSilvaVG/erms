using ERMS.Data;
using ERMS.DTOs;
using ERMS.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace ERMS.Services
{
    public class UserService(AppDbContext context, JwtService jwtService) : IUserService
    {
        private readonly AppDbContext _context = context;
        private readonly JwtService _jwtService = jwtService;


        // register user
        public async Task<User> RegisterAsync(RegisterUserDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Registration data cannot be null");

            // Check if email already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);
            
            if (existingUser != null)
                throw new InvalidOperationException($"Email '{dto.Email}' is already registered.");

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
                _ => throw new ArgumentException($"Invalid user type: {dto.UserType}")
            };

            // Add to DbSet
            await _context.Users.AddAsync(user);
            
            // Save changes to database
            await _context.SaveChangesAsync();
            
            // Return the entity with generated Id
            return user;
        }


        // login user
        public async Task<AuthResponseDTO?> LoginAsync(LoginDTO dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return null; // Invalid credentials
            }

            // Generate JWT token
            string token = _jwtService.GenerateToken(
                user.Id, 
                user.Email, 
                user.UserType.ToString()
            );

            var userResponse = new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType
            };

            return new AuthResponseDTO
            {
                Token = token,
                User = userResponse
            };
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
                ?? throw new KeyNotFoundException($"User with ID {id} not found.");
            
            if (!string.IsNullOrWhiteSpace(dto.Name))
                user.Name = dto.Name;
            
            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                // Check if new email is already taken by another user
                var emailExists = await _context.Users
                    .AnyAsync(u => u.Email == dto.Email && u.Id != id);
                
                if (emailExists)
                    throw new InvalidOperationException($"Email '{dto.Email}' is already in use.");
                
                user.Email = dto.Email;
            }
            
            if (!string.IsNullOrWhiteSpace(dto.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            
            await _context.SaveChangesAsync();
        }

        // delete user
        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id) ?? throw new KeyNotFoundException($"User with ID {id} not found.");
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}