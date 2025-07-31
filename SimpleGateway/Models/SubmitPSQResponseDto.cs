using System.ComponentModel.DataAnnotations;

namespace SimpleGateway.Models
{
    public class SubmitPSQResponseDto
    {
        [Required]
        public string QuestionnaireCode { get; set; } = string.Empty;
        
        // 12 Patient Satisfaction Questions (scores: -1, -2, 3, 4, or null for "Not observed")
        public int? PutMeAtEaseScore { get; set; }
        public int? TreatedWithDignityScore { get; set; }
        public int? ListenedToConcernsScore { get; set; }
        public int? ExplainedTreatmentOptionsScore { get; set; }
        public int? InvolvedInDecisionsScore { get; set; }
        public int? InvolvedFamilyScore { get; set; }
        public int? TailoredApproachScore { get; set; }
        public int? ExplainedNextStepsScore { get; set; }
        public int? ProvidedGuidanceScore { get; set; }
        public int? AllocatedTimeScore { get; set; }
        public int? WorkedWithTeamScore { get; set; }
        public int? CanTrustDentistScore { get; set; }
        
        // 2 Open-ended text feedback questions
        public string? DoesWellComment { get; set; }
        public string? CouldImproveComment { get; set; }
    }
}
