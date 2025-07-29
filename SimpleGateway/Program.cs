using Microsoft.EntityFrameworkCore;
using SimpleGateway.Models;
using SimpleGateway.Data;

var builder = WebApplication.CreateBuilder(args);

// Configure Railway port (Railway provides PORT environment variable)
var port = Environment.GetEnvironmentVariable("PORT") ?? Environment.GetEnvironmentVariable("HTTP_PORT") ?? "8080";
Console.WriteLine($"Starting application on port {port}");
Console.WriteLine($"PORT environment variable: {Environment.GetEnvironmentVariable("PORT")}");
Console.WriteLine($"HTTP_PORT environment variable: {Environment.GetEnvironmentVariable("HTTP_PORT")}");
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

// Add Entity Framework - Railway PostgreSQL database with SEPARATE CONTEXTS
// Main context for critical data (Users, PerformerDetails, etc.)
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
    Console.WriteLine("Using PostgreSQL database (Railway) - Main Context");
    options.UseNpgsql(connectionString);
});

// ISOLATED TestData2 context - separate migrations, separate safety
builder.Services.AddDbContext<TestData2Context>(options =>
{
    // Use same connection but different migration history
    var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL") 
                   ?? Environment.GetEnvironmentVariable("POSTGRES_URL")
                   ?? Environment.GetEnvironmentVariable("POSTGRESQL_URL");
    
    string connectionString;
    
    if (!string.IsNullOrEmpty(databaseUrl))
    {
        connectionString = databaseUrl;
    }
    else
    {
        connectionString = "Host=postgres.railway.internal;Database=railway;Username=postgres;Password=JqzUsDviTmzGBwiuibsJFPMflWkgiGAS;Port=5432;";
    }
    
    Console.WriteLine("Using PostgreSQL database (Railway) - TestData2 Context (ISOLATED)");
    options.UseNpgsql(connectionString, npgsqlOptions => 
    {
        // Use separate migration history table to isolate TestData2 migrations
        npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory_TestData2");
    });
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
        // Initialize MAIN context (Users, PerformerDetails, etc.)
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
        
        // Initialize ISOLATED TestData2 context separately
        var testData2Context = scope.ServiceProvider.GetRequiredService<TestData2Context>();
        Console.WriteLine("Initializing isolated TestData2Context...");
        
        var testData2CanConnect = testData2Context.Database.CanConnect();
        Console.WriteLine($"TestData2Context connection test: {(testData2CanConnect ? "SUCCESS" : "FAILED")}");
        
        if (!testData2CanConnect)
        {
            Console.WriteLine("TestData2Context connection failed. Attempting to create...");
            testData2Context.Database.EnsureCreated();
            Console.WriteLine("TestData2Context creation attempted");
        }
        
        // Use migrations to ensure database is up to date
        Console.WriteLine("Running database migrations...");
        try 
        {
            // Check for pending migrations
            var pendingMigrations = context.Database.GetPendingMigrations().ToList();
            if (pendingMigrations.Any())
            {
                Console.WriteLine($"Found {pendingMigrations.Count} pending migrations:");
                foreach (var migration in pendingMigrations)
                {
                    Console.WriteLine($"  - {migration}");
                    if (migration.Contains("StructuredConversation"))
                    {
                        Console.WriteLine($"    *** STRUCTURED CONVERSATION MIGRATION DETECTED ***");
                    }
                }
            }
            else
            {
                Console.WriteLine("No pending migrations found");
            }
            
            // Apply migrations for MAIN context
            context.Database.Migrate();
            Console.WriteLine("Main database migrations completed successfully");
            
            // Verify StructuredConversations table was created
            try
            {
                var structuredConvCount = context.StructuredConversations.Count();
                Console.WriteLine($"StructuredConversations table verification: {structuredConvCount} records found");
            }
            catch (Exception tableEx)
            {
                Console.WriteLine($"StructuredConversations table verification FAILED: {tableEx.Message}");
                
                // FALLBACK: Manual table creation for Railway deployment issues
                if (tableEx.Message.Contains("does not exist") || tableEx.Message.Contains("relation") || tableEx.Message.Contains("StructuredConversations"))
                {
                    Console.WriteLine("Attempting manual StructuredConversations table creation...");
                    try
                    {
                        var createTableSql = @"
                            CREATE TABLE IF NOT EXISTS ""StructuredConversations"" (
                                ""Id"" integer GENERATED BY DEFAULT AS IDENTITY,
                                ""ClinicalExperienceSummary"" text NULL,
                                ""DevelopmentNeeds"" text NULL,
                                ""SupervisorSummary"" text NULL,
                                ""AdvisorComments"" text NULL,
                                ""Username"" character varying(256) NOT NULL,
                                ""CreatedDate"" timestamp with time zone NOT NULL,
                                ""ModifiedDate"" timestamp with time zone NULL,
                                CONSTRAINT ""PK_StructuredConversations"" PRIMARY KEY (""Id"")
                            );";
                        
                        context.Database.ExecuteSqlRaw(createTableSql);
                        Console.WriteLine("✅ StructuredConversations table created manually");
                        
                        // Verify again
                        var verifyCount = context.StructuredConversations.Count();
                        Console.WriteLine($"✅ Manual table creation verified: {verifyCount} records found");
                    }
                    catch (Exception manualEx)
                    {
                        Console.WriteLine($"Manual table creation failed: {manualEx.Message}");
                    }
                }
            }
            
            // Apply migrations for ISOLATED TestData2 context
            Console.WriteLine("=== TESTDATA2 CONTEXT MIGRATION START ===");
            try
            {
                // Check connection specifically for TestData2Context
                var testData2CanConnectForMigration = testData2Context.Database.CanConnect();
                Console.WriteLine($"TestData2Context migration connection test: {(testData2CanConnectForMigration ? "SUCCESS" : "FAILED")}");
                
                var testData2PendingMigrations = testData2Context.Database.GetPendingMigrations().ToList();
                if (testData2PendingMigrations.Any())
                {
                    Console.WriteLine($"Found {testData2PendingMigrations.Count} pending TestData2 migrations:");
                    foreach (var migration in testData2PendingMigrations)
                    {
                        Console.WriteLine($"  - {migration}");
                    }
                }
                else
                {
                    Console.WriteLine("No pending TestData2 migrations found");
                }
                
                // Apply TestData2Context migrations
                Console.WriteLine("Applying TestData2Context migrations...");
                testData2Context.Database.Migrate();
                Console.WriteLine("TestData2Context migrations completed successfully");
                
                // Verify TestData2 table exists
                try
                {
                    var testData2Count = testData2Context.TestData2.Count();
                    Console.WriteLine($"TestData2 table verification: {testData2Count} records found");
                }
                catch (Exception verifyEx)
                {
                    Console.WriteLine($"TestData2 table verification failed: {verifyEx.Message}");
                }
            }
            catch (Exception testData2MigrationEx)
            {
                Console.WriteLine($"TestData2Context migration error: {testData2MigrationEx.Message}");
                Console.WriteLine($"TestData2Context migration stack trace: {testData2MigrationEx.StackTrace}");
                
                // Try to ensure created for TestData2Context as fallback
                try
                {
                    Console.WriteLine("Attempting TestData2Context.Database.EnsureCreated() as fallback...");
                    testData2Context.Database.EnsureCreated();
                    Console.WriteLine("TestData2Context ensure created completed");
                    
                    // Verify after EnsureCreated
                    var testData2Count = testData2Context.TestData2.Count();
                    Console.WriteLine($"TestData2 table created via EnsureCreated: {testData2Count} records found");
                }
                catch (Exception ensureEx)
                {
                    Console.WriteLine($"TestData2Context EnsureCreated also failed: {ensureEx.Message}");
                }
            }
            Console.WriteLine("=== TESTDATA2 CONTEXT MIGRATION END ===");
            
            // Verify applied migrations
            var appliedMigrations = context.Database.GetAppliedMigrations().ToList();
            Console.WriteLine($"Applied migrations ({appliedMigrations.Count}):");
            foreach (var migration in appliedMigrations.TakeLast(3)) // Show last 3
            {
                Console.WriteLine($"  - {migration}");
            }
        }
        catch (Exception migrationEx)
        {
            Console.WriteLine($"Migration error: {migrationEx.Message}");
            Console.WriteLine($"Migration stack trace: {migrationEx.StackTrace}");
            
            // Only fall back to EnsureCreated if migrations completely fail
            Console.WriteLine("Attempting to ensure database is created as fallback...");
            try
            {
                context.Database.EnsureCreated();
                Console.WriteLine("Database ensure created completed");
            }
            catch (Exception ensureEx)
            {
                Console.WriteLine($"EnsureCreated also failed: {ensureEx.Message}");
                throw; // Re-throw to stop startup if database can't be initialized
            }
        }
        
        // Verify tables exist
        Console.WriteLine("Verifying database tables...");
        try
        {
            var usersCount = context.Users.Count();
            var performerDetailsCount = context.PerformerDetails.Count();
            var assignmentsCount = context.Assignments.Count();
            var testDataCount = context.TestData.Count();
            
            // TestData2 is now in isolated context
            var testData2Count = testData2Context.TestData2.Count();
            
            Console.WriteLine($"Database table verification:");
            Console.WriteLine($"  Users: {usersCount} records");
            Console.WriteLine($"  PerformerDetails: {performerDetailsCount} records");
            Console.WriteLine($"  Assignments: {assignmentsCount} records");
            Console.WriteLine($"  TestData: {testDataCount} records");
            Console.WriteLine($"  TestData2: {testData2Count} records (ISOLATED CONTEXT)");
        }
        catch (Exception tableEx)
        {
            Console.WriteLine($"Table verification error: {tableEx.Message}");
            Console.WriteLine("This is expected if migrations haven't been applied yet");
            
            // If table verification fails, it likely means migrations need to run
            // The migrations should handle creating tables with proper schema
            Console.WriteLine("Tables will be created by migrations on next deployment");
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
