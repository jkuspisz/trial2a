using System.ComponentModel.DataAnnotations;

namespace SimpleGateway.Models
{
    public class PrevExpModel
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please specify your country of qualification")]
        [Display(Name = "Country of Qualification")]
        public string QualificationCountry { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please specify your university or institution")]
        [Display(Name = "University/Institution")]
        public string QualificationUniversity { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please provide your employment history")]
        [Display(Name = "Employment History")]
        public string EmploymentHistory { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
