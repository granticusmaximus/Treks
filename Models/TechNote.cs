using System.ComponentModel.DataAnnotations;

namespace Treks.Models
{
    public class TechNote
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Notes { get; set; }
        public string? Username { get; set; }
        public string? FullName { get; set; }
        public byte[]? ProfilePicture { get; set; }

        public ICollection<TicketTechNote>? TicketTechNotes { get; set; }
    }
}