using System.ComponentModel.DataAnnotations;
using ERMS.Enums;

namespace ERMS.DTOs
{
    // DTO for creating a new event
    public class CreateEventDTO
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 60 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event type is required")]
        public EventType Type { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Location must be between 3 and 100 characters")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Total slots is required")]
        [Range(1, 10000, ErrorMessage = "Total slots must be between 1 and 10000")]
        public int TotalSlots { get; set; }
    }

    // DTO for updating an event
    public class UpdateEventDTO
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 60 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event type is required")]
        public EventType Type { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Location must be between 3 and 100 characters")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event status is required")]
        public EventStatus Status { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Total slots is required")]
        [Range(1, 10000, ErrorMessage = "Total slots must be between 1 and 10000")]
        public int TotalSlots { get; set; }
    }

    // DTO for returning event data
    public class EventResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public EventType Type { get; set; }
        public string Location { get; set; } = string.Empty;
        public EventStatus Status { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public int TotalSlots { get; set; }
        public int OccupiedSlots { get; set; }
        public int AvailableSlots => TotalSlots - OccupiedSlots;
        public OrganizerInfoDTO Organizer { get; set; } = null!;
    }

    // DTO for organizer info inside event response
    public class OrganizerInfoDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}