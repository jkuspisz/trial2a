using System.ComponentModel.DataAnnotations;

namespace SimpleGateway.Models
{
    public class PSQResponse
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int PSQQuestionnaireId { get; set; }
        
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        
        // 12 Patient Satisfaction Questions (PSQ.txt scores: -1, -2, 3, 4, or null for "Not observed")
        public int? PutMeAtEaseScore { get; set; }                    // The Dentist put me at ease
        public int? TreatedWithDignityScore { get; set; }            // The Dentist treated me with dignity and respect
        public int? ListenedToConcernsScore { get; set; }            // The Dentist listened and responded to my concerns
        public int? ExplainedTreatmentOptionsScore { get; set; }     // The Dentist clearly explained available treatment options including costs
        public int? InvolvedInDecisionsScore { get; set; }           // The Dentist involved me as much as I wanted to be in decision about my care
        public int? InvolvedFamilyScore { get; set; }                // The Dentist involved my family/carers appropriately
        public int? TailoredApproachScore { get; set; }              // The Dentist tailored their approach to meet my needs
        public int? ExplainedNextStepsScore { get; set; }            // The Dentist explained what will happen next with my treatment
        public int? ProvidedGuidanceScore { get; set; }              // The Dentist provided guidance on how to take care of my teeth and gums
        public int? AllocatedTimeScore { get; set; }                 // The Dentist allocated the right amount of time for my treatment
        public int? WorkedWithTeamScore { get; set; }                // The Dentist worked well with other team members
        public int? CanTrustDentistScore { get; set; }               // I feel I can trust this dentist with my dental care
        
        // 2 Open-ended text feedback questions
        public string? DoesWellComment { get; set; }                 // Anything you feel this dentist does particularly well?
        public string? CouldImproveComment { get; set; }             // Anything you feel this dentist could improve upon?
        
        // Navigation property
        public virtual PSQQuestionnaire? Questionnaire { get; set; }
    }
}
