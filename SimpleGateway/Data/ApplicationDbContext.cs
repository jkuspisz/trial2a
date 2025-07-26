using Microsoft.EntityFrameworkCore;
using SimpleGateway.Models;

namespace SimpleGateway
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets for all our entities
        public DbSet<UserModel> Users { get; set; }
        public DbSet<PerformerDetailsModel> PerformerDetails { get; set; }
        // Temporarily disabled file upload entities to fix session issues
        // public DbSet<FileUploadModel> FileUploads { get; set; }
        // public DbSet<FileUploadEntry> FileUploadEntries { get; set; }
        public DbSet<AssignmentModel> Assignments { get; set; }
        public DbSet<PreviousExperienceModel> PreviousExperiences { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }
        public DbSet<EmploymentHistoryJob> EmploymentHistoryJobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure UserModel
            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Role).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(255);
            });

            // Configure PerformerDetailsModel
            modelBuilder.Entity<PerformerDetailsModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.GDCNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.ContactNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.SupportingDentist).IsRequired().HasMaxLength(100);
                entity.Property(e => e.SupportingDentistContactNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.PracticeAddress).IsRequired().HasMaxLength(200);
                entity.Property(e => e.PracticePostCode).IsRequired().HasMaxLength(10);
                entity.Property(e => e.UniversityCountryOfQualification).IsRequired().HasMaxLength(100);
            });

            // Temporarily disabled FileUpload configuration to fix session issues
            /*
            // Configure FileUploadModel
            modelBuilder.Entity<FileUploadModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            });

            // Configure FileUploadEntry
            modelBuilder.Entity<FileUploadEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ContentType).HasMaxLength(100);
                entity.Property(e => e.FilePath).HasMaxLength(500);
                entity.Property(e => e.FileUploadModelId).IsRequired();
            });
            */

            // Configure AssignmentModel
            modelBuilder.Entity<AssignmentModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Notes).HasMaxLength(1000);
                
                // Configure foreign key relationships
                entity.HasOne(e => e.Performer)
                      .WithMany()
                      .HasForeignKey(e => e.PerformerId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Supervisor)
                      .WithMany()
                      .HasForeignKey(e => e.SupervisorId)
                      .OnDelete(DeleteBehavior.SetNull);
                
                entity.HasOne(e => e.Advisor)
                      .WithMany()
                      .HasForeignKey(e => e.AdvisorId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure PreviousExperienceModel
            modelBuilder.Entity<PreviousExperienceModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.GdcGapsExplanation).HasMaxLength(2000);
                entity.Property(e => e.NhsExperience).HasMaxLength(1000);
                entity.Property(e => e.FullTime).HasMaxLength(10);
                entity.Property(e => e.PartTimeDaysPerWeek).HasMaxLength(10);
                entity.Property(e => e.Years).HasMaxLength(10);
                entity.Property(e => e.Months).HasMaxLength(10);
                entity.Property(e => e.AdvisorDeclarationBy).HasMaxLength(100);
                entity.Property(e => e.AdvisorDeclarationComment).HasMaxLength(1000);
                
                // Ignore complex collections for now - they have separate tables
                entity.Ignore(e => e.Qualifications);
                entity.Ignore(e => e.EmploymentHistory);
            });

            // Configure Qualification
            modelBuilder.Entity<Qualification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.QualificationName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Country).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Institution).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Year).IsRequired().HasMaxLength(10);
            });

            // Configure EmploymentHistoryJob
            modelBuilder.Entity<EmploymentHistoryJob>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.JobTitle).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(500);
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Users with static dates to prevent deployment issues
            var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            modelBuilder.Entity<UserModel>().HasData(
                new UserModel { Id = 1, Username = "performer1", Password = "password123", Role = "performer", FirstName = "John", LastName = "Smith", DisplayName = "John Smith", Email = "john.smith@example.com", CreatedDate = seedDate, IsActive = true },
                new UserModel { Id = 2, Username = "performer2", Password = "password123", Role = "performer", FirstName = "Jane", LastName = "Johnson", DisplayName = "Jane Johnson", Email = "jane.johnson@example.com", CreatedDate = seedDate, IsActive = true },
                new UserModel { Id = 3, Username = "performer3", Password = "password123", Role = "performer", FirstName = "Mike", LastName = "Wilson", DisplayName = "Mike Wilson", Email = "mike.wilson@example.com", CreatedDate = seedDate, IsActive = true },
                new UserModel { Id = 4, Username = "advisor1", Password = "password123", Role = "advisor", FirstName = "Dr. Sarah", LastName = "Davis", DisplayName = "Dr. Sarah Davis", Email = "sarah.davis@example.com", CreatedDate = seedDate, IsActive = true },
                new UserModel { Id = 5, Username = "supervisor1", Password = "password123", Role = "supervisor", FirstName = "Prof. Robert", LastName = "Brown", DisplayName = "Prof. Robert Brown", Email = "robert.brown@example.com", CreatedDate = seedDate, IsActive = true },
                new UserModel { Id = 6, Username = "admin1", Password = "password123", Role = "admin", FirstName = "Admin", LastName = "User", DisplayName = "Admin User", Email = "admin@example.com", CreatedDate = seedDate, IsActive = true },
                new UserModel { Id = 7, Username = "superuser", Password = "password123", Role = "superuser", FirstName = "Super", LastName = "User", DisplayName = "Super User", Email = "superuser@example.com", CreatedDate = seedDate, IsActive = true }
            );
        }
    }
}
