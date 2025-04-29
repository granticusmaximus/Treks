using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Treks.Data;
using Treks.Models;

namespace Treks.Services
{
    public interface IAttachmentService
    {
        Task<TicketAttachment> UploadAttachmentAsync(IFormFile file, string ticketId);
        Task<List<TicketAttachment>> GetAttachmentsByTicketIdAsync(string ticketId);
        Task<bool> DeleteAttachmentAsync(int attachmentId);
    }

    public class AttachmentService : IAttachmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AttachmentService(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<TicketAttachment> UploadAttachmentAsync(IFormFile file, string ticketId)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "attachments");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var attachment = new TicketAttachment
            {
                TicketId = ticketId,
                FileName = file.FileName,
                FilePath = $"/attachments/{uniqueFileName}",
                UploadedAt = DateTime.UtcNow
            };

            _context.Attachments.Add(attachment);
            await _context.SaveChangesAsync();

            return attachment;
        }

        public async Task<List<TicketAttachment>> GetAttachmentsByTicketIdAsync(string ticketId)
        {
            return await _context.Attachments
                .Where(a => a.TicketId == ticketId)
                .OrderByDescending(a => a.UploadedAt)
                .ToListAsync();
        }

        public async Task<bool> DeleteAttachmentAsync(int attachmentId)
        {
            var attachment = await _context.Attachments.FindAsync(attachmentId);
            if (attachment == null) return false;

            var fullPath = Path.Combine(_env.WebRootPath, attachment.FilePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            _context.Attachments.Remove(attachment);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
