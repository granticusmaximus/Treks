using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Treks.Data;
using Treks.Models;

namespace Treks.Services
{
    public class RoleService
    {
        private readonly ApplicationDbContext _context;

        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create a new role
        public async Task<bool> CreateRoleAsync(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            _context.Roles.Add(role);
            return await _context.SaveChangesAsync() > 0;
        }

        // Retrieve a role by ID
        public async Task<Role> GetRoleByIdAsync(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        // Retrieve all roles
        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        // Update an existing role
        public async Task<bool> UpdateRoleAsync(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            _context.Roles.Update(role);
            return await _context.SaveChangesAsync() > 0;
        }

        // Delete a role by ID
        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
                return false;

            _context.Roles.Remove(role);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}