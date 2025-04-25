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

        public async Task UpdateTaskAsync(Ticket ticket)
        {
            var existingTicket = await _context.Tickets.FindAsync(ticket.TicketId);
            if (existingTicket != null)
            {
                existingTicket.isComplete = ticket.isComplete;
                existingTicket.Title = ticket.Title;
                existingTicket.Description = ticket.Description;
                existingTicket.Severity = ticket.Severity;
                existingTicket.DueDate = ticket.DueDate;
                await _context.SaveChangesAsync();
            }
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
            // Remove relationships in the junction table
            var ticketTechNotes = await _context.TicketTechNotes
                .Where(ttn => ttn.TechNoteId == techNoteId)
                .ToListAsync();

            _context.TicketTechNotes.RemoveRange(ticketTechNotes);

            // Remove the TechNote itself
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
            // Logic to update the completion status of a subtask
            var subTask = await _context.SubTasks.FindAsync(subTaskId);
            if (subTask != null)
            {
                subTask.IsComplete = isComplete;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateSubTaskAsync(SubTask subTask)
        {
            var existing = await _context.SubTasks.FindAsync(subTask.SubTaskId);
            if (existing != null)
            {
                existing.Title = subTask.Title;
                existing.IsComplete = subTask.IsComplete;
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
    }
}