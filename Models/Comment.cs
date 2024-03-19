using System.ComponentModel.DataAnnotations;

namespace Treks.Models
{
    public class Comment 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Notes { get; set; }
        public string? Username { get; set; }

       // Foreign key for the Company relationship
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public Comment()
        {
            Notes = string.Empty;  // Set a default value for 'Notes' in the constructor
        }
    }
}