using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace SimpleGateway.Models
{
    public class EmploymentHistoryJob
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string JobTitle { get; set; } = "";
        public string Address { get; set; } = "";
        public bool TreatedAdults { get; set; }
        public bool TreatedChildren { get; set; }
    }

    public class Qualification
    {
        public string QualificationName { get; set; } = "";
        public string Country { get; set; } = "";
        public string Institution { get; set; } = "";
        public string Year { get; set; } = "";
    }

    public class ClinicalProcedureEntry
    {
        public string Category { get; set; } = "";
        public string Procedure { get; set; } = "";
        public string NumberCategory { get; set; } = "";
        public int? ConfidenceLevel { get; set; }
        public DateTime? DateLastProcedure { get; set; }
        public string ExtraInfo { get; set; } = "";
        public bool TrainingNeedIdentified { get; set; }
        public string AdvisorComment { get; set; } = "";
        public DateTime? AdvisorSignedOffAt { get; set; }
        public string AdvisorSignedOffBy { get; set; } = "";
    }

    public class PreviousExperienceModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        
        // Section 1: Basic Information
        public string GdcGapsExplanation { get; set; } = "";
        public string NhsExperience { get; set; } = "";
        public string FullTime { get; set; } = "";
        public string PartTimeDaysPerWeek { get; set; } = "";
        public string Years { get; set; } = "";
        public string Months { get; set; } = "";
        
        // Section 1: Collections stored as JSON
        public string QualificationsJson { get; set; } = "[]";
        public string EmploymentHistoryJson { get; set; } = "[]";
        
        // Section 2: Clinical Experience stored as JSON
        public string ClinicalExperienceJson { get; set; } = "[]";
        
        // Declarations
        public bool ApplicantConfirmed { get; set; }
        public DateTime? ApplicantConfirmedAt { get; set; }
        public bool IsSubmitted { get; set; }
        public DateTime? AdvisorDeclarationAt { get; set; }
        public string AdvisorDeclarationBy { get; set; } = "";
        public string AdvisorDeclarationComment { get; set; } = "";
        
        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Helper properties (not stored in database)
        public List<Qualification> Qualifications
        {
            get => string.IsNullOrEmpty(QualificationsJson) ? new() : JsonSerializer.Deserialize<List<Qualification>>(QualificationsJson) ?? new();
            set => QualificationsJson = JsonSerializer.Serialize(value);
        }
        
        public List<EmploymentHistoryJob> EmploymentHistory
        {
            get => string.IsNullOrEmpty(EmploymentHistoryJson) ? new() : JsonSerializer.Deserialize<List<EmploymentHistoryJob>>(EmploymentHistoryJson) ?? new();
            set => EmploymentHistoryJson = JsonSerializer.Serialize(value);
        }
        
        public List<ClinicalProcedureEntry> ClinicalExperience
        {
            get => string.IsNullOrEmpty(ClinicalExperienceJson) ? new() : JsonSerializer.Deserialize<List<ClinicalProcedureEntry>>(ClinicalExperienceJson) ?? new();
            set => ClinicalExperienceJson = JsonSerializer.Serialize(value);
        }
    }
}
