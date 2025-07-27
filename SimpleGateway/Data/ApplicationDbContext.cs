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
                entity.Property(e => e.UKWorkExperience).HasColumnType("text");
                entity.Property(e => e.LastPatientTreatment).HasColumnType("text");
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
        }
    }
}
