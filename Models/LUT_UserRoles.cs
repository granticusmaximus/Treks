using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Treks.Models
{
    public class LUT_UserRoles
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int RoleId { get; set; }

        public ApplicationUser User { get; set; }
        public Role Role { get; set; }
    }
}