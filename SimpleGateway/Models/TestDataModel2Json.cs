using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace SimpleGateway.Models
{
    public class TestDataModel2Json
    {
        public int Id { get; set; }

        // Keep original required fields
        [Required(ErrorMessage = "Please specify how long you have worked in the UK")]
        [Display(Name = "How long have you worked in the UK (if any)?")]
        public string UKWorkExperience { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please specify when you last treated a patient")]
        [Display(Name = "When was the last time you treated a patient?")]
        public string LastPatientTreatment { get; set; } = string.Empty;

        // Store all complex assessment data as JSON
        [Display(Name = "Assessment Data")]
        public string? AssessmentDataJson { get; set; }

        // Helper property to work with structured data
        public Dictionary<string, object>? AssessmentData
        {
            get
            {
                if (string.IsNullOrEmpty(AssessmentDataJson))
                    return new Dictionary<string, object>();
                
                try
                {
                    return JsonSerializer.Deserialize<Dictionary<string, object>>(AssessmentDataJson);
                }
                catch
                {
                    return new Dictionary<string, object>();
                }
            }
            set
            {
                AssessmentDataJson = value != null ? JsonSerializer.Serialize(value) : null;
            }
        }

        // Audit fields
        public string Username { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
    }
}
