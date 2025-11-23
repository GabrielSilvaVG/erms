using ERMS.DTOs;
using ERMS.Models;

namespace ERMS.Services
{
    public interface IUserService
    {
        Task<User> RegisterAsync(RegisterUserDTO dto);
        Task<AuthResponseDTO?> LoginAsync(LoginDTO dto);
        Task<User?> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task UpdateAsync(int id, UpdateUserDTO dto);
        Task DeleteAsync(int id);
    }
}
