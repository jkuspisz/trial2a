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

        // Registration and Qualifications Fields
        [Display(Name = "GDC registration number")]
        public string? GDCRegistrationNumber { get; set; }
        
        [Display(Name = "Date of UK registration as a dentist")]
        public string? UKRegistrationDate { get; set; }
        
        [Display(Name = "GDC registration gaps explanation")]
        public string? GDCRegistrationGaps { get; set; }
        
        [Display(Name = "Primary qualification")]
        public string? PrimaryQualification { get; set; }
        
        [Display(Name = "Primary qualification country")]
        public string? PrimaryQualificationCountry { get; set; }
        
        [Display(Name = "Primary qualification institution")]
        public string? PrimaryQualificationInstitution { get; set; }
        
        [Display(Name = "Primary qualification year")]
        public string? PrimaryQualificationYear { get; set; }

        // Employment History Fields
        [Display(Name = "NHS experience")]
        public string? NHSExperience { get; set; }
        
        [Display(Name = "NHS experience years")]
        public string? NHSExperienceYears { get; set; }
        
        [Display(Name = "NHS experience months")]
        public string? NHSExperienceMonths { get; set; }
        
        [Display(Name = "NHS experience type")]
        public string? NHSExperienceType { get; set; }
        
        [Display(Name = "NHS experience days per week")]
        public string? NHSExperienceDaysPerWeek { get; set; }
        
        [Display(Name = "Most recent job from date")]
        public string? MostRecentJobFromDate { get; set; }
        
        [Display(Name = "Most recent job to date")]
        public string? MostRecentJobToDate { get; set; }
        
        [Display(Name = "Most recent job title")]
        public string? MostRecentJobTitle { get; set; }
        
        [Display(Name = "Most recent job address")]
        public string? MostRecentJobAddress { get; set; }
        
        [Display(Name = "Most recent job - adult patients")]
        public bool MostRecentJobAdultPatients { get; set; }
        
        [Display(Name = "Most recent job - child patients")]
        public bool MostRecentJobChildPatients { get; set; }

        // Dental Procedures Assessment - Based on CSV structure
        // Clinical Exam/Assessments
        [Display(Name = "Clinical Exam/Assessments - Number")]
        public int? ClinicalExamNumber { get; set; }
        
        [Display(Name = "Clinical Exam/Assessments - Confidence (1-6)")]
        [Range(1, 6, ErrorMessage = "Confidence must be between 1 and 6")]
        public int? ClinicalExamConfidence { get; set; }
        
        [Display(Name = "Clinical Exam/Assessments - Last Date")]
        public string? ClinicalExamLastDate { get; set; }
        
        [Display(Name = "Clinical Exam/Assessments - Need Support?")]
        public bool ClinicalExamNeedSupport { get; set; }

        // Basic Periodontal Examinations (BPE)
        [Display(Name = "BPE - Number")]
        public int? BPENumber { get; set; }
        
        [Display(Name = "BPE - Confidence (1-6)")]
        [Range(1, 6, ErrorMessage = "Confidence must be between 1 and 6")]
        public int? BPEConfidence { get; set; }
        
        [Display(Name = "BPE - Last Date")]
        public string? BPELastDate { get; set; }
        
        [Display(Name = "BPE - Need Support?")]
        public bool BPENeedSupport { get; set; }

        // 6-Point Pocket Chart (6PPC)
        [Display(Name = "6PPC - Number")]
        public int? SixPPCNumber { get; set; }
        
        [Display(Name = "6PPC - Confidence (1-6)")]
        [Range(1, 6, ErrorMessage = "Confidence must be between 1 and 6")]
        public int? SixPPCConfidence { get; set; }
        
        [Display(Name = "6PPC - Last Date")]
        public string? SixPPCLastDate { get; set; }
        
        [Display(Name = "6PPC - Need Support?")]
        public bool SixPPCNeedSupport { get; set; }

        // Radiographs
        [Display(Name = "Radiographs - Number")]
        public int? RadiographsNumber { get; set; }
        
        [Display(Name = "Radiographs - Confidence (1-6)")]
        [Range(1, 6, ErrorMessage = "Confidence must be between 1 and 6")]
        public int? RadiographsConfidence { get; set; }
        
        [Display(Name = "Radiographs - Last Date")]
        public string? RadiographsLastDate { get; set; }
        
        [Display(Name = "Radiographs - Need Support?")]
        public bool RadiographsNeedSupport { get; set; }

        // Extractions
        [Display(Name = "Extractions - Number")]
        public int? ExtractionsNumber { get; set; }
        
        [Display(Name = "Extractions - Confidence (1-6)")]
        [Range(1, 6, ErrorMessage = "Confidence must be between 1 and 6")]
        public int? ExtractionsConfidence { get; set; }
        
        [Display(Name = "Extractions - Last Date")]
        public string? ExtractionsLastDate { get; set; }
        
        [Display(Name = "Extractions - Need Support?")]
        public bool ExtractionsNeedSupport { get; set; }

        // Extraction with root division
        [Display(Name = "Root Division Extractions - Number")]
        public int? RootDivisionNumber { get; set; }
        
        [Display(Name = "Root Division Extractions - Confidence (1-6)")]
        [Range(1, 6, ErrorMessage = "Confidence must be between 1 and 6")]
        public int? RootDivisionConfidence { get; set; }
        
        [Display(Name = "Root Division Extractions - Last Date")]
        public string? RootDivisionLastDate { get; set; }
        
        [Display(Name = "Root Division Extractions - Need Support?")]
        public bool RootDivisionNeedSupport { get; set; }

        // Scaling/root debridement
        [Display(Name = "Scaling/Root Debridement - Number")]
        public int? ScalingNumber { get; set; }
        
        [Display(Name = "Scaling/Root Debridement - Confidence (1-6)")]
        [Range(1, 6, ErrorMessage = "Confidence must be between 1 and 6")]
        public int? ScalingConfidence { get; set; }
        
        [Display(Name = "Scaling/Root Debridement - Last Date")]
        public string? ScalingLastDate { get; set; }
        
        [Display(Name = "Scaling/Root Debridement - Need Support?")]
        public bool ScalingNeedSupport { get; set; }

        // Amalgam fillings
        [Display(Name = "Amalgam Fillings - Number")]
        public int? AmalgamNumber { get; set; }
        
        [Display(Name = "Amalgam Fillings - Confidence (1-6)")]
        [Range(1, 6, ErrorMessage = "Confidence must be between 1 and 6")]
        public int? AmalgamConfidence { get; set; }
        
        [Display(Name = "Amalgam Fillings - Last Date")]
        public string? AmalgamLastDate { get; set; }
        
        [Display(Name = "Amalgam Fillings - Need Support?")]
        public bool AmalgamNeedSupport { get; set; }

        // Composite fillings
        [Display(Name = "Composite Fillings - Number")]
        public int? CompositeNumber { get; set; }
        
        [Display(Name = "Composite Fillings - Confidence (1-6)")]
        [Range(1, 6, ErrorMessage = "Confidence must be between 1 and 6")]
        public int? CompositeConfidence { get; set; }
        
        [Display(Name = "Composite Fillings - Last Date")]
        public string? CompositeLastDate { get; set; }
        
        [Display(Name = "Composite Fillings - Need Support?")]
        public bool CompositeNeedSupport { get; set; }

        // Crowns/Veneers/Onlays
        [Display(Name = "Crowns/Veneers/Onlays - Number")]
        public int? CrownsNumber { get; set; }
        
        [Display(Name = "Crowns/Veneers/Onlays - Confidence (1-6)")]
        [Range(1, 6, ErrorMessage = "Confidence must be between 1 and 6")]
        public int? CrownsConfidence { get; set; }
        
        [Display(Name = "Crowns/Veneers/Onlays - Last Date")]
        public string? CrownsLastDate { get; set; }
        
        [Display(Name = "Crowns/Veneers/Onlays - Need Support?")]
        public bool CrownsNeedSupport { get; set; }

        // Dentures
        [Display(Name = "Dentures - Number")]
        public int? DenturesNumber { get; set; }
        
        [Display(Name = "Dentures - Confidence (1-6)")]
        [Range(1, 6, ErrorMessage = "Confidence must be between 1 and 6")]
        public int? DenturesConfidence { get; set; }
        
        [Display(Name = "Dentures - Last Date")]
        public string? DenturesLastDate { get; set; }
        
        [Display(Name = "Dentures - Need Support?")]
        public bool DenturesNeedSupport { get; set; }

        // Single Root Endo
        [Display(Name = "Single Root Endo - Number")]
        public int? SingleEndoNumber { get; set; }
        
        [Display(Name = "Single Root Endo - Confidence (1-6)")]
        [Range(1, 6, ErrorMessage = "Confidence must be between 1 and 6")]
        public int? SingleEndoConfidence { get; set; }
        
        [Display(Name = "Single Root Endo - Last Date")]
        public string? SingleEndoLastDate { get; set; }
        
        [Display(Name = "Single Root Endo - Need Support?")]
        public bool SingleEndoNeedSupport { get; set; }

        // Multi-Root Endo
        [Display(Name = "Multi-Root Endo - Number")]
        public int? MultiEndoNumber { get; set; }
        
        [Display(Name = "Multi-Root Endo - Confidence (1-6)")]
        [Range(1, 6, ErrorMessage = "Confidence must be between 1 and 6")]
        public int? MultiEndoConfidence { get; set; }
        
        [Display(Name = "Multi-Root Endo - Last Date")]
        public string? MultiEndoLastDate { get; set; }
        
        [Display(Name = "Multi-Root Endo - Need Support?")]
        public bool MultiEndoNeedSupport { get; set; }

        // Peads fillings
        [Display(Name = "Pediatric Fillings - Number")]
        public int? PeadsNumber { get; set; }
        
        [Display(Name = "Pediatric Fillings - Confidence (1-6)")]
        [Range(1, 6, ErrorMessage = "Confidence must be between 1 and 6")]
        public int? PeadsConfidence { get; set; }
        
        [Display(Name = "Pediatric Fillings - Last Date")]
        public string? PeadsLastDate { get; set; }
        
        [Display(Name = "Pediatric Fillings - Need Support?")]
        public bool PeadsNeedSupport { get; set; }

        // For tracking who submitted this test data
        public string Username { get; set; } = string.Empty;
        
        // Audit fields
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
    }
}
