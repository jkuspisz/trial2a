// Future Questionnaire Models - Ready for implementation

namespace SimpleGateway.Models
{
    // For sending questionnaires to non-users
    public class ExternalContact
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string OrganizationRole { get; set; } = ""; // Patient, Colleague, etc.
        public string PerformerUsername { get; set; } = ""; // Which performer this contact relates to
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }

    // Questionnaire templates
    public class Questionnaire
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string CreatedByUsername { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public List<QuestionnaireQuestion> Questions { get; set; } = new();
        public List<QuestionnaireResponse> Responses { get; set; } = new();
    }

    // Individual questions
    public class QuestionnaireQuestion
    {
        public int Id { get; set; }
        public int QuestionnaireId { get; set; }
        public string QuestionText { get; set; } = "";
        public string QuestionType { get; set; } = ""; // Text, MultipleChoice, Rating, etc.
        public string? Options { get; set; } // JSON for multiple choice options
        public int OrderIndex { get; set; }
        public bool IsRequired { get; set; }
        
        // Navigation properties
        public Questionnaire Questionnaire { get; set; } = null!;
    }

    // Responses from external contacts
    public class QuestionnaireResponse
    {
        public int Id { get; set; }
        public int QuestionnaireId { get; set; }
        public string PerformerUsername { get; set; } = ""; // Which performer this is about
        public string RespondentEmail { get; set; } = "";
        public string RespondentName { get; set; } = "";
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public bool IsCompleted { get; set; }
        public string UniqueToken { get; set; } = ""; // For tracking anonymous responses
        
        // Navigation properties
        public Questionnaire Questionnaire { get; set; } = null!;
        public List<QuestionnaireAnswer> Answers { get; set; } = new();
    }

    // Individual answers
    public class QuestionnaireAnswer
    {
        public int Id { get; set; }
        public int ResponseId { get; set; }
        public int QuestionId { get; set; }
        public string AnswerText { get; set; } = "";
        public int? NumericValue { get; set; } // For ratings
        public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public QuestionnaireResponse Response { get; set; } = null!;
        public QuestionnaireQuestion Question { get; set; } = null!;
    }
}
