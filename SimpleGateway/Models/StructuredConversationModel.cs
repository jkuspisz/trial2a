using System.ComponentModel.DataAnnotations;

namespace SimpleGateway.Models
{
    public class StructuredConversationModel
    {
        public int Id { get; set; }

        [Display(Name = "Summary of clinical experience")]
        [DataType(DataType.MultilineText)]
        public string? ClinicalExperienceSummary { get; set; }

        [Display(Name = "Development needs identified (if any)")]
        [DataType(DataType.MultilineText)]
        public string? DevelopmentNeeds { get; set; }

        [Display(Name = "Supporting Dentist/Supervisor Summary of application/suitability and/or advice given")]
        [DataType(DataType.MultilineText)]
        public string? SupervisorSummary { get; set; }

        [Display(Name = "Any other comments from advisor")]
        [DataType(DataType.MultilineText)]
        public string? AdvisorComments { get; set; }

        // Audit fields
        public string Username { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
    }
}
