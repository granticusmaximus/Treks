using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Treks.Models
{
    public class SubTask
    {
        [Key]
        public int SubTaskId { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        public bool IsComplete { get; set; }

        // Foreign key to Ticket
        [Required]
        [ForeignKey("Ticket")]
        public string TicketId { get; set; }
        
        public virtual Ticket Ticket { get; set; }
    }
}