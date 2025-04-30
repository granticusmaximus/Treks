using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Treks.Models
{
    public class TicketChangeLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TicketId { get; set; } = null!;

        [Required]
        public string ChangedByUserId { get; set; } = null!;

        [Required]
        public string ChangeDescription { get; set; } = null!;

        public DateTime ChangeDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("TicketId")]
        public virtual Ticket Ticket { get; set; } = null!;

        [ForeignKey("ChangedByUserId")]
        public virtual ApplicationUser ChangedByUser { get; set; } = null!;
    }
}
