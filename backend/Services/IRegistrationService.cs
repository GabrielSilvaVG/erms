using Eventra.Models;

namespace Eventra.Services
{
    public interface IRegistrationService
    {
        Task<Registration> CreateAsync(int eventId, int participantId);
        Task<Registration?> GetByIdAsync(int id);
        Task<IEnumerable<Registration>> GetAllAsync();
        Task<IEnumerable<Registration>> GetByEventIdAsync(int eventId);
        Task<IEnumerable<Registration>> GetByParticipantIdAsync(int participantId);
        Task DeleteAsync(int id);
    }
}