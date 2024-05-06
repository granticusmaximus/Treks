using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Treks.Data;
using Treks.Models;

namespace Treks.Services
{
    public class ApplicationUserService
    {
        #region Property
        private readonly ApplicationDbContext _appDBContext;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region Constructor
        public ApplicationUserService(ApplicationDbContext appDBContext, UserManager<ApplicationUser> userManager)
        {
            _appDBContext = appDBContext;
            _userManager = userManager;
        }
        #endregion

        #region Insert User
        public async Task<bool> InsertUserAsync(ApplicationUser appUser, string selectedRoleId)
        {
            var role = await _appDBContext.Roles.FindAsync(selectedRoleId);
            if (role == null)
            {
                throw new ArgumentException("Invalid role ID provided.");
            }

            appUser.RoleId = selectedRoleId;

            await _appDBContext.ApplicationUser.AddAsync(appUser);
            await _appDBContext.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Get All App Users   
        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _appDBContext.ApplicationUser.ToListAsync();
        }
        #endregion

        #region Get App User by Id
        public async Task<ApplicationUser> GetUserAsync(string Id)
        {
            ApplicationUser? appUser = await _appDBContext.ApplicationUser.FirstOrDefaultAsync(c => c.Id.Equals(Id));
            return appUser;
        }
        #endregion

        #region Get Assigned Companies by UserID
        public async Task<List<Company>> GetAssignedCompaniesAsync(string userId)
        {
            // Fetch the user from the database with all related assigned companies
            var user = await _appDBContext.ApplicationUser
                .Include(u => u.AssignedCompany)  // Eagerly load the AssignedCompany navigation property
                .FirstOrDefaultAsync(u => u.Id == userId); // Use user ID to identify the user

            if (user == null)
            {
                throw new ArgumentException("Invalid user ID provided.");
            }

            return user.AssignedCompany.ToList(); // Convert to a list for easier manipulation and usage
        }
        #endregion

        #region Get Assigned Tickets by UserID
        public async Task<List<Ticket>> GetAssignedTicketsAsync(string userId)
        {
            // Fetch the user from the database with all related assigned tickets
            var user = await _appDBContext.ApplicationUser
                .Include(u => u.AssignedTickets)  // Eagerly load the AssignedTickets navigation property
                .FirstOrDefaultAsync(u => u.Id == userId); // Use user ID to identify the user

            if (user == null)
            {
                throw new ArgumentException("Invalid user ID provided.");
            }

            return user.AssignedTickets.ToList(); // Convert to a list for easier manipulation and usage
        }
        #endregion

        #region Update App User
        public async Task<bool> UpdateUserAsync(ApplicationUser appUser, string selectedRoleId)
        {
            var role = await _appDBContext.Roles.FindAsync(selectedRoleId);
            if (role == null)
            {
                throw new ArgumentException("Invalid role ID provided.");
            }

            appUser.RoleId = selectedRoleId;

            _appDBContext.ApplicationUser.Update(appUser);
            await _appDBContext.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Delete App User
        public async Task<bool> DeleteUserAsync(ApplicationUser appUser)
        {
            _appDBContext.Remove(appUser);
            await _appDBContext.SaveChangesAsync();
            return true;
        }
        #endregion

        public async Task<ApplicationUser> GetUserByNameAsync(string userName)
        {
            return await _appDBContext.ApplicationUser.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<bool> UpdateUserAsync(ApplicationUser appUser)
        {
            _appDBContext.ApplicationUser.Update(appUser);
            await _appDBContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Succeeded;
        }
    }
}