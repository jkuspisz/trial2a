using System.ComponentModel.DataAnnotations;

namespace SimpleGateway.Models
{
    public class TestDataModel2
    {
        public int Id { get; set; }

        // Original fields (kept for backward compatibility)
        [Required(ErrorMessage = "Please specify how long you have worked in the UK")]
        [Display(Name = "How long have you worked in the UK (if any)?")]
        public string UKWorkExperience { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please specify when you last treated a patient")]
        [Display(Name = "When was the last time you treated a patient?")]
        public string LastPatientTreatment { get; set; } = string.Empty;

        // Registration and Qualifications
        [Display(Name = "GDC registration number")]
        public string? GDCRegistrationNumber { get; set; }

        [Display(Name = "Date of UK registration as a dentist")]
        public string? UKRegistrationDate { get; set; }

        [Display(Name = "Gaps in GDC registration explanation")]
        public string? GDCGapsExplanation { get; set; }

        // Professional Qualifications
        [Display(Name = "Primary dental qualification")]
        public string? PrimaryQualification { get; set; }

        [Display(Name = "Primary qualification country")]
        public string? PrimaryQualificationCountry { get; set; }

        [Display(Name = "Primary qualification institution")]
        public string? PrimaryQualificationInstitution { get; set; }

        [Display(Name = "Primary qualification year")]
        public string? PrimaryQualificationYear { get; set; }

        // Employment History
        [Display(Name = "Worked in NHS general dental practice")]
        public string? WorkedInNHS { get; set; }

        [Display(Name = "NHS work years")]
        public int? NHSWorkYears { get; set; }

        [Display(Name = "NHS work months")]
        public int? NHSWorkMonths { get; set; }

        [Display(Name = "NHS work full-time")]
        public bool NHSWorkFullTime { get; set; }

        [Display(Name = "NHS work part-time")]
        public bool NHSWorkPartTime { get; set; }

        [Display(Name = "NHS work days per week")]
        public int? NHSWorkDaysPerWeek { get; set; }

        // Confidence and Experience (simplified to text)
        [Display(Name = "Describe your confidence levels (1-6 scale) for different procedures")]
        [DataType(DataType.MultilineText)]
        public string? ConfidenceLevelsText { get; set; }

        // Last Procedure Dates (simplified to text)
        [Display(Name = "Describe when you last performed various procedures")]
        [DataType(DataType.MultilineText)]
        public string? LastProcedureDatesText { get; set; }

        // Training Needs (simplified to text)
        [Display(Name = "Describe what training you need in different areas")]
        [DataType(DataType.MultilineText)]
        public string? TrainingNeedsText { get; set; }

        // For tracking who submitted this test data
        public string Username { get; set; } = string.Empty;
        
        // Audit fields
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
    }
}
