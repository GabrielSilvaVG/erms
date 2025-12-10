using Eventra.DTOs;
using Eventra.Models;

namespace Eventra.Services
{
    public interface IUserService
    {
        Task<User> RegisterAsync(RegisterUserDTO dto);
        Task<AuthResponseDTO> LoginAsync(LoginDTO dto);
        Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
        Task<User?> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task UpdateAsync(int id, UpdateUserDTO dto);
        Task DeleteAsync(int id);
    }
}
