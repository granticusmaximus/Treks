using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Treks.Data;
using Treks.Models;

namespace Treks.Services
{
    public class CompanyService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CompanyService> _logger;

        public CompanyService(ApplicationDbContext context, ILogger<CompanyService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<List<Company>> GetAllCompaniesAsync()
        {
            try
            {
                // Includes comments for eager loading, adjust if comments should be lazy-loaded
                return await _context.Companies.Include(c => c.Comments).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving companies.");
                throw new ApplicationException("Error occurred while retrieving companies.", ex);
            }
        }

        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            try
            {
                // Include comments when fetching a specific company
                var company = await _context.Companies
                    .Include(c => c.Comments)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (company == null)
                {
                    _logger.LogWarning("Company {CompanyId} not found.", id);
                    throw new ApplicationException($"Company with ID {id} not found.");
                }

                return company;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving company {CompanyId}.", id);
                throw new ApplicationException($"Error occurred while retrieving company with ID {id}.", ex);
            }
        }

        public async Task<bool> CreateCompanyAsync(Company company)
        {
            if (company == null)
                throw new ArgumentNullException(nameof(company));

            try
            {
                _context.Companies.Add(company);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating company.");
                throw new ApplicationException("Error occurred while creating company.", ex);
            }
        }

        public async Task<bool> UpdateCompanyAsync(Company company)
        {
            if (company == null)
                throw new ArgumentNullException(nameof(company));

            try
            {
                _context.Companies.Update(company);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating company {CompanyId}.", company.Id);
                throw new ApplicationException($"Error occurred while updating company with ID {company.Id}.", ex);
            }
        }

        public async Task AddCommentAsync(Comment comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            try
            {
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding company comment.");
                throw new ApplicationException("Error occurred while adding a comment.", ex);
            }
        }

        public async Task<List<Comment>> GetCommentsByCompanyIdAsync(int companyId)
        {
            try
            {
                // Adjust this method if you need to handle hierarchical comments differently
                var comments = await _context.Comments
                    .Where(c => c.CompanyId == companyId)
                    .ToListAsync();

                return comments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving comments for company {CompanyId}.", companyId);
                throw new ApplicationException($"Error occurred while retrieving comments for company ID {companyId}.", ex);
            }
        }

        public async Task<List<Ticket>> GetActiveTicketsbyCompanyIdAsync(int companyId)
        {
            try
            {
                var tickets = await _context.Tickets
                    .Where(t => t.AssignedCompanyId == companyId && t.isComplete != true)
                    .ToListAsync();

                return tickets;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active tickets for company {CompanyId}.", companyId);
                throw new ApplicationException($"Error occurred while retrieving active tickets for company ID {companyId}.", ex);
            }
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            try
            {
                var company = await _context.Companies.FindAsync(id);
                if (company == null)
                    return false;

                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting company {CompanyId}.", id);
                throw new ApplicationException($"Error occurred while deleting company with ID {id}.", ex);
            }
        }

        public async Task<bool> AssignUsersToCompanyAsync(int companyId, List<string> userIds)
        {
            try
            {
                var company = await _context.Companies.Include(c => c.AssignedUsers)
                                                      .FirstOrDefaultAsync(c => c.Id == companyId);

                if (company == null)
                {
                    _logger.LogWarning("Company {CompanyId} not found for user assignment.", companyId);
                    throw new ApplicationException($"Company with ID {companyId} not found.");
                }

                var users = await _context.ApplicationUser.Where(u => userIds.Contains(u.Id)).ToListAsync();

                if (users == null || !users.Any())
                {
                    _logger.LogWarning("No valid users found for company assignment {CompanyId}.", companyId);
                    throw new ApplicationException("No valid users found for assignment.");
                }

                foreach (var user in users)
                {
                    if (!company.AssignedUsers.Contains(user))
                    {
                        company.AssignedUsers.Add(user);
                    }

                    if (!user.AssignedCompany.Contains(company))
                    {
                        user.AssignedCompany.Add(company);
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning users to company {CompanyId}.", companyId);
                throw new ApplicationException("Error occurred while assigning users to company.", ex);
            }
        }
    }
}
