using System.ComponentModel.DataAnnotations;

namespace SimpleGateway.Models
{
    public class TestDataModel2Simple
    {
        public int Id { get; set; }

        // Basic Info (keep original fields for compatibility)
        [Required(ErrorMessage = "Please specify how long you have worked in the UK")]
        [Display(Name = "How long have you worked in the UK (if any)?")]
        public string UKWorkExperience { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please specify when you last treated a patient")]
        [Display(Name = "When was the last time you treated a patient?")]
        public string LastPatientTreatment { get; set; } = string.Empty;

        // Simple text-based comprehensive assessment
        [Display(Name = "Registration and Qualifications")]
        [DataType(DataType.MultilineText)]
        public string? RegistrationQualifications { get; set; }

        [Display(Name = "NHS Experience and Employment History")]
        [DataType(DataType.MultilineText)]
        public string? NHSExperience { get; set; }

        [Display(Name = "Clinical Experience - Procedures Performed")]
        [DataType(DataType.MultilineText)]
        public string? ClinicalExperience { get; set; }

        [Display(Name = "Confidence Levels and Areas of Strength")]
        [DataType(DataType.MultilineText)]
        public string? ConfidenceLevels { get; set; }

        [Display(Name = "Training Needs and Development Areas")]
        [DataType(DataType.MultilineText)]
        public string? TrainingNeeds { get; set; }

        [Display(Name = "Additional Comments or Information")]
        [DataType(DataType.MultilineText)]
        public string? AdditionalComments { get; set; }

        // Audit fields
        public string Username { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
    }
}
