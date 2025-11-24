using System.ComponentModel.DataAnnotations;

namespace Eventra.DTOs
{
    // DTO for creating a new registration (participant subscribing to event)
    public class CreateRegistrationDTO
    {
        [Required(ErrorMessage = "Event ID is required")]
        public int EventId { get; set; }
        
    }

    // DTO for returning registration data
    public class RegistrationResponseDTO
    {
        public int Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int EventId { get; set; }
        public string EventTitle { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string EventLocation { get; set; } = string.Empty;
        public int ParticipantId { get; set; }
        public string ParticipantName { get; set; } = string.Empty;
        public string ParticipantEmail { get; set; } = string.Empty;
    }

    // DTO for simplified event info in participant's registrations list
    public class EventInfoDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    // DTO for simplified participant info in event's registrations list
    public class ParticipantInfoDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
    }
}