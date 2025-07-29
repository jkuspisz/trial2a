using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleGateway.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWorkBasedAssessmentsToSimplifiedSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionPlan",
                table: "WorkBasedAssessments");

            migrationBuilder.DropColumn(
                name: "AreasForDevelopment",
                table: "WorkBasedAssessments");

            migrationBuilder.DropColumn(
                name: "ClinicalArea",
                table: "WorkBasedAssessments");

            migrationBuilder.DropColumn(
                name: "LearningObjectives",
                table: "WorkBasedAssessments");

            migrationBuilder.DropColumn(
                name: "OverallRating",
                table: "WorkBasedAssessments");

            migrationBuilder.DropColumn(
                name: "PerformerComments",
                table: "WorkBasedAssessments");

            migrationBuilder.DropColumn(
                name: "ProcedureDetails",
                table: "WorkBasedAssessments");

            migrationBuilder.DropColumn(
                name: "Recommendations",
                table: "WorkBasedAssessments");

            migrationBuilder.RenameColumn(
                name: "SupervisorRole",
                table: "WorkBasedAssessments",
                newName: "SupervisorActionPlan");

            migrationBuilder.RenameColumn(
                name: "SupervisorName",
                table: "WorkBasedAssessments",
                newName: "ProcedureDescription");

            migrationBuilder.RenameColumn(
                name: "SupervisorComments",
                table: "WorkBasedAssessments",
                newName: "LearningReflection");

            migrationBuilder.RenameColumn(
                name: "SkillsAssessment",
                table: "WorkBasedAssessments",
                newName: "LearningNeeds");

            migrationBuilder.AddColumn<bool>(
                name: "OverallAcceptable",
                table: "WorkBasedAssessments",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OverallAcceptable",
                table: "WorkBasedAssessments");

            migrationBuilder.RenameColumn(
                name: "SupervisorActionPlan",
                table: "WorkBasedAssessments",
                newName: "SupervisorRole");

            migrationBuilder.RenameColumn(
                name: "ProcedureDescription",
                table: "WorkBasedAssessments",
                newName: "SupervisorName");

            migrationBuilder.RenameColumn(
                name: "LearningReflection",
                table: "WorkBasedAssessments",
                newName: "SupervisorComments");

            migrationBuilder.RenameColumn(
                name: "LearningNeeds",
                table: "WorkBasedAssessments",
                newName: "SkillsAssessment");

            migrationBuilder.AddColumn<string>(
                name: "ActionPlan",
                table: "WorkBasedAssessments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AreasForDevelopment",
                table: "WorkBasedAssessments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClinicalArea",
                table: "WorkBasedAssessments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LearningObjectives",
                table: "WorkBasedAssessments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OverallRating",
                table: "WorkBasedAssessments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PerformerComments",
                table: "WorkBasedAssessments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProcedureDetails",
                table: "WorkBasedAssessments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Recommendations",
                table: "WorkBasedAssessments",
                type: "text",
                nullable: true);
        }
    }
}
