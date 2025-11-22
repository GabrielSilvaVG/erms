using ERMS.Data;
using ERMS.Models;

namespace ERMS.Services
{
    public class EventService(AppDbContext context) : IGenericService<Event, int>
    {
        private readonly AppDbContext _context = context;

        public Task<Event> InsertAsync(Event entity)
        {
            throw new NotImplementedException();
        }
        public Task<Event?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Event>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Event entity)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}