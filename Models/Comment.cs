using System.ComponentModel.DataAnnotations;

namespace Treks.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TheComment { get; set; }

        public string? Username { get; set; }
        
        public byte[]? ProfilePicture { get; set; }

        public string? FullName { get; set; }

        // Foreign key for the Company relationship
        public int CompanyId { get; set; }
        
        // Self-referencing relationship to allow hierarchical comments
        public int? ParentCommentId { get; set; } // Nullable for top-level comments
        public Comment? ParentComment { get; set; }
        public ICollection<Comment> ChildComments { get; set; } = new List<Comment>();

        public ICollection<LUT_Comments>? Comments { get; set; }
    }

}