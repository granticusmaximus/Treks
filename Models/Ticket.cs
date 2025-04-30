using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Treks.Models
{
    public class Ticket
    {
        [Key]
        public string TicketId { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public TicketSeverity Severity { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime TimeOfCreation { get; set; }

        public bool isComplete { get; set; }

        // Relationship to ApplicationUser
        [ForeignKey("AssignedUserId")]
        [Display(Name = "Assigned User")]
        public string AssignedUserId { get; set; }

        public virtual ApplicationUser AssignedUser { get; set; } // Navigation property

        [ForeignKey("CompanyId")]
        public int AssignedCompanyId { get; set; }
        public virtual Company AssignedCompany { get; set; }
        public virtual ICollection<TicketTechNote> TicketTechNotes { get; set; }
        public virtual ICollection<SubTask> SubTasks { get; set; }
        public ICollection<TicketAttachment>? Attachments { get; set; }
        public virtual ICollection<TicketChangeLog> ChangeLogs { get; set; } = new List<TicketChangeLog>();

    }

    public enum TicketSeverity
    {
        Low,
        Medium,
        High
    }
}