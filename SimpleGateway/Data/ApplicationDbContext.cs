using Microsoft.EntityFrameworkCore;
using SimpleGateway.Models;

namespace SimpleGateway.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets for all our entities
        public DbSet<UserModel> Users { get; set; }
        public DbSet<PerformerDetailsModel> PerformerDetails { get; set; }
        public DbSet<FileUploadModel> FileUploads { get; set; }
        public DbSet<FileUploadEntry> FileUploadEntries { get; set; }
        public DbSet<AssignmentModel> Assignments { get; set; }
        public DbSet<TestDataModel> TestData { get; set; }
        public DbSet<TestDataModel2> TestData2 { get; set; }
        public DbSet<StructuredConversationModel> StructuredConversations { get; set; }
        public DbSet<WorkBasedAssessmentModel> WorkBasedAssessments { get; set; }
        public DbSet<AgreementTermsModel> AgreementTerms { get; set; }
        public DbSet<MSFQuestionnaire> MSFQuestionnaires { get; set; }
        public DbSet<MSFResponse> MSFResponses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure UserModel
            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique();
            });

            // Configure PerformerDetailsModel
            modelBuilder.Entity<PerformerDetailsModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique();
            });

            // Configure AssignmentModel
            modelBuilder.Entity<AssignmentModel>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            // Configure TestDataModel
            modelBuilder.Entity<TestDataModel>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            // Configure TestDataModel2
            modelBuilder.Entity<TestDataModel2>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UKWorkExperience).HasColumnType("text");
                entity.Property(e => e.LastPatientTreatment).HasColumnType("text");
                entity.Property(e => e.Username).HasMaxLength(256);
                
                // Configure date fields as text for flexibility
                entity.Property(e => e.ClinicalExamLastDate).HasMaxLength(50);
                entity.Property(e => e.BPELastDate).HasMaxLength(50);
                entity.Property(e => e.SixPPCLastDate).HasMaxLength(50);
                entity.Property(e => e.RadiographsLastDate).HasMaxLength(50);
                entity.Property(e => e.ExtractionsLastDate).HasMaxLength(50);
                entity.Property(e => e.RootDivisionLastDate).HasMaxLength(50);
                entity.Property(e => e.ScalingLastDate).HasMaxLength(50);
                entity.Property(e => e.AmalgamLastDate).HasMaxLength(50);
                entity.Property(e => e.CompositeLastDate).HasMaxLength(50);
                entity.Property(e => e.CrownsLastDate).HasMaxLength(50);
                entity.Property(e => e.DenturesLastDate).HasMaxLength(50);
                entity.Property(e => e.SingleEndoLastDate).HasMaxLength(50);
                entity.Property(e => e.MultiEndoLastDate).HasMaxLength(50);
                entity.Property(e => e.PeadsLastDate).HasMaxLength(50);
                
                // Set default values for boolean need support fields
                entity.Property(e => e.ClinicalExamNeedSupport).HasDefaultValue(false);
                entity.Property(e => e.BPENeedSupport).HasDefaultValue(false);
                entity.Property(e => e.SixPPCNeedSupport).HasDefaultValue(false);
                entity.Property(e => e.RadiographsNeedSupport).HasDefaultValue(false);
                entity.Property(e => e.ExtractionsNeedSupport).HasDefaultValue(false);
                entity.Property(e => e.RootDivisionNeedSupport).HasDefaultValue(false);
                entity.Property(e => e.ScalingNeedSupport).HasDefaultValue(false);
                entity.Property(e => e.AmalgamNeedSupport).HasDefaultValue(false);
                entity.Property(e => e.CompositeNeedSupport).HasDefaultValue(false);
                entity.Property(e => e.CrownsNeedSupport).HasDefaultValue(false);
                entity.Property(e => e.DenturesNeedSupport).HasDefaultValue(false);
                entity.Property(e => e.SingleEndoNeedSupport).HasDefaultValue(false);
                entity.Property(e => e.MultiEndoNeedSupport).HasDefaultValue(false);
                entity.Property(e => e.PeadsNeedSupport).HasDefaultValue(false);
            });

            // Configure StructuredConversationModel
            modelBuilder.Entity<StructuredConversationModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).HasMaxLength(256);
                entity.Property(e => e.ClinicalExperienceSummary).HasColumnType("text");
                entity.Property(e => e.DevelopmentNeeds).HasColumnType("text");
                entity.Property(e => e.SupervisorSummary).HasColumnType("text");
                entity.Property(e => e.AdvisorComments).HasColumnType("text");
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed users - using static dates to avoid migration warnings
            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            modelBuilder.Entity<UserModel>().HasData(
                new UserModel
                {
                    Id = 1,
                    Username = "admin",
                    Password = "$2a$11$K5ZfJ5WMzHPbfT6Gw4YzWOxKoKjQnN8qFZHlUZ7QJC8CgCYzq8dUy", // "admin123"
                    FirstName = "Admin",
                    LastName = "User",
                    DisplayName = "Administrator",
                    Role = "Admin",
                    Email = "admin@example.com",
                    CreatedDate = seedDate
                },
                new UserModel
                {
                    Id = 2,
                    Username = "performer1",
                    Password = "$2a$11$nrmQZZ2qLbT4mFyf3u4D9uqNPqK9M1XKoFVh8kWIUx8rRyJhU7uBK", // "performer123"
                    FirstName = "John",
                    LastName = "Smith",
                    DisplayName = "Dr. John Smith",
                    Role = "Performer",
                    Email = "john.smith@example.com",
                    CreatedDate = seedDate
                },
                new UserModel
                {
                    Id = 3,
                    Username = "james",
                    Password = "$2a$11$X.9UhD2YhUYqZpD3.eM5yOY4z5ZkKUGKjGP.rM6OqQC5eEfLHdDFu", // "james123"
                    FirstName = "James",
                    LastName = "Brown",
                    DisplayName = "Dr. James Brown",
                    Role = "Performer",
                    Email = "james.brown@example.com",
                    CreatedDate = seedDate
                },
                new UserModel
                {
                    Id = 4,
                    Username = "agency1",
                    Password = "$2a$11$Th4qZ8zSB5qF9kJ1OzOhzOmA8pG3u2CpLn5RtD9HwGv6mJkF3uP2w", // "agency123"
                    FirstName = "Agency",
                    LastName = "Manager",
                    DisplayName = "Agency Manager",
                    Role = "Agency",
                    Email = "agency@example.com",
                    CreatedDate = seedDate
                }
            );

            // Configure WorkBasedAssessmentModel
            modelBuilder.Entity<WorkBasedAssessmentModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).HasMaxLength(256);
                entity.Property(e => e.AssessmentType).HasMaxLength(50);
                entity.Property(e => e.Title).HasMaxLength(500);
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.ProcedureDescription).HasColumnType("text");
                entity.Property(e => e.LearningReflection).HasColumnType("text");
                entity.Property(e => e.LearningNeeds).HasColumnType("text");
                entity.Property(e => e.SupervisorActionPlan).HasColumnType("text");
                entity.Property(e => e.CompletedBySupervisor).HasMaxLength(256);
            });

            // Configure AgreementTermsModel
            modelBuilder.Entity<AgreementTermsModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).HasMaxLength(256);
            });

            // Configure MSFQuestionnaire
            modelBuilder.Entity<MSFQuestionnaire>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
                entity.Property(e => e.UniqueCode).IsRequired();
                entity.HasIndex(e => e.UniqueCode).IsUnique();
                
                entity.HasOne(q => q.Performer)
                    .WithMany()
                    .HasForeignKey(q => q.PerformerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure MSFResponse
            modelBuilder.Entity<MSFResponse>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.HasOne(r => r.Questionnaire)
                    .WithMany(q => q.Responses)
                    .HasForeignKey(r => r.MSFQuestionnaireId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
