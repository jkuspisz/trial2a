using System.ComponentModel.DataAnnotations;

namespace SimpleGateway.Models
{
    public class WorkBasedAssessmentModel
    {
        public int Id { get; set; }
        
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Assessment Type")]
        public string AssessmentType { get; set; } = string.Empty; // "DOPS" or "CBDs"
        
        [Required]
        [Display(Name = "Assessment Title")]
        public string Title { get; set; } = string.Empty;
        
        // Status tracking
        [Display(Name = "Status")]
        public string Status { get; set; } = "Draft"; // "Draft", "PerformerComplete", "Complete"
        
        // PERFORMER SECTION
        [Display(Name = "Date of Assessment")]
        [DataType(DataType.Date)]
        public DateTime? AssessmentDate { get; set; }
        
        [Display(Name = "Clinical Area/Setting")]
        public string? ClinicalArea { get; set; }
        
        [Display(Name = "Procedure/Case Details")]
        [DataType(DataType.MultilineText)]
        public string? ProcedureDetails { get; set; }
        
        [Display(Name = "Learning Objectives")]
        [DataType(DataType.MultilineText)]
        public string? LearningObjectives { get; set; }
        
        [Display(Name = "Self-Assessment Comments")]
        [DataType(DataType.MultilineText)]
        public string? PerformerComments { get; set; }
        
        [Display(Name = "Areas for Development")]
        [DataType(DataType.MultilineText)]
        public string? AreasForDevelopment { get; set; }
        
        // Performer submission tracking
        public bool IsPerformerSubmitted { get; set; } = false;
        public DateTime? PerformerSubmittedAt { get; set; }
        
        // SUPERVISOR SECTION
        [Display(Name = "Supervisor Name")]
        public string? SupervisorName { get; set; }
        
        [Display(Name = "Supervisor Role")]
        public string? SupervisorRole { get; set; }
        
        [Display(Name = "Overall Performance Rating")]
        public string? OverallRating { get; set; } // "Excellent", "Good", "Satisfactory", "Needs Improvement"
        
        [Display(Name = "Specific Skills Assessment")]
        [DataType(DataType.MultilineText)]
        public string? SkillsAssessment { get; set; }
        
        [Display(Name = "Supervisor Comments")]
        [DataType(DataType.MultilineText)]
        public string? SupervisorComments { get; set; }
        
        [Display(Name = "Recommendations")]
        [DataType(DataType.MultilineText)]
        public string? Recommendations { get; set; }
        
        [Display(Name = "Action Plan")]
        [DataType(DataType.MultilineText)]
        public string? ActionPlan { get; set; }
        
        // Supervisor submission tracking
        public bool IsSupervisorCompleted { get; set; } = false;
        public string? CompletedBySupervisor { get; set; }
        public DateTime? SupervisorCompletedAt { get; set; }
        
        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Helper method to determine display status
        public string GetDisplayStatus()
        {
            if (!IsPerformerSubmitted)
                return "Draft";
            else if (IsPerformerSubmitted && !IsSupervisorCompleted)
                return "Awaiting Supervisor";
            else
                return "Complete";
        }
        
        // Helper method to get status color
        public string GetStatusColor()
        {
            return GetDisplayStatus() switch
            {
                "Draft" => "secondary",
                "Awaiting Supervisor" => "warning",
                "Complete" => "success",
                _ => "secondary"
            };
        }
    }
}
