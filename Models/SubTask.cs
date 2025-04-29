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

        public bool IsComplete { get; set; }

        [Required]
        public string TicketId { get; set; }

        [ForeignKey("TicketId")]
        public Ticket Ticket { get; set; }

        // 🆕 Added Properties
        [Range(1, 5, ErrorMessage = "Priority must be between 1 (High) and 5 (Low).")]
        public int Priority { get; set; } = 3; // Default to Medium priority

        public DateTime? DueDate { get; set; } // Optional
    }
} 