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
        context.Database.EnsureCreated();
        
        // Check if we need to add missing users (like superuser)
        if (!context.Users.Any(u => u.Username == "superuser"))
        {
            context.Users.Add(new UserModel 
            { 
                Username = "superuser", 
                Password = "password123", 
                Role = "superuser", 
                FirstName = "Super", 
                LastName = "User", 
                DisplayName = "Super User", 
                Email = "superuser@example.com", 
                CreatedDate = DateTime.UtcNow, 
                IsActive = true 
            });
        }
        
        // Check if admin1 exists (in case it was missed)
        if (!context.Users.Any(u => u.Username == "admin1"))
        {
            context.Users.Add(new UserModel 
            { 
                Username = "admin1", 
                Password = "password123", 
                Role = "admin", 
                FirstName = "Admin", 
                LastName = "User", 
                DisplayName = "Admin User", 
                Email = "admin@example.com", 
                CreatedDate = DateTime.UtcNow, 
                IsActive = true 
            });
        }
        
        context.SaveChanges();
        Console.WriteLine("Database created and users verified successfully");
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
