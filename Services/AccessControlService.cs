using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Treks.Data;

namespace Treks.Services
{
    public class AccessContext
    {
        public string RoleName { get; set; } = "";
        public IReadOnlyList<int> CompanyIds { get; set; } = Array.Empty<int>();

        public bool IsAdmin => RoleName == "Admin";
        public bool IsManager => RoleName == "Manager";
        public bool IsDeveloper => RoleName == "Developer";
        public bool IsUser => RoleName == "User";

        public bool CanAccessCompany(int companyId)
        {
            return IsAdmin || CompanyIds.Contains(companyId);
        }
    }

    public class AccessControlService
    {
        private readonly ApplicationDbContext _context;

        public AccessControlService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AccessContext> GetAccessContextAsync(ClaimsPrincipal principal)
        {
            if (principal?.Identity?.IsAuthenticated != true)
                return new AccessContext();

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return new AccessContext();

            var user = await _context.ApplicationUser
                .Include(u => u.Role)
                .Include(u => u.AssignedCompany)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return new AccessContext();

            var context = new AccessContext
            {
                RoleName = user.Role?.Name ?? "",
                CompanyIds = user.AssignedCompany.Select(c => c.Id).ToList()
            };

            return context;
        }
    }
}
