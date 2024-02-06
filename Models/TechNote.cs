using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Treks.Models
{
    public class TechNote
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Notes { get; set; }
        public string? Username { get; set; }

        public ICollection<TicketTechNote>? TicketTechNotes { get; set; }
    }
}