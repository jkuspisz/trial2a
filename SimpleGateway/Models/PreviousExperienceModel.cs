using System.ComponentModel.DataAnnotations;

namespace SimpleGateway.Models
{
    public class EmploymentHistoryJob
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; } = "";
        
        [Display(Name = "Full Address")]
        public string Address { get; set; } = "";
        
        [Display(Name = "Treated Adult Patients")]
        public bool TreatedAdults { get; set; }
        
        [Display(Name = "Treated Child Patients")]
        public bool TreatedChildren { get; set; }
    }

    public class Qualification
    {
        [Required]
        [Display(Name = "Qualification")]
        public string QualificationName { get; set; } = "";
        
        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; } = "";
        
        [Required]
        [Display(Name = "Institution")]
        public string Institution { get; set; } = "";
        
        [Required]
        [Display(Name = "Year")]
        public string Year { get; set; } = "";
    }

    public class PreviousExperienceModel
    {
        public string Username { get; set; } = "";
        
        [Display(Name = "GDC Gaps Explanation")]
        public string GdcGapsExplanation { get; set; } = "";
        
        public List<Qualification> Qualifications { get; set; } = new();
        
        [Display(Name = "NHS Experience")]
        public string NhsExperience { get; set; } = "";
        
        [Display(Name = "Full Time")]
        public string FullTime { get; set; } = "";
        
        [Display(Name = "Part Time Days Per Week")]
        public string PartTimeDaysPerWeek { get; set; } = "";
        
        [Display(Name = "Years")]
        public string Years { get; set; } = "";
        
        [Display(Name = "Months")]
        public string Months { get; set; } = "";
        
        public List<EmploymentHistoryJob> EmploymentHistory { get; set; } = new();
        
        public bool ApplicantConfirmed { get; set; }
        public DateTime? ApplicantConfirmedAt { get; set; }
        
        [Display(Name = "Form Submitted")]
        public bool IsSubmitted { get; set; }
        
        public DateTime? AdvisorDeclarationAt { get; set; }
        public string AdvisorDeclarationBy { get; set; } = "";
        
        [Display(Name = "Advisor Declaration Comment")]
        public string AdvisorDeclarationComment { get; set; } = "";
    }
}
