using System.ComponentModel.DataAnnotations;

namespace SimpleGateway.Models
{
    public class ClinicalExperienceEntry
    {
        public string Username { get; set; } = "";
        public string Category { get; set; } = "";
        public string Procedure { get; set; } = "";
        
        [Display(Name = "Number of Cases")]
        public string NumberCategory { get; set; } = "";
        
        [Display(Name = "Confidence Level (1-6)")]
        [Range(1, 6, ErrorMessage = "Confidence level must be between 1 and 6")]
        public int? ConfidenceLevel { get; set; }
        
        [Display(Name = "Date of Last Procedure")]
        public DateTime? DateLastProcedure { get; set; }
        
        [Display(Name = "Extra Information")]
        public string ExtraInfo { get; set; } = "";
        
        [Display(Name = "Training Need Identified")]
        public bool TrainingNeedIdentified { get; set; }
        
        [Display(Name = "Advisor Comment")]
        public string AdvisorComment { get; set; } = "";
        
        public DateTime? AdvisorSignedOffAt { get; set; }
        public string AdvisorSignedOffBy { get; set; } = "";
    }

    public class ProcedureDefinition
    {
        public string Procedure { get; set; } = "";
        public List<string> NumberCategories { get; set; } = new();
    }

    public class ProcedureSection
    {
        public string Category { get; set; } = "";
        public List<ProcedureDefinition> Procedures { get; set; } = new();
    }

    public static class ProcedureSections
    {
        public static readonly List<ProcedureSection> All = new()
        {
            new ProcedureSection
            {
                Category = "Diagnosis & Assessment",
                Procedures = new()
                {
                    new() { Procedure = "Clinical assessments (patient examinations)", NumberCategories = new() { "0", "1-40", "+40" } },
                    new() { Procedure = "Basic Periodontal Examinations (BPE)", NumberCategories = new() { "0", "1-40", "+40" } },
                    new() { Procedure = "6-point pocket charts", NumberCategories = new() { "0", "1-40", "+40" } },
                    new() { Procedure = "Take own radiographs", NumberCategories = new() { "0", "1-40", "+40" } }
                }
            },
            new ProcedureSection
            {
                Category = "Restorative Dentistry",
                Procedures = new()
                {
                    new() { Procedure = "Fillings using amalgam", NumberCategories = new() { "0", "1-10", "+11" } },
                    new() { Procedure = "Fillings using composite resin", NumberCategories = new() { "0", "1-10", "+11" } },
                    new() { Procedure = "Crowns", NumberCategories = new() { "0", "1-10", "+11" } }
                }
            },
            new ProcedureSection
            {
                Category = "Prosthetics / Prosthodontics",
                Procedures = new()
                {
                    new() { Procedure = "Dentures provided", NumberCategories = new() { "0-5", "+6" } }
                }
            },
            new ProcedureSection
            {
                Category = "Periodontics",
                Procedures = new()
                {
                    new() { Procedure = "Mechanical debridement procedures", NumberCategories = new() { "0-5", "6-10", "+10" } }
                }
            },
            new ProcedureSection
            {
                Category = "Paediatric Dentistry",
                Procedures = new()
                {
                    new() { Procedure = "Deciduous teeth restored", NumberCategories = new() { "0-5", "6-10", "+10" } }
                }
            },
            new ProcedureSection
            {
                Category = "Extractions & Oral Surgery",
                Procedures = new()
                {
                    new() { Procedure = "Simple extractions", NumberCategories = new() { "0-5", "6-10", "+10" } },
                    new() { Procedure = "Extractions including root division", NumberCategories = new() { "0-5", "6-10", "+10" } }
                }
            },
            new ProcedureSection
            {
                Category = "Endodontics",
                Procedures = new()
                {
                    new() { Procedure = "Single rooted teeth", NumberCategories = new() { "0", "1-4", "+5" } },
                    new() { Procedure = "Molars", NumberCategories = new() { "0", "1-4", "+5" } }
                }
            }
        };
    }
}
