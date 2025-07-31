using System.ComponentModel.DataAnnotations;

namespace SimpleGateway.Models
{
    public class FinalSignOffModel
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string PerformerUsername { get; set; } = string.Empty;
        
        // Performer Declaration
        public bool PerformerDeclarationSigned { get; set; } = false;
        public string? PerformerSignedBy { get; set; }
        public DateTime? PerformerSignedAt { get; set; }
        
        // Supervisor Declaration  
        public bool SupervisorDeclarationSigned { get; set; } = false;
        public string? SupervisorSignedBy { get; set; }
        public DateTime? SupervisorSignedAt { get; set; }
        
        // Advisor Declaration
        public bool AdvisorDeclarationSigned { get; set; } = false;
        public string? AdvisorSignedBy { get; set; }
        public DateTime? AdvisorSignedAt { get; set; }
        
        // Overall completion status
        public bool IsFullyCompleted => PerformerDeclarationSigned && SupervisorDeclarationSigned && AdvisorDeclarationSigned;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
