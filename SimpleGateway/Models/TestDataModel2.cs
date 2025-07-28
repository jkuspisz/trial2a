using System.ComponentModel.DataAnnotations;

namespace SimpleGateway.Models
{
    public class TestDataModel2
    {
        public int Id { get; set; }

        // Original fields (kept for backward compatibility)
        [Display(Name = "How long have you worked in the UK (if any)?")]
        public string UKWorkExperience { get; set; } = string.Empty;

        [Display(Name = "When was the last time you treated a patient?")]
        public string LastPatientTreatment { get; set; } = string.Empty;

        // Registration and Qualifications Fields
        [Display(Name = "GDC registration number")]
        public string? GDCRegistrationNumber { get; set; }
        
        [Display(Name = "Date of UK registration as a dentist")]
        public string? UKRegistrationDate { get; set; }
        
        [Display(Name = "GDC registration gaps explanation")]
        public string? GDCRegistrationGaps { get; set; }
        
        // Qualifications (6 entries)
        [Display(Name = "Qualification 1")]
        public string? Qualification1 { get; set; }
        [Display(Name = "Qualification 1 country")]
        public string? Qualification1Country { get; set; }
        [Display(Name = "Qualification 1 institution")]
        public string? Qualification1Institution { get; set; }
        [Display(Name = "Qualification 1 year")]
        public string? Qualification1Year { get; set; }

        [Display(Name = "Qualification 2")]
        public string? Qualification2 { get; set; }
        [Display(Name = "Qualification 2 country")]
        public string? Qualification2Country { get; set; }
        [Display(Name = "Qualification 2 institution")]
        public string? Qualification2Institution { get; set; }
        [Display(Name = "Qualification 2 year")]
        public string? Qualification2Year { get; set; }

        [Display(Name = "Qualification 3")]
        public string? Qualification3 { get; set; }
        [Display(Name = "Qualification 3 country")]
        public string? Qualification3Country { get; set; }
        [Display(Name = "Qualification 3 institution")]
        public string? Qualification3Institution { get; set; }
        [Display(Name = "Qualification 3 year")]
        public string? Qualification3Year { get; set; }

        [Display(Name = "Qualification 4")]
        public string? Qualification4 { get; set; }
        [Display(Name = "Qualification 4 country")]
        public string? Qualification4Country { get; set; }
        [Display(Name = "Qualification 4 institution")]
        public string? Qualification4Institution { get; set; }
        [Display(Name = "Qualification 4 year")]
        public string? Qualification4Year { get; set; }

        [Display(Name = "Qualification 5")]
        public string? Qualification5 { get; set; }
        [Display(Name = "Qualification 5 country")]
        public string? Qualification5Country { get; set; }
        [Display(Name = "Qualification 5 institution")]
        public string? Qualification5Institution { get; set; }
        [Display(Name = "Qualification 5 year")]
        public string? Qualification5Year { get; set; }

        [Display(Name = "Qualification 6")]
        public string? Qualification6 { get; set; }
        [Display(Name = "Qualification 6 country")]
        public string? Qualification6Country { get; set; }
        [Display(Name = "Qualification 6 institution")]
        public string? Qualification6Institution { get; set; }
        [Display(Name = "Qualification 6 year")]
        public string? Qualification6Year { get; set; }

        // Employment History (6 entries) - Simplified without NHS-specific questions
        [Display(Name = "Job 1 from date")]
        public string? Job1FromDate { get; set; }
        [Display(Name = "Job 1 to date")]
        public string? Job1ToDate { get; set; }
        [Display(Name = "Job 1 title")]
        public string? Job1Title { get; set; }
        [Display(Name = "Job 1 address")]
        public string? Job1Address { get; set; }
        [Display(Name = "Job 1 type")]
        public string? Job1Type { get; set; } // Private or NHS
        [Display(Name = "Job 1 adult patients")]
        public bool Job1AdultPatients { get; set; }
        [Display(Name = "Job 1 child patients")]
        public bool Job1ChildPatients { get; set; }

        [Display(Name = "Job 2 from date")]
        public string? Job2FromDate { get; set; }
        [Display(Name = "Job 2 to date")]
        public string? Job2ToDate { get; set; }
        [Display(Name = "Job 2 title")]
        public string? Job2Title { get; set; }
        [Display(Name = "Job 2 address")]
        public string? Job2Address { get; set; }
        [Display(Name = "Job 2 type")]
        public string? Job2Type { get; set; } // Private or NHS
        [Display(Name = "Job 2 adult patients")]
        public bool Job2AdultPatients { get; set; }
        [Display(Name = "Job 2 child patients")]
        public bool Job2ChildPatients { get; set; }

        [Display(Name = "Job 3 from date")]
        public string? Job3FromDate { get; set; }
        [Display(Name = "Job 3 to date")]
        public string? Job3ToDate { get; set; }
        [Display(Name = "Job 3 title")]
        public string? Job3Title { get; set; }
        [Display(Name = "Job 3 address")]
        public string? Job3Address { get; set; }
        [Display(Name = "Job 3 type")]
        public string? Job3Type { get; set; } // Private or NHS
        [Display(Name = "Job 3 adult patients")]
        public bool Job3AdultPatients { get; set; }
        [Display(Name = "Job 3 child patients")]
        public bool Job3ChildPatients { get; set; }

        [Display(Name = "Job 4 from date")]
        public string? Job4FromDate { get; set; }
        [Display(Name = "Job 4 to date")]
        public string? Job4ToDate { get; set; }
        [Display(Name = "Job 4 title")]
        public string? Job4Title { get; set; }
        [Display(Name = "Job 4 address")]
        public string? Job4Address { get; set; }
        [Display(Name = "Job 4 type")]
        public string? Job4Type { get; set; } // Private or NHS
        [Display(Name = "Job 4 adult patients")]
        public bool Job4AdultPatients { get; set; }
        [Display(Name = "Job 4 child patients")]
        public bool Job4ChildPatients { get; set; }

        [Display(Name = "Job 5 from date")]
        public string? Job5FromDate { get; set; }
        [Display(Name = "Job 5 to date")]
        public string? Job5ToDate { get; set; }
        [Display(Name = "Job 5 title")]
        public string? Job5Title { get; set; }
        [Display(Name = "Job 5 address")]
        public string? Job5Address { get; set; }
        [Display(Name = "Job 5 type")]
        public string? Job5Type { get; set; } // Private or NHS
        [Display(Name = "Job 5 adult patients")]
        public bool Job5AdultPatients { get; set; }
        [Display(Name = "Job 5 child patients")]
        public bool Job5ChildPatients { get; set; }

        [Display(Name = "Job 6 from date")]
        public string? Job6FromDate { get; set; }
        [Display(Name = "Job 6 to date")]
        public string? Job6ToDate { get; set; }
        [Display(Name = "Job 6 title")]
        public string? Job6Title { get; set; }
        [Display(Name = "Job 6 address")]
        public string? Job6Address { get; set; }
        [Display(Name = "Job 6 type")]
        public string? Job6Type { get; set; } // Private or NHS
        [Display(Name = "Job 6 adult patients")]
        public bool Job6AdultPatients { get; set; }
        [Display(Name = "Job 6 child patients")]
        public bool Job6ChildPatients { get; set; }

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

        // Advisor-only comment field
        [Display(Name = "Advisor Comments")]
        public string? AdvisorComment { get; set; }

        // For tracking who submitted this test data
        public string Username { get; set; } = string.Empty;
        
        // Audit fields
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
    }
}
