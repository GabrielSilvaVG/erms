using System.ComponentModel.DataAnnotations;
using ERMS.Enums;

namespace ERMS.DTOs
{
    // DTO for registering a new user
    public class RegisterUserDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 60 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(60, ErrorMessage = "Email cannot exceed 60 characters")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;


        [Required(ErrorMessage = "User type is required")]
        [EnumDataType(typeof(UserType), ErrorMessage = "Invalid user type")]
        public UserType UserType { get; set; }
    }

    // DTO for login
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }

    // DTO for returning user data (without password)
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserType UserType { get; set; }
    }

    // DTO for updating user data (all fields optional)
    public class UpdateUserDTO
    {
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 60 characters")]
        public string? Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(60, ErrorMessage = "Email cannot exceed 60 characters")]
        public string? Email { get; set; }
        
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public string? Password { get; set; }
    }

    // DTO for authentication response with token
    public class AuthResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public UserResponseDTO User { get; set; } = null!;
    }
}
