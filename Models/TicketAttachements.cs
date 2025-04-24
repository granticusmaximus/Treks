using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Treks.Models
{
    public class TicketAttachment
    {
        [Key]
        public int Id { get; set; }

        public string FileName { get; set; }
        public string FileUrl { get; set; }

        [ForeignKey("TicketId")]
        public string TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}