using Microsoft.AspNetCore.Mvc;
using Treks.Services;

namespace Treks.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadsController : ControllerBase
    {
        private readonly IFileUploadService _uploadService;
        private readonly ILogger<UploadsController> _logger;

        public UploadsController(
            ILogger<UploadsController> logger,
            IFileUploadService uploadService)
        {
            _logger = logger;
            _uploadService = uploadService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                var url = await _uploadService.UploadImageAsync(file);
                return Ok(new { location = url }); // TinyMCE expects { location: "url" }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "File upload failed.");
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}