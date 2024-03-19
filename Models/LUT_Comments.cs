namespace Treks.Models
{
    public class LUT_Comments
    {
        public string CompanyId { get; set; }
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
        public Company Company { get; set; }
    }
}