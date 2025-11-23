using ERMS.Data;
using ERMS.DTOs;
using ERMS.Models;

namespace ERMS.Services
{
    public class RegistrationService(AppDbContext context) : IRegistrationService
    {
        private readonly AppDbContext _context = context;

        public Task<Registration> CreateAsync(int eventId, int participantId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Registration>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Registration>> GetByEventIdAsync(int eventId)
        {
            throw new NotImplementedException();
        }

        public Task<Registration?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Registration>> GetByParticipantIdAsync(int participantId)
        {
            throw new NotImplementedException();
        }
    }
}