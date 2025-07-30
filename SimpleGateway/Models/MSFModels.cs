using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleGateway.Models
{
    // Main MSF questionnaire for a performer
    public class MSFQuestionnaire
    {
        public int Id { get; set; }
        public int PerformerId { get; set; } // Foreign key to UserModel
        public string Title { get; set; } = string.Empty;
        public string UniqueCode { get; set; } = string.Empty; // For the shareable link
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public virtual UserModel Performer { get; set; } = null!;
        public virtual ICollection<MSFResponse> Responses { get; set; } = new List<MSFResponse>();
    }
    
    // A response from a colleague
    public class MSFResponse
    {
        public int Id { get; set; }
        public int MSFQuestionnaireId { get; set; }
        public string? RespondentName { get; set; }
        public string? RespondentRole { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        
        // Patient Care & Communication (1-6)
        public int? PatientCareQualityScore { get; set; }
        public string? PatientCareQualityComment { get; set; }
        public int? CommunicationSkillsScore { get; set; }
        public string? CommunicationSkillsComment { get; set; }
        public int? CommunicationEmpathyScore { get; set; }
        public string? CommunicationEmpathyComment { get; set; }
        public int? HistoryTakingScore { get; set; }
        public string? HistoryTakingComment { get; set; }
        public int? ConsultationManagementScore { get; set; }
        public string? ConsultationManagementComment { get; set; }
        public int? CulturalSensitivityScore { get; set; }
        public string? CulturalSensitivityComment { get; set; }

        // Professional Integrity & Development (7-11)
        public int? EthicalProfessionalismScore { get; set; }
        public string? EthicalProfessionalismComment { get; set; }
        public int? ProfessionalDevelopmentScore { get; set; }
        public string? ProfessionalDevelopmentComment { get; set; }
        public int? TechnicalCompetenceScore { get; set; }
        public string? TechnicalCompetenceComment { get; set; }
        public int? DecisionMakingScore { get; set; }
        public string? DecisionMakingComment { get; set; }
        public int? DocumentationScore { get; set; }
        public string? DocumentationComment { get; set; }

        // Team Working & Quality Improvement (12-17)
        public int? TeamCollaborationScore { get; set; }
        public string? TeamCollaborationComment { get; set; }
        public int? TeamSupportScore { get; set; }
        public string? TeamSupportComment { get; set; }
        public int? LeadershipSkillsScore { get; set; }
        public string? LeadershipSkillsComment { get; set; }
        public int? QualityImprovementScore { get; set; }
        public string? QualityImprovementComment { get; set; }
        public int? HealthSafetyAwarenessScore { get; set; }
        public string? HealthSafetyAwarenessComment { get; set; }
        public int? ContinuousImprovementScore { get; set; }
        public string? ContinuousImprovementComment { get; set; }

        public string? AdditionalComments { get; set; }
        
        public virtual MSFQuestionnaire Questionnaire { get; set; } = null!;
    }
    
    // DTO for creating MSF
    public class CreateMSFDto
    {
        public string PerformerUsername { get; set; } = string.Empty;
    }
    
    // DTO for submitting MSF response
    public class SubmitMSFResponseDto
    {
        public string QuestionnaireCode { get; set; } = string.Empty;
        public string? RespondentName { get; set; }
        public string? RespondentRole { get; set; }
        
        // Patient Care & Communication (1-6)
        public int? PatientCareQualityScore { get; set; }
        public string? PatientCareQualityComment { get; set; }
        public int? CommunicationSkillsScore { get; set; }
        public string? CommunicationSkillsComment { get; set; }
        public int? CommunicationEmpathyScore { get; set; }
        public string? CommunicationEmpathyComment { get; set; }
        public int? HistoryTakingScore { get; set; }
        public string? HistoryTakingComment { get; set; }
        public int? ConsultationManagementScore { get; set; }
        public string? ConsultationManagementComment { get; set; }
        public int? CulturalSensitivityScore { get; set; }
        public string? CulturalSensitivityComment { get; set; }

        // Professional Integrity & Development (7-11)
        public int? EthicalProfessionalismScore { get; set; }
        public string? EthicalProfessionalismComment { get; set; }
        public int? ProfessionalDevelopmentScore { get; set; }
        public string? ProfessionalDevelopmentComment { get; set; }
        public int? TechnicalCompetenceScore { get; set; }
        public string? TechnicalCompetenceComment { get; set; }
        public int? DecisionMakingScore { get; set; }
        public string? DecisionMakingComment { get; set; }
        public int? DocumentationScore { get; set; }
        public string? DocumentationComment { get; set; }

        // Team Working & Quality Improvement (12-17)
        public int? TeamCollaborationScore { get; set; }
        public string? TeamCollaborationComment { get; set; }
        public int? TeamSupportScore { get; set; }
        public string? TeamSupportComment { get; set; }
        public int? LeadershipSkillsScore { get; set; }
        public string? LeadershipSkillsComment { get; set; }
        public int? QualityImprovementScore { get; set; }
        public string? QualityImprovementComment { get; set; }
        public int? HealthSafetyAwarenessScore { get; set; }
        public string? HealthSafetyAwarenessComment { get; set; }
        public int? ContinuousImprovementScore { get; set; }
        public string? ContinuousImprovementComment { get; set; }

        public string? AdditionalComments { get; set; }
    }
    
    // DTO for MSF results
    public class MSFResultsDto
    {
        public string PerformerUsername { get; set; } = string.Empty;
        public string PerformerName { get; set; } = string.Empty;
        public int TotalResponses { get; set; }
        public DateTime LastUpdated { get; set; }
        
        // New properties for controller compatibility
        public List<ResponseSummary> ResponseSummary { get; set; } = new List<ResponseSummary>();
        public Dictionary<string, double> QuestionAverages { get; set; } = new Dictionary<string, double>();
        public List<string> AllComments { get; set; } = new List<string>();
        
        // Patient Care and Communication averages
        public double TreatPatientsWithCompassionAvg { get; set; }
        public double EnableInformedDecisionsAvg { get; set; }
        public double RecogniseCommNeedsPatientsAvg { get; set; }
        public double ProduceClearCommunicationsAvg { get; set; }
        
        // Professional Integrity and Development averages
        public double DemonstrateIntegrityAvg { get; set; }
        public double WorkWithinScopeAvg { get; set; }
        public double EngageWithDevelopmentAvg { get; set; }
        public double KeepPracticeUpToDateAvg { get; set; }
        public double FacilitateLearningAvg { get; set; }
        public double InteractWithColleaguesAvg { get; set; }
        public double PromoteEqualityDiversityAvg { get; set; }
        
        // Team Working and Quality Improvement averages
        public double RecogniseImpactOnOthersAvg { get; set; }
        public double ManageTimeResourcesAvg { get; set; }
        public double WorkAsTeamMemberAvg { get; set; }
        public double WorkToStandardsAvg { get; set; }
        public double ParticipateInImprovementAvg { get; set; }
        public double MinimiseWasteEnvironmentAvg { get; set; }
        
        // Category averages
        public double PatientCareCommAvg { get; set; }
        public double ProfessionalIntegrityAvg { get; set; }
        public double TeamWorkingQualityAvg { get; set; }
        public double OverallAverage { get; set; }
        
        // Rating distribution for charts (counts of 1s, 2s, 3s, 4s)
        public Dictionary<string, Dictionary<int, int>> RatingDistribution { get; set; } = new Dictionary<string, Dictionary<int, int>>();
        
        // Feedback
        public List<string> DoesParticularlyWellComments { get; set; } = new List<string>();
        public List<string> CouldImproveUponComments { get; set; } = new List<string>();
        
        // Relationship breakdown
        public Dictionary<string, int> RelationshipBreakdown { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> DurationBreakdown { get; set; } = new Dictionary<string, int>();
    }

    public class ResponseSummary
    {
        public string RespondentRole { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
    }
}
