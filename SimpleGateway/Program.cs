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

// Add Entity Framework - Auto-detect database type
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
    var connectionString = databaseUrl ?? builder.Configuration.GetConnectionString("DefaultConnection");
    
    Console.WriteLine($"DATABASE_URL exists: {!string.IsNullOrEmpty(databaseUrl)}");
    Console.WriteLine($"Connection string: {connectionString}");
    
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Database connection string not found. Please set DATABASE_URL environment variable or DefaultConnection in appsettings.json");
    }
    
    // Auto-detect database type based on connection string
    if (connectionString.StartsWith("postgresql://") || connectionString.Contains("postgresql"))
    {
        Console.WriteLine("Using PostgreSQL database");
        options.UseNpgsql(connectionString);
    }
    else
    {
        Console.WriteLine("Using SQLite database");
        options.UseSqlite(connectionString);
    }
});

var app = builder.Build();

// Add error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

Console.WriteLine($"Starting application on port {port}");

// Initialize database on startup
try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        var connectionString = context.Database.GetConnectionString();
        Console.WriteLine($"Database connection string: {connectionString}");
        
        // Use migrations to ensure database is up to date
        Console.WriteLine("Running database migrations...");
        context.Database.Migrate();
        Console.WriteLine("Database migrations completed successfully");
        
        // Check database content
        var performerDetailsCount = context.PerformerDetails.Count();
        Console.WriteLine($"PerformerDetails records in database: {performerDetailsCount}");
        
        if (performerDetailsCount > 0)
        {
            var performerDetails = context.PerformerDetails.ToList();
            Console.WriteLine($"Existing PerformerDetails:");
            foreach (var detail in performerDetails)
            {
                Console.WriteLine($"- Username: {detail.Username}, FirstName: {detail.FirstName}, LastName: {detail.LastName}, GDC: {detail.GDCNumber}");
            }
        }
        
        // Debug: Check what users actually exist in the database
        var existingUsers = context.Users.ToList();
        Console.WriteLine($"Database has {existingUsers.Count} users:");
        var seedUsers = existingUsers.Where(u => u.CreatedDate.Year == 2024 && u.CreatedDate.Month == 1 && u.CreatedDate.Day == 1).ToList();
        var newUsers = existingUsers.Where(u => !(u.CreatedDate.Year == 2024 && u.CreatedDate.Month == 1 && u.CreatedDate.Day == 1)).ToList();
        
        Console.WriteLine($"Seed users ({seedUsers.Count}):");
        foreach (var user in seedUsers)
        {
            Console.WriteLine($"- {user.Username} ({user.Role}) - Created: {user.CreatedDate}");
        }
        
        Console.WriteLine($"User-created users ({newUsers.Count}):");
        foreach (var user in newUsers)
        {
            Console.WriteLine($"- {user.Username} ({user.Role}) - Created: {user.CreatedDate}");
        }
        
        // Add missing users if they don't exist
        var usersToAdd = new[]
        {
            new { Username = "admin1", Role = "admin", FirstName = "Admin", LastName = "User", DisplayName = "Admin User", Email = "admin@example.com" },
            new { Username = "superuser", Role = "superuser", FirstName = "Super", LastName = "User", DisplayName = "Super User", Email = "superuser@example.com" }
        };
        
        bool addedUsers = false;
        foreach (var userTemplate in usersToAdd)
        {
            if (!context.Users.Any(u => u.Username == userTemplate.Username))
            {
                context.Users.Add(new UserModel
                {
                    Username = userTemplate.Username,
                    Password = "password123",
                    Role = userTemplate.Role,
                    FirstName = userTemplate.FirstName,
                    LastName = userTemplate.LastName,
                    DisplayName = userTemplate.DisplayName,
                    Email = userTemplate.Email,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                });
                addedUsers = true;
                Console.WriteLine($"Added missing user: {userTemplate.Username}");
            }
        }
        
        if (addedUsers)
        {
            context.SaveChanges();
            Console.WriteLine("Missing users added successfully");
        }
        
        Console.WriteLine("Database initialization completed");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Database initialization failed: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
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
Console.WriteLine("Data persistence test: Database should survive this deployment");
app.Run();
