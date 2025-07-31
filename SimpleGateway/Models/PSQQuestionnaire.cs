using System.ComponentModel.DataAnnotations;

namespace SimpleGateway.Models
{
    public class PSQQuestionnaire
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int PerformerId { get; set; }
        
        [Required]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string UniqueCode { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        public virtual UserModel? Performer { get; set; }
        public virtual ICollection<PSQResponse> Responses { get; set; } = new List<PSQResponse>();
    }
}
