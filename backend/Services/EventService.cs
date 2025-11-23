using ERMS.Data;
using ERMS.DTOs;
using ERMS.Models;
using Microsoft.EntityFrameworkCore;

namespace ERMS.Services
{
    public class EventService(AppDbContext context) : IEventService
    {
        private readonly AppDbContext _context = context;

        // create event
        public async Task<Event> CreateAsync(CreateEventDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Event data cannot be null");
            var newEvent = new Event
            {
                Title = dto.Title,
                Type = dto.Type,
                Location = dto.Location,
                Date = dto.Date,
                Description = dto.Description,
                TotalSlots = dto.TotalSlots,
                Status = Enums.EventStatus.Scheduled // Default status for new events
            };
            await _context.Events.AddAsync(newEvent);
            await _context.SaveChangesAsync();
            return newEvent;
        }

        // event by id
        public async Task<Event?> GetByIdAsync(int id)
        {
            return await _context.Events
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // all events
        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            var events = await _context.Events
                .Include(e => e.Organizer)
                .ToListAsync(); 
            return events;
        }

        // update event
        public async Task UpdateAsync(int id, UpdateEventDTO dto)
        {
            var eventToUpdate = _context.Events.Find(id) ?? throw new KeyNotFoundException($"Event with ID {id} not found.");

            if (!string.IsNullOrEmpty(dto.Title))
            {
                var titleExists = await _context.Events
                    .AnyAsync(e => e.Title == dto.Title && e.Id != id);
                if (titleExists)
                    throw new InvalidOperationException($"An event with the title '{dto.Title}' already exists.");
                eventToUpdate.Title = dto.Title;
            }
            if (dto.Type.HasValue)
                eventToUpdate.Type = dto.Type.Value;
            if (!string.IsNullOrEmpty(dto.Location))
                eventToUpdate.Location = dto.Location;
            if (dto.Status.HasValue)
                eventToUpdate.Status = dto.Status.Value;
            if (dto.Date.HasValue)
                eventToUpdate.Date = dto.Date.Value;
            if (!string.IsNullOrEmpty(dto.Description))
                eventToUpdate.Description = dto.Description;
            
            if (dto.TotalSlots.HasValue)
            {
                // Validate that new total slots is not less than occupied slots
                if (dto.TotalSlots.Value < eventToUpdate.OccupiedSlots)
                    throw new InvalidOperationException(
                        $"Cannot reduce total slots to {dto.TotalSlots.Value}. " +
                        $"There are already {eventToUpdate.OccupiedSlots} participants registered.");
                
                eventToUpdate.TotalSlots = dto.TotalSlots.Value;
            }
            
            await _context.SaveChangesAsync();
        }

        // delete event
        public async Task DeleteAsync(int id)
        {
            var eventToDelete = await _context.Events.FindAsync(id) ?? throw new KeyNotFoundException($"Event with ID {id} not found.");
            _context.Events.Remove(eventToDelete);
            await _context.SaveChangesAsync();
        }
    }
}