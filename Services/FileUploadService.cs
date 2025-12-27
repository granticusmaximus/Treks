using Microsoft.AspNetCore.Http;

namespace Treks.Services
{
    public interface IFileUploadService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }

    public class FileUploadService : IFileUploadService
    {
        private const long MaxFileSize = 5 * 1024 * 1024;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<FileUploadService> _logger;

        public FileUploadService(IWebHostEnvironment env, ILogger<FileUploadService> logger)
        {
            _env = env;
            _logger = logger;
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file.");

            if (file.Length > MaxFileSize)
                throw new InvalidOperationException("File exceeds the 5 MB limit.");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };

            if (!allowed.Contains(ext))
                throw new InvalidOperationException("Unsupported file type.");

            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/{fileName}";
        }
    }
}
