using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Treks.Data;

namespace Treks.Services
{
    public class RoleClaimsTransformation : IClaimsTransformation
    {
        private readonly ApplicationDbContext _context;

        public RoleClaimsTransformation(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal?.Identity?.IsAuthenticated != true)
                return principal;

            var identity = principal.Identity as ClaimsIdentity;
            if (identity == null)
                return principal;

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return principal;

            if (principal.Claims.Any(c => c.Type == ClaimTypes.Role))
                return principal;

            var user = await _context.ApplicationUser
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var roleName = user?.Role?.Name;
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, roleName));
            }

            return principal;
        }
    }
}
