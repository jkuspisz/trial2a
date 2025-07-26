using System.ComponentModel.DataAnnotations;

namespace SimpleGateway.Models
{
    public class SimplePreviousExperienceModel
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Username { get; set; } = string.Empty;
        
        // Previous Employment
        public string? PreviousEmployer { get; set; }
        public string? JobTitle { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? JobDescription { get; set; }
        
        // Education & Qualifications
        public string? QualificationsSummary { get; set; }
        
        // Clinical Experience
        public string? ClinicalExperience { get; set; }
        
        // Skills & Competencies
        public string? SkillsAndCompetencies { get; set; }
        
        // Additional Information
        public string? AdditionalNotes { get; set; }
        
        // Metadata
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
