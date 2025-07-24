using Microsoft.EntityFrameworkCore;

namespace SimpleGateway.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets for all our entities - Scalable for future expansion
        public DbSet<UserModel> Users { get; set; }
        public DbSet<PerformerDetailsModel> PerformerDetails { get; set; }
        public DbSet<FileUploadModel> FileUploads { get; set; }
        public DbSet<FileUploadEntry> FileUploadEntries { get; set; }

        // Future expansion ready - Questionnaire system for non-users
        // public DbSet<Questionnaire> Questionnaires { get; set; }
        // public DbSet<QuestionnaireResponse> QuestionnaireResponses { get; set; }
        // public DbSet<ExternalContact> ExternalContacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure UserModel - Foundation for authentication
            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
                entity.Property(e => e.DisplayName).HasMaxLength(100);
            });

            // Configure PerformerDetailsModel - Dental healthcare professional profiles
            modelBuilder.Entity<PerformerDetailsModel>(entity =>
            {
                entity.HasKey(e => e.Username); // Using Username as primary key based on existing structure
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.GDCNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ContactNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.SupportingDentist).IsRequired().HasMaxLength(200);
                entity.Property(e => e.SupportingDentistContactNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.PracticeAddress).IsRequired().HasMaxLength(500);
                entity.Property(e => e.PracticePostCode).IsRequired().HasMaxLength(20);
                entity.Property(e => e.UniversityCountryOfQualification).IsRequired().HasMaxLength(300);
            });

            // Configure FileUploadModel - Scalable document management
            modelBuilder.Entity<FileUploadModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            });

            // Configure FileUploadEntry - Individual file tracking
            modelBuilder.Entity<FileUploadEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FileId).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ContentType).HasMaxLength(100);
                entity.Property(e => e.FilePath).HasMaxLength(500);
            });

            // Seed initial data for immediate functionality
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Users - Ready for immediate testing with actual UserModel properties
            modelBuilder.Entity<UserModel>().HasData(
                new UserModel { Id = 1, Username = "performer1", Password = "password123", Role = "performer", FirstName = "John", LastName = "Smith", DisplayName = "John Smith", Email = "john.smith@example.com", CreatedDate = DateTime.UtcNow, IsActive = true },
                new UserModel { Id = 2, Username = "performer2", Password = "password123", Role = "performer", FirstName = "Sarah", LastName = "Johnson", DisplayName = "Sarah Johnson", Email = "sarah.johnson@example.com", CreatedDate = DateTime.UtcNow, IsActive = true },
                new UserModel { Id = 3, Username = "advisor1", Password = "password123", Role = "advisor", FirstName = "Michael", LastName = "Davis", DisplayName = "Michael Davis", Email = "michael.davis@example.com", CreatedDate = DateTime.UtcNow, IsActive = true },
                new UserModel { Id = 4, Username = "supervisor1", Password = "password123", Role = "supervisor", FirstName = "Emily", LastName = "Wilson", DisplayName = "Emily Wilson", Email = "emily.wilson@example.com", CreatedDate = DateTime.UtcNow, IsActive = true },
                new UserModel { Id = 5, Username = "admin", Password = "admin123", Role = "admin", FirstName = "Admin", LastName = "User", DisplayName = "Administrator", Email = "admin@example.com", CreatedDate = DateTime.UtcNow, IsActive = true }
            );

            // Note: PerformerDetailsModel has different structure - will seed after EF migrations
            // This allows for flexible expansion as you add more features
        }
    }
}
