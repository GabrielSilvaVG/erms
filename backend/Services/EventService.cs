using ERMS.Data;
using ERMS.DTOs;
using ERMS.Models;

namespace ERMS.Services
{
    public class EventService(AppDbContext context) : IEventService
    {
        private readonly AppDbContext _context = context;

        public Task<Event> CreateAsync(CreateEventDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Event>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Event?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int id, UpdateEventDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}