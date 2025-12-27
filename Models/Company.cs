using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Treks.Models
{
    public class Company 
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Company Name")]
        [Required]
        public string Name { get; set; }

        [Phone]
        public string? ContactNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }

        public virtual ICollection<ApplicationUser> AssignedUsers { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }    

        public virtual ICollection<LUT_Comments> Comments { get; set; }
        public virtual ICollection<CompanyFile> Files { get; set; } = new List<CompanyFile>();
    }
}
