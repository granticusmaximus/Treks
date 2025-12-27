using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Treks.Models
{
    public class CompanyFile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; } = null!;

        [Required]
        [StringLength(260)]
        public string FileName { get; set; } = "";

        [Required]
        [StringLength(512)]
        public string FileUrl { get; set; } = "";

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
