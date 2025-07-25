using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleGateway.Models
{
    public class AssignmentModel
    {
        public int Id { get; set; }

        [Required]
        public int PerformerId { get; set; }

        public int? SupervisorId { get; set; }

        public int? AdvisorId { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        [ForeignKey("PerformerId")]
        public virtual UserModel? Performer { get; set; }

        [ForeignKey("SupervisorId")]
        public virtual UserModel? Supervisor { get; set; }

        [ForeignKey("AdvisorId")]
        public virtual UserModel? Advisor { get; set; }
    }
}
