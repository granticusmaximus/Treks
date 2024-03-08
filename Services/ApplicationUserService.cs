using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Treks.Data;
using Treks.Models;

namespace Treks.Services
{
    public class ApplicationUserService
    {
        #region Property
        private readonly ApplicationDbContext _appDBContext;
        #endregion

        #region Constructor
        public ApplicationUserService(ApplicationDbContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        #endregion

        #region Insert User
        public async Task<bool> InsertUserAsync(ApplicationUser appUser, int selectedRoleId)
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

        #region Update App User
        public async Task<bool> UpdateUserAsync(ApplicationUser appUser, int selectedRoleId)
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
    }
}