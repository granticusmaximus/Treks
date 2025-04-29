using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Treks.Models
{
    public class TicketAttachment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; } = string.Empty;
        public string StoredFileName { get; set; } = string.Empty;

        [Required]
        public string FilePath { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; } = DateTime.Now;
        public string FileUrl { get; set; }

        [Required]
        public string TicketId { get; set; } = string.Empty;

        [ForeignKey("TicketId")]
        public Ticket Ticket { get; set; } = null!;
    }
}