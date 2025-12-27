using Microsoft.AspNetCore.Components.Forms;

namespace Treks.Services
{
    public class UploadService
    {
        private const long MaxUploadBytes = 10 * 1024 * 1024;
        private readonly IWebHostEnvironment _env;

        public UploadService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadFileAsync(IBrowserFile file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            if (file.Size > MaxUploadBytes)
                throw new InvalidOperationException("File exceeds the 10 MB limit.");

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var safeFileName = Path.GetFileName(file.Name);
            var uniqueFileName = $"{Guid.NewGuid()}_{safeFileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.OpenReadStream(MaxUploadBytes).CopyToAsync(fileStream);
            }

            return $"/uploads/{uniqueFileName}";
        }
    }
}
