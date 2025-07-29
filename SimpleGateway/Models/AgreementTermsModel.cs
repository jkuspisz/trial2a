using System.ComponentModel.DataAnnotations;

namespace SimpleGateway.Models
{
    public class AgreementTermsModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;

        [Display(Name = "Agreement Terms Details")]
        [DataType(DataType.MultilineText)]
        public string? AgreementTermsDetails { get; set; }

        [Display(Name = "Educational Support Needs")]
        [DataType(DataType.MultilineText)]
        public string? EducationalSupportNeeds { get; set; }

        [Display(Name = "Clinical Support Needs")]
        [DataType(DataType.MultilineText)]
        public string? ClinicalSupportNeeds { get; set; }

        [Display(Name = "Review Progress Notes")]
        [DataType(DataType.MultilineText)]
        public string? ReviewProgressNotes { get; set; }

        [Display(Name = "Evidence Submission Status")]
        [DataType(DataType.MultilineText)]
        public string? EvidenceSubmissionStatus { get; set; }

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
