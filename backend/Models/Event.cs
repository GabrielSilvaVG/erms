using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eventra.Enums;

namespace Eventra.Models
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(60)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public EventType Type { get; set; }

        [Required]
        [StringLength(255)]
        public string PlaceId { get; set; } = string.Empty;  // Google Place ID

        [Required]
        public EventStatus Status { get; set; } = EventStatus.Scheduled;

        [Required]
        public DateTime Date { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public int TotalSlots { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
        public int OccupiedSlots { get; set; } = 0;

        [Required]
        public int OrganizerId { get; set; }

        [ForeignKey("OrganizerId")]
        public Organizer Organizer { get; set; } = null!;

        public ICollection<Registration> Registrations { get; set; } = [];
    }
}