using Microsoft.EntityFrameworkCore;
using SimpleGateway.Models;

namespace SimpleGateway.Data
{
    /// <summary>
    /// Isolated database context for TestData2 operations only.
    /// This prevents TestData2 migrations from affecting user management tables.
    /// </summary>
    public class TestData2Context : DbContext
    {
        public TestData2Context(DbContextOptions<TestData2Context> options) : base(options)
        {
        }

        // Only TestData2 - completely isolated from Users, PerformerDetails, etc.
        public DbSet<TestDataModel2> TestData2 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure TestDataModel2 with all safety measures
            modelBuilder.Entity<TestDataModel2>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                // Ensure username indexing for performance
                entity.HasIndex(e => e.Username);
                
                // Configure text fields for PostgreSQL
                entity.Property(e => e.UKWorkExperience).HasColumnType("text");
                entity.Property(e => e.LastPatientTreatment).HasColumnType("text");
                
                // Configure registration fields
                entity.Property(e => e.GDCRegistrationNumber).HasMaxLength(100);
                entity.Property(e => e.UKRegistrationDate).HasMaxLength(50);
                entity.Property(e => e.GDCRegistrationGaps).HasMaxLength(500);
                entity.Property(e => e.Qualification1).HasMaxLength(500);
                entity.Property(e => e.Qualification1Country).HasMaxLength(100);
                entity.Property(e => e.Qualification1Institution).HasMaxLength(300);
                entity.Property(e => e.Qualification1Year).HasMaxLength(10);
                
                entity.Property(e => e.Qualification2).HasMaxLength(500);
                entity.Property(e => e.Qualification2Country).HasMaxLength(100);
                entity.Property(e => e.Qualification2Institution).HasMaxLength(300);
                entity.Property(e => e.Qualification2Year).HasMaxLength(10);
                
                entity.Property(e => e.Qualification3).HasMaxLength(500);
                entity.Property(e => e.Qualification3Country).HasMaxLength(100);
                entity.Property(e => e.Qualification3Institution).HasMaxLength(300);
                entity.Property(e => e.Qualification3Year).HasMaxLength(10);
                
                entity.Property(e => e.Qualification4).HasMaxLength(500);
                entity.Property(e => e.Qualification4Country).HasMaxLength(100);
                entity.Property(e => e.Qualification4Institution).HasMaxLength(300);
                entity.Property(e => e.Qualification4Year).HasMaxLength(10);
                
                entity.Property(e => e.Qualification5).HasMaxLength(500);
                entity.Property(e => e.Qualification5Country).HasMaxLength(100);
                entity.Property(e => e.Qualification5Institution).HasMaxLength(300);
                entity.Property(e => e.Qualification5Year).HasMaxLength(10);
                
                entity.Property(e => e.Qualification6).HasMaxLength(500);
                entity.Property(e => e.Qualification6Country).HasMaxLength(100);
                entity.Property(e => e.Qualification6Institution).HasMaxLength(300);
                entity.Property(e => e.Qualification6Year).HasMaxLength(10);
                
                // Configure all employment fields as nullable
                entity.Property(e => e.Job1FromDate).HasMaxLength(20);
                entity.Property(e => e.Job1ToDate).HasMaxLength(20);
                entity.Property(e => e.Job1Title).HasMaxLength(200);
                entity.Property(e => e.Job1Address).HasColumnType("text");
                entity.Property(e => e.Job1Type).HasMaxLength(50);
                
                entity.Property(e => e.Job2FromDate).HasMaxLength(20);
                entity.Property(e => e.Job2ToDate).HasMaxLength(20);
                entity.Property(e => e.Job2Title).HasMaxLength(200);
                entity.Property(e => e.Job2Address).HasColumnType("text");
                entity.Property(e => e.Job2Type).HasMaxLength(50);
                
                entity.Property(e => e.Job3FromDate).HasMaxLength(20);
                entity.Property(e => e.Job3ToDate).HasMaxLength(20);
                entity.Property(e => e.Job3Title).HasMaxLength(200);
                entity.Property(e => e.Job3Address).HasColumnType("text");
                entity.Property(e => e.Job3Type).HasMaxLength(50);
                
                entity.Property(e => e.Job4FromDate).HasMaxLength(20);
                entity.Property(e => e.Job4ToDate).HasMaxLength(20);
                entity.Property(e => e.Job4Title).HasMaxLength(200);
                entity.Property(e => e.Job4Address).HasColumnType("text");
                entity.Property(e => e.Job4Type).HasMaxLength(50);
                
                entity.Property(e => e.Job5FromDate).HasMaxLength(20);
                entity.Property(e => e.Job5ToDate).HasMaxLength(20);
                entity.Property(e => e.Job5Title).HasMaxLength(200);
                entity.Property(e => e.Job5Address).HasColumnType("text");
                entity.Property(e => e.Job5Type).HasMaxLength(50);
                
                entity.Property(e => e.Job6FromDate).HasMaxLength(20);
                entity.Property(e => e.Job6ToDate).HasMaxLength(20);
                entity.Property(e => e.Job6Title).HasMaxLength(200);
                entity.Property(e => e.Job6Address).HasColumnType("text");
                entity.Property(e => e.Job6Type).HasMaxLength(50);
            });
        }
    }
}
