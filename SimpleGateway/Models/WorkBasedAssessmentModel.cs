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
        // Date fields - always ensure UTC for PostgreSQL compatibility
        private DateTime? _assessmentDate;
        [Display(Name = "Date of Assessment")]
        [DataType(DataType.Date)]
        public DateTime? AssessmentDate 
        { 
            get => _assessmentDate;
            set => _assessmentDate = value?.Kind == DateTimeKind.Utc ? value : DateTime.SpecifyKind(value ?? DateTime.UtcNow, DateTimeKind.Utc);
        }
        
        private DateTime? _performerSubmittedAt;
        public DateTime? PerformerSubmittedAt 
        { 
            get => _performerSubmittedAt;
            set => _performerSubmittedAt = value?.Kind == DateTimeKind.Utc ? value : value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : null;
        }
        
        private DateTime? _supervisorCompletedAt;
        public DateTime? SupervisorCompletedAt 
        { 
            get => _supervisorCompletedAt;
            set => _supervisorCompletedAt = value?.Kind == DateTimeKind.Utc ? value : value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : null;
        }
        
        private DateTime _createdAt = DateTime.UtcNow;
        public DateTime CreatedAt 
        { 
            get => _createdAt;
            set => _createdAt = value.Kind == DateTimeKind.Utc ? value : DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
        
        private DateTime _updatedAt = DateTime.UtcNow;
        public DateTime UpdatedAt 
        { 
            get => _updatedAt;
            set => _updatedAt = value.Kind == DateTimeKind.Utc ? value : DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
        
        // Additional properties that were accidentally removed
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
        
        // SUPERVISOR SECTION - Simplified to match new requirements
        [Display(Name = "Overall was this encounter acceptable or not acceptable?")]
        public bool? OverallAcceptable { get; set; } // true = Acceptable, false = Not Acceptable
        
        [Display(Name = "Overall comment on the encounter and provide an agreed action plan (if needed). It is useful to include any resources the performer could reference.")]
        [DataType(DataType.MultilineText)]
        public string? SupervisorActionPlan { get; set; }
        
        // Supervisor submission tracking
        public bool IsSupervisorCompleted { get; set; } = false;
        public string? CompletedBySupervisor { get; set; }
        
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
