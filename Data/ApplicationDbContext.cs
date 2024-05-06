using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Treks.Models;

namespace Treks.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, string>  // Use your custom Role class
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Ticket>()
            .Property(t => t.Severity)
            .HasConversion<string>();
        modelBuilder.Entity<TicketTechNote>()
            .HasKey(ttn => new { ttn.TicketId, ttn.TechNoteId });

        modelBuilder.Entity<TicketTechNote>()
            .HasOne(ttn => ttn.Ticket)
            .WithMany(t => t.TicketTechNotes)
            .HasForeignKey(ttn => ttn.TicketId);

        modelBuilder.Entity<TicketTechNote>()
            .HasOne(ttn => ttn.TechNote)
            .WithMany(tn => tn.TicketTechNotes)
            .HasForeignKey(ttn => ttn.TechNoteId);

        modelBuilder.Entity<ApplicationUser>()
            .HasMany(u => u.AssignedTickets) 
            .WithOne(t => t.AssignedUser)
            .HasForeignKey(t => t.AssignedUserId)
            .IsRequired(false);
        
        modelBuilder.Entity<LUT_Comments>()
            .HasOne(c=> c.Company)
            .WithMany(c => c.Comments)
            .HasForeignKey(c => c.CompanyId);

        modelBuilder.Entity<LUT_Comments>()
            .HasOne(c => c.Comment)
            .WithMany(c => c.Comments)
            .HasForeignKey(c => c.CommentId);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.ParentComment)
            .WithMany(c => c.ChildComments)
            .HasForeignKey(c => c.ParentCommentId)
            .OnDelete(DeleteBehavior.Restrict); // Prevents cascade delete

    }

    public DbSet<ApplicationUser> ApplicationUser { get; set; }
    public DbSet<TechNote> Notes { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketTechNote> TicketTechNotes { get; set; }
    public DbSet<LUT_UserRoles> LUT_UserRoles { get; set; }
    public DbSet<LUT_Comments> LUT_Comments { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Comment> Comments { get; set; }

}