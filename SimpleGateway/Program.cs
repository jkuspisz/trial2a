using Microsoft.EntityFrameworkCore;
using SimpleGateway.Models;
using SimpleGateway.Data;

var builder = WebApplication.CreateBuilder(args);

// Configure Railway port (Railway provides PORT environment variable)
var port = Environment.GetEnvironmentVariable("PORT") ?? Environment.GetEnvironmentVariable("HTTP_PORT") ?? "8080";
Console.WriteLine($"Starting application on port {port}");
Console.WriteLine($"PORT environment variable: {Environment.GetEnvironmentVariable("PORT")}");
Console.WriteLine($"HTTP_PORT environment variable: {Environment.GetEnvironmentVariable("HTTP_PORT")}");

// Debug: Print ALL environment variables to see what Railway provides
Console.WriteLine("=== ALL ENVIRONMENT VARIABLES ===");
foreach (System.Collections.DictionaryEntry env in Environment.GetEnvironmentVariables())
{
    var key = env.Key?.ToString() ?? "";
    var value = env.Value?.ToString() ?? "";
    if (key.Contains("DATABASE") || key.Contains("POSTGRES") || key.Contains("PORT") || key.Contains("RAILWAY"))
    {
        Console.WriteLine($"{key}: {value}");
    }
}
Console.WriteLine("=== END ENVIRONMENT VARIABLES ===");
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

// Add Entity Framework - Railway PostgreSQL database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // Railway PostgreSQL connection - try multiple possible environment variable names
    var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL") 
                   ?? Environment.GetEnvironmentVariable("POSTGRES_URL")
                   ?? Environment.GetEnvironmentVariable("POSTGRESQL_URL");
    
    string connectionString;
    
    if (!string.IsNullOrEmpty(databaseUrl))
    {
        // Use environment variable if available
        connectionString = databaseUrl;
        Console.WriteLine($"Using connection string from: Environment Variable ({databaseUrl.Substring(0, Math.Min(20, databaseUrl.Length))}...)");
    }
    else
    {
        // Convert Railway PostgreSQL URL to proper Npgsql connection string format
        connectionString = "Host=postgres.railway.internal;Database=railway;Username=postgres;Password=JqzUsDviTmzGBwiuibsJFPMflWkgiGAS;Port=5432;";
        Console.WriteLine($"Using connection string from: Converted Railway PostgreSQL format");
    }
    
    Console.WriteLine($"DATABASE_URL exists: {!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DATABASE_URL"))}");
    Console.WriteLine($"POSTGRES_URL exists: {!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("POSTGRES_URL"))}");
    Console.WriteLine($"Connection string: {connectionString}");
    
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Database connection string not found");
    }
    
    // Always use PostgreSQL for Railway deployment
    Console.WriteLine("Using PostgreSQL database (Railway)");
    options.UseNpgsql(connectionString);
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
        
        // Test database connection first
        Console.WriteLine("Testing database connection...");
        var canConnect = context.Database.CanConnect();
        Console.WriteLine($"Database connection test: {(canConnect ? "SUCCESS" : "FAILED")}");
        
        if (!canConnect)
        {
            Console.WriteLine("Database connection failed. Attempting to create database...");
            context.Database.EnsureCreated();
            Console.WriteLine("Database creation attempted");
        }
        
        // Use migrations to ensure database is up to date
        Console.WriteLine("Running database migrations...");
        try 
        {
            context.Database.Migrate();
            Console.WriteLine("Database migrations completed successfully");
        }
        catch (Exception migrationEx)
        {
            Console.WriteLine($"Migration error: {migrationEx.Message}");
            Console.WriteLine("Attempting to ensure database is created...");
            context.Database.EnsureCreated();
            Console.WriteLine("Database ensure created completed");
        }
        
        // Verify tables exist
        Console.WriteLine("Verifying database tables...");
        try
        {
            var usersCount = context.Users.Count();
            var performerDetailsCount = context.PerformerDetails.Count();
            var assignmentsCount = context.Assignments.Count();
            var testDataCount = context.TestData.Count();
            var testData2Count = context.TestData2.Count();
            
            Console.WriteLine($"Database table verification:");
            Console.WriteLine($"  Users: {usersCount} records");
            Console.WriteLine($"  PerformerDetails: {performerDetailsCount} records");
            Console.WriteLine($"  Assignments: {assignmentsCount} records");
            Console.WriteLine($"  TestData: {testDataCount} records");
            Console.WriteLine($"  TestData2: {testData2Count} records");
        }
        catch (Exception tableEx)
        {
            Console.WriteLine($"Table verification error: {tableEx.Message}");
            Console.WriteLine("Database tables may not exist properly");
            
            // Always attempt to create both TestData tables if there are any table errors
            Console.WriteLine("Attempting to create TestData tables...");
            try
            {
                // Execute raw SQL to create TestData table
                context.Database.ExecuteSqlRaw(@"
                    CREATE TABLE IF NOT EXISTS ""TestData"" (
                        ""Id"" serial PRIMARY KEY,
                        ""UKWorkExperience"" text NOT NULL,
                        ""LastPatientTreatment"" text NOT NULL,
                        ""Username"" text NOT NULL,
                        ""CreatedDate"" timestamp with time zone NOT NULL,
                        ""ModifiedDate"" timestamp with time zone
                    );
                ");
                Console.WriteLine("TestData table created successfully");
                
                // Execute raw SQL to create TestData2 table
                context.Database.ExecuteSqlRaw(@"
                    CREATE TABLE IF NOT EXISTS ""TestData2"" (
                        ""Id"" serial PRIMARY KEY,
                        ""UKWorkExperience"" text NOT NULL,
                        ""LastPatientTreatment"" text NOT NULL,
                        ""Username"" text NOT NULL,
                        ""CreatedDate"" timestamp with time zone NOT NULL,
                        ""ModifiedDate"" timestamp with time zone
                    );
                ");
                Console.WriteLine("TestData2 table created successfully");
                
                // Verify both tables now exist
                var testDataCount = context.TestData.Count();
                var testData2Count = context.TestData2.Count();
                Console.WriteLine($"TestData table verification: {testDataCount} records");
                Console.WriteLine($"TestData2 table verification: {testData2Count} records");
            }
            catch (Exception createEx)
            {
                Console.WriteLine($"Failed to create TestData tables: {createEx.Message}");
            }
        }
        
        // Additional data verification
        try
        {
            var performerDetailsData = context.PerformerDetails.ToList();
            if (performerDetailsData.Count > 0)
            {
                Console.WriteLine($"Existing PerformerDetails:");
                foreach (var detail in performerDetailsData)
                {
                    Console.WriteLine($"- Username: {detail.Username}, FirstName: {detail.FirstName}, LastName: {detail.LastName}, GDC: {detail.GDCNumber}");
                }
            }
        }
        catch (Exception dataEx)
        {
            Console.WriteLine($"Error reading performer details: {dataEx.Message}");
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
