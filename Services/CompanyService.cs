using Microsoft.EntityFrameworkCore;
using Treks.Data;
using Treks.Models;

namespace Treks.Services
{
    public class CompanyService {
        private readonly ApplicationDbContext _context;

        public CompanyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateCompanyAsync(Company company)
        {
            if (company == null)
                throw new ArgumentNullException(nameof(company));

            _context.Companies.Add(company);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            return await _context.Companies.FindAsync(id);
        }

        public async Task<List<Company>> GetAllCompaniesAsync()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<bool> UpdateCompanyAsync(Company company)
        {
            if (company == null)
                throw new ArgumentNullException(nameof(company));

            _context.Companies.Update(company);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
                return false;

            _context.Companies.Remove(company);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}