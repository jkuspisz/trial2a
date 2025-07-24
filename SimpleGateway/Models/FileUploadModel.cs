using System.ComponentModel.DataAnnotations;

namespace SimpleGateway.Models
{
    public class FileUploadEntry
    {
        public int Id { get; set; }
        public string FileId { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        [Display(Name = "File Description")]
        public string Description { get; set; } = "";
        
        [Required]
        [Display(Name = "File Name")]
        public string FileName { get; set; } = "";
        
        [Display(Name = "File Size (bytes)")]
        public long FileSize { get; set; }
        
        [Display(Name = "File Type")]
        public string ContentType { get; set; } = "";
        
        [Display(Name = "Upload Date")]
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        
        [Display(Name = "File Path")]
        public string FilePath { get; set; } = "";
        
        [Display(Name = "Is Required")]
        public bool IsRequired { get; set; }
        
        [Display(Name = "Category")]
        public string Category { get; set; } = "";
    }

    public class FileUploadModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public List<FileUploadEntry> UploadedFiles { get; set; } = new();
        
        [Display(Name = "Indemnity/Insurance Evidence")]
        public bool HasIndemnityEvidence { get; set; }
        
        public DateTime? LastUpdated { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
