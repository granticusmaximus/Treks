using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Treks.Data;
using Treks.Models;

namespace Treks.Services
{
    public class TicketService
    {
        private readonly ApplicationDbContext _context;

        public TicketService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Ticket>> GetAllTasksAsync()
        {
            return await _context.Tickets
                .Include(t => t.AssignedUser)
                .Include(t => t.AssignedCompany)
                .ToListAsync();
        }

        public async Task<Ticket> GetTaskByIdAsync(string ticketId, bool includeTechNotes = false)
        {
            IQueryable<Ticket> query = _context.Tickets;

            if (includeTechNotes)
            {
                query = query.Include(t => t.TicketTechNotes)
                             .ThenInclude(ttn => ttn.TechNote);
            }

            return await query.FirstOrDefaultAsync(t => t.TicketId == ticketId);
        }

        public async Task CreateNewTaskAsync(Ticket ticket)
        {
            ticket.TimeOfCreation = DateTime.Now;
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTaskAsync(Ticket updatedTicket)
        {
            var originalTicket = await _context.Tickets
                .Include(t => t.AssignedUser)
                .Include(t => t.AssignedCompany)
                .FirstOrDefaultAsync(t => t.TicketId == updatedTicket.TicketId);

            if (originalTicket == null) return;

            var changeLog = new TicketChangeLog
            {
                TicketId = originalTicket.TicketId,
                ChangedByUserId = updatedTicket.AssignedUser?.FullName ?? "Unknown",
                ChangeDate = DateTime.Now,
                ChangeDescription = GenerateChangeSummary(originalTicket, updatedTicket)
            };

            _context.TicketChangeLogs.Add(changeLog);

            originalTicket.Title = updatedTicket.Title;
            originalTicket.Description = updatedTicket.Description;
            originalTicket.Severity = updatedTicket.Severity;
            originalTicket.DueDate = updatedTicket.DueDate;
            originalTicket.isComplete = updatedTicket.isComplete;
            originalTicket.AssignedUserId = updatedTicket.AssignedUserId;
            originalTicket.AssignedCompanyId = updatedTicket.AssignedCompanyId;

            await _context.SaveChangesAsync();
        }

        private string GenerateChangeSummary(Ticket original, Ticket updated)
        {
            var changes = new List<string>();

            if (original.Title != updated.Title)
                changes.Add($"Title changed from '{original.Title}' to '{updated.Title}'");
            if (original.Description != updated.Description)
                changes.Add("Description updated");
            if (original.Severity != updated.Severity)
                changes.Add($"Severity changed from {original.Severity} to {updated.Severity}");
            if (original.DueDate != updated.DueDate)
                changes.Add($"Due date changed from {original.DueDate.ToShortDateString()} to {updated.DueDate.ToShortDateString()}");
            if (original.isComplete != updated.isComplete)
                changes.Add($"Completion status changed to {(updated.isComplete ? "Complete" : "Incomplete")}");

            return string.Join(", ", changes);
        }

        public async Task DeleteTaskAsync(string ticketId)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.TicketId == ticketId);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddTechNoteToTicketAsync(string ticketId, TechNote techNote)
        {
            if (techNote.Id == 0)
            {
                _context.Notes.Add(techNote);
                await _context.SaveChangesAsync();
            }

            var ticketTechNote = new TicketTechNote
            {
                TicketId = ticketId,
                TechNoteId = techNote.Id
            };

            _context.Add(ticketTechNote);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTechNoteAsync(int techNoteId)
        {
            var ticketTechNotes = await _context.TicketTechNotes
                .Where(ttn => ttn.TechNoteId == techNoteId)
                .ToListAsync();

            _context.TicketTechNotes.RemoveRange(ticketTechNotes);

            var techNote = await _context.Notes.FindAsync(techNoteId);
            if (techNote != null)
            {
                _context.Notes.Remove(techNote);
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddSubTaskAsync(string ticketId, SubTask subTask)
        {
            subTask.TicketId = ticketId;
            _context.SubTasks.Add(subTask);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SubTask>> GetSubTasksForTicketAsync(string ticketId)
        {
            return await _context.SubTasks
                .Where(st => st.TicketId == ticketId)
                .ToListAsync();
        }

        public async Task CompleteSubTaskAsync(int subTaskId)
        {
            var subTask = await _context.SubTasks.FindAsync(subTaskId);
            if (subTask != null)
            {
                subTask.IsComplete = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateSubTaskCompletionAsync(int subTaskId, bool isComplete)
        {
            var subTask = await _context.SubTasks.FindAsync(subTaskId);
            if (subTask != null)
            {
                subTask.IsComplete = isComplete;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteSubTaskAsync(int subTaskId)
        {
            var subTask = await _context.SubTasks.FindAsync(subTaskId);
            if (subTask != null)
            {
                _context.SubTasks.Remove(subTask);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateSubTaskTitleAsync(int subTaskId, string newTitle)
        {
            var subTask = await _context.SubTasks.FindAsync(subTaskId);
            if (subTask != null)
            {
                subTask.Title = newTitle;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateSubTaskDueDateAsync(int subTaskId, DateTime? dueDate)
        {
            var subTask = await _context.SubTasks.FindAsync(subTaskId);
            if (subTask != null)
            {
                subTask.DueDate = dueDate;
                await _context.SaveChangesAsync();
            }
        }
    }
}
