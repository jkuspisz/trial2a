using Microsoft.EntityFrameworkCore;
using SimpleGateway.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure Railway port (Railway provides PORT environment variable)
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

// Add Entity Framework - SQLite database (file-based, perfect for deployment!)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Add error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

Console.WriteLine($"Starting application on port {port}");

// Ensure database is created and seeded on startup
try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        // For Railway deployment - recreate database with fresh seed data
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        
        Console.WriteLine("Database recreated with fresh seed data successfully");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Database initialization failed: {ex.Message}");
    // Don't crash the app, continue without database
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();

// Add health check endpoint for Railway
app.MapGet("/health", () => "OK");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

Console.WriteLine("Application configured, starting server...");
app.Run();
