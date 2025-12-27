using System.Text;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Treks.Data;
using Treks.Models;

namespace Treks.Services
{
    public class CompanyFileService
    {
        private const long MaxUploadBytes = 10 * 1024 * 1024;
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CompanyFileService> _logger;

        public CompanyFileService(IWebHostEnvironment env, ApplicationDbContext context, ILogger<CompanyFileService> logger)
        {
            _env = env;
            _context = context;
            _logger = logger;
        }

        public async Task<List<CompanyFile>> GetFilesForCompanyAsync(int companyId)
        {
            return await _context.CompanyFiles
                .Where(f => f.CompanyId == companyId)
                .OrderByDescending(f => f.UploadedAt)
                .ToListAsync();
        }

        public async Task<CompanyFile?> UploadCompanyFileAsync(int companyId, string companyName, IBrowserFile file)
        {
            if (file == null || file.Size == 0)
                return null;

            if (file.Size > MaxUploadBytes)
                throw new InvalidOperationException("File exceeds the 10 MB limit.");

            var folderName = $"{SanitizeName(companyName)}-files";
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", folderName);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var safeFileName = Path.GetFileName(file.Name);
            var uniqueFileName = $"{Guid.NewGuid()}_{safeFileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.OpenReadStream(MaxUploadBytes).CopyToAsync(fileStream);
            }

            var url = $"/uploads/{folderName}/{uniqueFileName}";

            var record = new CompanyFile
            {
                CompanyId = companyId,
                FileName = safeFileName,
                FileUrl = url,
                UploadedAt = DateTime.UtcNow
            };

            _context.CompanyFiles.Add(record);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Uploaded file {FileName} for company {CompanyId}.", safeFileName, companyId);

            return record;
        }

        public async Task<bool> DeleteCompanyFileAsync(int companyId, int fileId)
        {
            var record = await _context.CompanyFiles
                .FirstOrDefaultAsync(f => f.Id == fileId && f.CompanyId == companyId);

            if (record == null)
                return false;

            var relativePath = record.FileUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(_env.WebRootPath, relativePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            _context.CompanyFiles.Remove(record);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted file {FileId} for company {CompanyId}.", fileId, companyId);
            return true;
        }

        private static string SanitizeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "company";

            var invalid = Path.GetInvalidFileNameChars().ToHashSet();
            var builder = new StringBuilder(name.Length);
            foreach (var ch in name.Trim())
            {
                if (invalid.Contains(ch))
                {
                    builder.Append('-');
                }
                else if (char.IsWhiteSpace(ch))
                {
                    builder.Append('-');
                }
                else
                {
                    builder.Append(char.ToLowerInvariant(ch));
                }
            }

            var result = builder.ToString();
            return string.IsNullOrWhiteSpace(result) ? "company" : result;
        }
    }
}
