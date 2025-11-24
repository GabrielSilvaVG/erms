using Eventra.Data;
using Eventra.DTOs;
using Eventra.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventra.Services
{
    public class RegistrationService(AppDbContext context) : IRegistrationService
    {
        private readonly AppDbContext _context = context;


        // create registration
        public async Task<Registration> CreateAsync(int eventId, int participantId)
        {
            // Check if event exists
            var eventEntity = await _context.Events.FindAsync(eventId)
                ?? throw new KeyNotFoundException($"Event with ID {eventId} not found.");

            // Check if participant exists
            var participant = await _context.Participants.FindAsync(participantId)
                ?? throw new KeyNotFoundException($"Participant with ID {participantId} not found.");

            // Check if participant is already registered for this event
            var existingRegistration = await _context.Registrations
                .FirstOrDefaultAsync(r => r.EventId == eventId && r.ParticipantId == participantId);
            
            if (existingRegistration != null)
                throw new InvalidOperationException($"Participant is already registered for this event.");

            // Check if event has available slots
            if (eventEntity.OccupiedSlots >= eventEntity.TotalSlots)
                throw new InvalidOperationException($"Event is full. No available slots.");

            var registration = new Registration
            {
                EventId = eventId,
                ParticipantId = participantId,
                RegistrationDate = DateTime.UtcNow
            };

            // Increment occupied slots
            eventEntity.OccupiedSlots++;

            await _context.Registrations.AddAsync(registration);
            await _context.SaveChangesAsync();

            return registration;
        }

        // all registrations
        public async Task<IEnumerable<Registration>> GetAllAsync() 
        {
            var registrations = await _context.Registrations
                .Include(r => r.Event)
                .Include(r => r.Participant)
                .ToListAsync();
            return registrations;
        }

        // all registrations for an event
        public async Task<IEnumerable<Registration>> GetByEventIdAsync(int eventId)
        {
            var registrations = await _context.Registrations
                .Include(r => r.Event)
                .Include(r => r.Participant)
                .Where(r => r.EventId == eventId)
                .ToListAsync();
            return registrations;
        }

        // registration by id
        public async Task<Registration?> GetByIdAsync(int id)
        {
            return await _context.Registrations
                .Include(r => r.Event)
                .Include(r => r.Participant)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        // all registrations for a participant
        public async Task<IEnumerable<Registration>> GetByParticipantIdAsync(int participantId)
        {
            var registrations = await _context.Registrations
                .Include(r => r.Event)
                .Include(r => r.Participant)
                .Where(r => r.ParticipantId == participantId)
                .ToListAsync();
            return registrations;
        }

        // delete registration
        public async Task DeleteAsync(int id)
        {
            var registration = await _context.Registrations.FindAsync(id) 
                ?? throw new KeyNotFoundException($"Registration with ID {id} not found.");
            
            // Get the event and decrement occupied slots
            var eventEntity = await _context.Events.FindAsync(registration.EventId);
            if (eventEntity != null && eventEntity.OccupiedSlots > 0)
            {
                eventEntity.OccupiedSlots--;
            }

            _context.Registrations.Remove(registration);
            await _context.SaveChangesAsync();
        }
    }
}