using Eventra.DTOs;
using Eventra.Models;

namespace Eventra.Services
{
    public interface IEventService
    {
        Task<Event> CreateAsync(CreateEventDTO dto);
        Task<Event?> GetByIdAsync(int id);
        Task<IEnumerable<Event>> GetAllAsync();
        Task UpdateAsync(int id, UpdateEventDTO dto);
        Task DeleteAsync(int id);
    }
}