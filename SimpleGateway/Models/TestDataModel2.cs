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
        public bool? NHSWorkFullTime { get; set; }

        [Display(Name = "NHS work part-time")]
        public bool? NHSWorkPartTime { get; set; }

        [Display(Name = "NHS work days per week")]
        public int? NHSWorkDaysPerWeek { get; set; }

        // Clinical Experience Counts
        [Display(Name = "Simple extractions count")]
        public int? SimpleExtractionsCount { get; set; }

        [Display(Name = "Root division extractions count")]
        public int? RootDivisionExtractionsCount { get; set; }

        [Display(Name = "Clinical assessments count")]
        public int? ClinicalAssessmentsCount { get; set; }

        [Display(Name = "BPE examinations count")]
        public int? BPEExaminationsCount { get; set; }

        [Display(Name = "Pocket charts count")]
        public int? PocketChartsCount { get; set; }

        [Display(Name = "Radiographs count")]
        public int? RadiographsCount { get; set; }

        [Display(Name = "Deciduous teeth restored count")]
        public int? DeciduousTeethCount { get; set; }

        [Display(Name = "Mechanical debridement count")]
        public int? MechanicalDebridementCount { get; set; }

        [Display(Name = "Dentures provided count")]
        public int? DenturesCount { get; set; }

        [Display(Name = "Amalgam fillings count")]
        public int? AmalgamFillingsCount { get; set; }

        // Confidence Levels (1-6 scale)
        [Display(Name = "Simple extractions confidence")]
        public int? SimpleExtractionsConfidence { get; set; }

        [Display(Name = "Root division confidence")]
        public int? RootDivisionConfidence { get; set; }

        [Display(Name = "Clinical assessment confidence")]
        public int? ClinicalAssessmentConfidence { get; set; }

        [Display(Name = "BPE confidence")]
        public int? BPEConfidence { get; set; }

        [Display(Name = "Pocket charts confidence")]
        public int? PocketChartsConfidence { get; set; }

        [Display(Name = "Radiographs confidence")]
        public int? RadiographsConfidence { get; set; }

        [Display(Name = "Deciduous teeth confidence")]
        public int? DeciduousTeethConfidence { get; set; }

        [Display(Name = "Mechanical debridement confidence")]
        public int? MechanicalDebridementConfidence { get; set; }

        [Display(Name = "Dentures confidence")]
        public int? DenturesConfidence { get; set; }

        // Last Procedure Dates
        [Display(Name = "Simple extractions last date")]
        public string? SimpleExtractionsLastDate { get; set; }

        [Display(Name = "Root division last date")]
        public string? RootDivisionLastDate { get; set; }

        [Display(Name = "Clinical assessment last date")]
        public string? ClinicalAssessmentLastDate { get; set; }

        [Display(Name = "BPE last date")]
        public string? BPELastDate { get; set; }

        [Display(Name = "Pocket charts last date")]
        public string? PocketChartsLastDate { get; set; }

        [Display(Name = "Radiographs last date")]
        public string? RadiographsLastDate { get; set; }

        [Display(Name = "Deciduous teeth last date")]
        public string? DeciduousTeethLastDate { get; set; }

        [Display(Name = "Mechanical debridement last date")]
        public string? MechanicalDebridementLastDate { get; set; }

        [Display(Name = "Dentures last date")]
        public string? DenturesLastDate { get; set; }

        // Training Needs
        [Display(Name = "Simple extractions training needed")]
        public bool? SimpleExtractionsTraining { get; set; }

        [Display(Name = "Root division training needed")]
        public bool? RootDivisionTraining { get; set; }

        [Display(Name = "Clinical assessment training needed")]
        public bool? ClinicalAssessmentTraining { get; set; }

        [Display(Name = "BPE training needed")]
        public bool? BPETraining { get; set; }

        [Display(Name = "Pocket charts training needed")]
        public bool? PocketChartsTraining { get; set; }

        [Display(Name = "Radiographs training needed")]
        public bool? RadiographsTraining { get; set; }

        [Display(Name = "Deciduous teeth training needed")]
        public bool? DeciduousTeethTraining { get; set; }

        [Display(Name = "Mechanical debridement training needed")]
        public bool? MechanicalDebridementTraining { get; set; }

        [Display(Name = "Dentures training needed")]
        public bool? DenturesTraining { get; set; }

        // For tracking who submitted this test data
        public string Username { get; set; } = string.Empty;
        
        // Audit fields
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
    }
}
