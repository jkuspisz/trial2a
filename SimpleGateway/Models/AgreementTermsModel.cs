using System.ComponentModel.DataAnnotations;

namespace SimpleGateway.Models
{
    public class AgreementTermsModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;

        // RESTRICTIONS checkboxes (5 items)
        public bool Restriction1 { get; set; } // Practice requirements and supporting dentist
        public bool Restriction2 { get; set; } // No out of hours or locum work
        public bool Restriction3 { get; set; } // Report complaints
        public bool Restriction4 { get; set; } // 12 month completion timeframe
        public bool Restriction5 { get; set; } // Agreement terms linked to probationary flag

        // SPECIFIC ACTIONS checkboxes (17 items)
        public bool Action1 { get; set; }  // NHS courses attendance
        public bool Action2 { get; set; }  // NHS competency-based references after 6 months
        public bool Action3 { get; set; }  // GDC core verifiable CPD topics
        public bool Action4 { get; set; }  // Up-to-date PDP
        public bool Action5 { get; set; }  // Record keeping audit after 3 months
        public bool Action6 { get; set; }  // Clinical audit with reflection
        public bool Action7 { get; set; }  // Patient satisfaction survey and multi-source feedback
        public bool Action8 { get; set; }  // IQD programme reflection
        public bool Action9 { get; set; }  // 3 DOPS on ID Block
        public bool Action10 { get; set; } // DOPS/CBDs for Radiography
        public bool Action11 { get; set; } // DOPS/CBDs for Periodontics
        public bool Action12 { get; set; } // DOPS/CBDs for Prosthetics
        public bool Action13 { get; set; } // DOPS/CBDs for Direct Restorations
        public bool Action14 { get; set; } // DOPS/CBDs for Paedodontics
        public bool Action15 { get; set; } // DOPS/CBDs for Oral Surgery
        public bool Action16 { get; set; } // DOPS/CBDs for Indirect Restorations
        public bool Action17 { get; set; } // DOPS/CBDs for Endodontics

        // Custom terms (advisor can add bespoke terms)
        [Display(Name = "Custom Agreement Terms")]
        [DataType(DataType.MultilineText)]
        public string? CustomTerms { get; set; }

        // Workflow control fields
        public bool IsReleased { get; set; } = false;
        public string? ReleasedBy { get; set; }
        public DateTime? ReleasedDate { get; set; }
        public bool IsAgreedByPerformer { get; set; } = false;
        public string? AgreedBy { get; set; }
        public DateTime? AgreedDate { get; set; }

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
