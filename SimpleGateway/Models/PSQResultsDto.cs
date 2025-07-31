namespace SimpleGateway.Models
{
    public class PSQResultsDto
    {
        public string PerformerUsername { get; set; } = string.Empty;
        public string PerformerName { get; set; } = string.Empty;
        public int TotalResponses { get; set; }
        public Dictionary<string, double> QuestionAverages { get; set; } = new Dictionary<string, double>();
        public List<string> DoesWellComments { get; set; } = new List<string>();
        public List<string> CouldImproveComments { get; set; } = new List<string>();
        public string? FeedbackUrl { get; set; }
        public string? UniqueCode { get; set; }
        public bool HasActiveQuestionnaire { get; set; }
    }
}
