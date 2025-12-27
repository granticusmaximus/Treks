using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Treks.Areas.Identity;
using Treks.Data;
using Treks.Services;
using Treks.Models;
using System.Net;
using System.Net.Mail;
using Treks.Hubs;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddRoles<Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<IClaimsTransformation, RoleClaimsTransformation>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ManagerOrAdmin", policy => policy.RequireRole("Admin", "Manager"));
    options.AddPolicy("Staff", policy => policy.RequireRole("Admin", "Manager", "Developer", "User"));
});
builder.Services.AddScoped<ApplicationUserService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<TicketService>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<CompanyFileService>();
builder.Services.AddScoped<AccessControlService>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => options.DetailedErrors = true);
builder.Services.AddScoped<UploadService>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddSingleton<ToastService>();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// Add EmailSender service
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<SmtpSettings>>().Value;
    if (string.IsNullOrWhiteSpace(settings.Host))
    {
        throw new InvalidOperationException("SmtpSettings:Host is required.");
    }

    var client = new SmtpClient
    {
        Host = settings.Host,
        Port = settings.Port,
        EnableSsl = settings.EnableSsl
    };

    if (!string.IsNullOrWhiteSpace(settings.UserName))
    {
        client.Credentials = new NetworkCredential(settings.UserName, settings.Password);
    }

    return client;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapHub<NotificationHub>("/notificationhub");

app.Run();
