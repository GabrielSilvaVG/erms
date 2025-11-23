using ERMS.Data;
using ERMS.DTOs;
using ERMS.Models;
using Microsoft.EntityFrameworkCore;

namespace ERMS.Services
{
    public class RegistrationService(AppDbContext context) : IRegistrationService
    {
        private readonly AppDbContext _context = context;


        // create registration
        public async Task<Registration> CreateAsync(int eventId, int participantId)
        {
            var registration = new Registration
            {
                EventId = eventId,
                ParticipantId = participantId,
                RegistrationDate = DateTime.UtcNow
            };

            await _context.Registrations.AddAsync(registration);
            await _context.SaveChangesAsync();

            return registration;
        }

        // all registrations
        public async Task<IEnumerable<Registration>> GetAllAsync() 
        {
            var registrations = await _context.Registrations.ToListAsync();
            return registrations;
        }

        // all registrations for an event
        public async Task<IEnumerable<Registration>> GetByEventIdAsync(int eventId)
        {
            var registrations = await _context.Registrations
                .Where(r => r.EventId == eventId)
                .ToListAsync();
            return registrations;
        }

        // registration by id
        public async Task<Registration?> GetByIdAsync(int id)// registration by id
        {
            var registration = await _context.Registrations.FindAsync(id) ?? 
            throw new KeyNotFoundException($"Registration with ID {id} not found.");
            return registration;
        }

        // all registrations for a participant
        public async Task<IEnumerable<Registration>> GetByParticipantIdAsync(int participantId)
        {
            var registrations = await _context.Registrations
                .Where(r => r.ParticipantId == participantId)
                .ToListAsync();
            return registrations;
        }

        // delete registration
        public async Task DeleteAsync(int id)
        {
            var registration = await _context.Registrations.FindAsync(id) ?? throw new KeyNotFoundException($"Registration with ID {id} not found.");
            _context.Registrations.Remove(registration);
            await _context.SaveChangesAsync();
        }
    }
}