using Microsoft.AspNetCore.Identity;

namespace Treks.Models
{
    public class Role : IdentityRole
    {
        public string Description { get; set; } 
    }
}