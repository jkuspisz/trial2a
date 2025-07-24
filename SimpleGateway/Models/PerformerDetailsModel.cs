using System.ComponentModel.DataAnnotations;

namespace SimpleGateway.Models
{
    public class PerformerDetailsModel
    {
        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "GDC Number is required")]
        [Display(Name = "GDC Number")]
        public string GDCNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact Number is required")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Supporting/Supervising Dentist is required")]
        [Display(Name = "Supporting/Supervising Dentist")]
        public string SupportingDentist { get; set; } = string.Empty;

        [Required(ErrorMessage = "Supporting Dentist Contact Number is required")]
        [Display(Name = "Supporting Dentist Contact Number")]
        public string SupportingDentistContactNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Practice Address is required")]
        [Display(Name = "Practice Address")]
        public string PracticeAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Practice Post Code is required")]
        [Display(Name = "Practice Post Code")]
        public string PracticePostCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of UK Registration is required")]
        [Display(Name = "Date of UK Registration")]
        [DataType(DataType.Date)]
        public DateTime DateOfUKRegistration { get; set; }

        [Required(ErrorMessage = "Date of Dental Qualification is required")]
        [Display(Name = "Date of Dental Qualification")]
        [DataType(DataType.Date)]
        public DateTime DateOfDentalQualification { get; set; }

        [Required(ErrorMessage = "University/Country of Dental Qualification is required")]
        [Display(Name = "University/Country of Dental Qualification")]
        public string UniversityCountryOfQualification { get; set; } = string.Empty;

        // For tracking completion status
        public bool IsCompleted { get; set; } = false;
        public string Username { get; set; } = string.Empty;
    }
}
