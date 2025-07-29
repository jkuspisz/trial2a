using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SimpleGateway.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkBasedAssessmentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkBasedAssessments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    AssessmentType = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    AssessmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ClinicalArea = table.Column<string>(type: "text", nullable: true),
                    ProcedureDetails = table.Column<string>(type: "text", nullable: true),
                    LearningObjectives = table.Column<string>(type: "text", nullable: true),
                    PerformerComments = table.Column<string>(type: "text", nullable: true),
                    AreasForDevelopment = table.Column<string>(type: "text", nullable: true),
                    IsPerformerSubmitted = table.Column<bool>(type: "boolean", nullable: false),
                    PerformerSubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SupervisorName = table.Column<string>(type: "text", nullable: true),
                    SupervisorRole = table.Column<string>(type: "text", nullable: true),
                    OverallRating = table.Column<string>(type: "text", nullable: true),
                    SkillsAssessment = table.Column<string>(type: "text", nullable: true),
                    SupervisorComments = table.Column<string>(type: "text", nullable: true),
                    Recommendations = table.Column<string>(type: "text", nullable: true),
                    ActionPlan = table.Column<string>(type: "text", nullable: true),
                    IsSupervisorCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompletedBySupervisor = table.Column<string>(type: "text", nullable: true),
                    SupervisorCompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkBasedAssessments", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkBasedAssessments");
        }
    }
}
