using Microsoft.AspNetCore.Mvc;
using Treks.Services;

namespace Treks.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttachmentsController : ControllerBase
    {
        private readonly IAttachmentService _attachmentService;
        private readonly ILogger<AttachmentsController> _logger;

        public AttachmentsController(IAttachmentService attachmentService, ILogger<AttachmentsController> logger)
        {
            _attachmentService = attachmentService;
            _logger = logger;
        }

        [HttpPost("upload/{ticketId}")]
        public async Task<IActionResult> UploadAttachment(string ticketId, IFormFile file)
        {
            try
            {
                var attachment = await _attachmentService.UploadAttachmentAsync(file, ticketId);
                return Ok(attachment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading attachment");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("ticket/{ticketId}")]
        public async Task<IActionResult> GetAttachments(string ticketId)
        {
            var attachments = await _attachmentService.GetAttachmentsByTicketIdAsync(ticketId);
            return Ok(attachments);
        }

        [HttpDelete("{attachmentId}")]
        public async Task<IActionResult> DeleteAttachment(int attachmentId)
        {
            try
            {
                await _attachmentService.DeleteAttachmentAsync(attachmentId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting attachment");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
