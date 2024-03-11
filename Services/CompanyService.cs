using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Treks.Data;
using Treks.Models;

namespace Treks.Services
{
    public class CompanyService
    {
        private readonly ApplicationDbContext _context;

        public CompanyService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Company>> GetAllCompaniesAsync()
        {
            try
            {
                return await _context.Companies.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw new ApplicationException("Error occurred while retrieving companies.", ex);
            }
        }

        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            try
            {
                return await _context.Companies.FindAsync(id);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
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
                // Log the exception or handle it appropriately
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
                // Log the exception or handle it appropriately
                throw new ApplicationException($"Error occurred while updating company with ID {company.Id}.", ex);
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
                // Log the exception or handle it appropriately
                throw new ApplicationException($"Error occurred while deleting company with ID {id}.", ex);
            }
        }
    }
}
