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

// Ensure database directory exists (for Railway persistent storage)
var dataDir = "/app/data";
Console.WriteLine($"Checking data directory: {dataDir}");
Console.WriteLine($"Data directory exists: {Directory.Exists(dataDir)}");

if (!Directory.Exists(dataDir))
{
    Directory.CreateDirectory(dataDir);
    Console.WriteLine($"Created data directory: {dataDir}");
}
else
{
    Console.WriteLine($"Data directory already exists");
    // List existing files in data directory
    var files = Directory.GetFiles(dataDir);
    Console.WriteLine($"Files in data directory ({files.Length}):");
    foreach (var file in files)
    {
        var fileInfo = new FileInfo(file);
        Console.WriteLine($"- {Path.GetFileName(file)} ({fileInfo.Length} bytes, modified: {fileInfo.LastWriteTime})");
    }
}

// Check Railway environment
Console.WriteLine($"Railway environment variables:");
Console.WriteLine($"- RAILWAY_ENVIRONMENT: {Environment.GetEnvironmentVariable("RAILWAY_ENVIRONMENT")}");
Console.WriteLine($"- RAILWAY_PROJECT_ID: {Environment.GetEnvironmentVariable("RAILWAY_PROJECT_ID")}");
Console.WriteLine($"- RAILWAY_SERVICE_ID: {Environment.GetEnvironmentVariable("RAILWAY_SERVICE_ID")}");
Console.WriteLine($"- PWD: {Environment.GetEnvironmentVariable("PWD")}");
Console.WriteLine($"- Current working directory: {Directory.GetCurrentDirectory()}");

// Ensure database is created and seeded on startup
try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        var connectionString = context.Database.GetConnectionString();
        Console.WriteLine($"Database connection string: {connectionString}");
        
        // Check if database file exists before migration
        var dbPath = "/app/data/SimpleGateway.db";
        var dbExists = File.Exists(dbPath);
        Console.WriteLine($"Database file exists before migration: {dbExists}");
        if (dbExists)
        {
            var fileInfo = new FileInfo(dbPath);
            Console.WriteLine($"Database file size: {fileInfo.Length} bytes, Last modified: {fileInfo.LastWriteTime}");
        }
        
        // Use migrations instead of EnsureCreated to preserve existing data
        context.Database.Migrate();
        
        // Check database file after migration
        var dbExistsAfter = File.Exists(dbPath);
        Console.WriteLine($"Database file exists after migration: {dbExistsAfter}");
        if (dbExistsAfter)
        {
            var fileInfoAfter = new FileInfo(dbPath);
            Console.WriteLine($"Database file size after migration: {fileInfoAfter.Length} bytes, Last modified: {fileInfoAfter.LastWriteTime}");
        }
        
        // Check PerformerDetails data specifically
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
app.Run();
