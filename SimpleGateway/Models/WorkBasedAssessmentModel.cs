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
        
        // PERFORMER SECTION - Customized Questions
        [Display(Name = "Date of Assessment")]
        [DataType(DataType.Date)]
        public DateTime? AssessmentDate { get; set; }
        
        [Display(Name = "Description of Procedure/Case")]
        [DataType(DataType.MultilineText)]
        public string? ProcedureDescription { get; set; }
        
        [Display(Name = "What did you learn from this encounter? - What do you think you did well? What could have gone better?")]
        [DataType(DataType.MultilineText)]
        public string? LearningReflection { get; set; }
        
        [Display(Name = "What (if any) learning needs does this encounter highlight for you?")]
        [DataType(DataType.MultilineText)]
        public string? LearningNeeds { get; set; }
        
        // Performer submission tracking
        public bool IsPerformerSubmitted { get; set; } = false;
        public DateTime? PerformerSubmittedAt { get; set; }
        
        // SUPERVISOR SECTION - Simplified to match new requirements
        [Display(Name = "Overall was this encounter acceptable or not acceptable?")]
        public bool? OverallAcceptable { get; set; } // true = Acceptable, false = Not Acceptable
        
        [Display(Name = "Overall comment on the encounter and provide an agreed action plan (if needed). It is useful to include any resources the performer could reference.")]
        [DataType(DataType.MultilineText)]
        public string? SupervisorActionPlan { get; set; }
        
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
