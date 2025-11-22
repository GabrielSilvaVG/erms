using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERMS.Models
{
    public class Registration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int EventId { get; set; }
        
        [ForeignKey("EventId")]
        public Event Event { get; set; } = null!;

        [Required]
        public int ParticipantId { get; set; }
        
        [ForeignKey("ParticipantId")]
        public Participant Participant { get; set; } = null!;
    }
}