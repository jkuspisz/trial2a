using System.ComponentModel.DataAnnotations;

namespace SimpleGateway.Models
{
    public class TestDataModel
    {
        public int Id { get; set; }

        // Original fields - keeping for backward compatibility
        [Required(ErrorMessage = "Please specify how long you have worked in the UK")]
        [Display(Name = "How long have you worked in the UK (if any)?")]
        public string UKWorkExperience { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please specify when you last treated a patient")]
        [Display(Name = "When was the last time you treated a patient?")]
        public string LastPatientTreatment { get; set; } = string.Empty;

        // New Supervisor/Supporting Dentist Information Fields
        [Display(Name = "GDC Number")]
        public string? GDCNumber { get; set; }
        
        [Display(Name = "How many years have you been on the performers list?")]
        public string? YearsOnPerformersList { get; set; }
        
        [Display(Name = "What training courses have you attended for the procedures you want to provide?")]
        public string? TrainingCoursesAttended { get; set; }
        
        [Display(Name = "Do you currently supervise any dental care professionals? If so, what is your experience of providing supervision?")]
        public string? CurrentSupervisionExperience { get; set; }
        
        [Display(Name = "Do you have any current conditions or restrictions on your practice? If so, please detail.")]
        public string? CurrentConditionsRestrictions { get; set; }
        
        [Display(Name = "Are you compliant with your CPD requirements? If not, please detail.")]
        public string? CPDCompliance { get; set; }

        // Declaration fields
        [Display(Name = "Declaration Signed")]
        public bool? DeclarationSigned { get; set; }
        
        [Display(Name = "Declaration Signed Date")]
        public DateTime? DeclarationSignedDate { get; set; }
        
        [Display(Name = "Declaration Signed By")]
        public string? DeclarationSignedBy { get; set; }

        // For tracking who submitted this test data
        public string Username { get; set; } = string.Empty;
        
        // Audit fields
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
    }
}
