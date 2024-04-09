using System.ComponentModel.DataAnnotations;

namespace Treks.Models
{
    public class LUT_Comments
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
        public Company Company { get; set; }
    }
}