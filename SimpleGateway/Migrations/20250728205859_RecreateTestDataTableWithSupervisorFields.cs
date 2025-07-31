using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleGateway.Migrations
{
    /// <inheritdoc />
    public partial class RecreateTestDataTableWithSupervisorFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add supervisor fields to TestData table
            migrationBuilder.AddColumn<string>(
                name: "GDCNumber",
                table: "TestData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YearsOnPerformersList",
                table: "TestData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrainingCoursesAttended",
                table: "TestData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentSupervisionExperience",
                table: "TestData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentConditionsRestrictions",
                table: "TestData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CPDCompliance",
                table: "TestData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DeclarationSigned",
                table: "TestData",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeclarationSignedDate",
                table: "TestData",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeclarationSignedBy",
                table: "TestData",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove supervisor fields from TestData table
            migrationBuilder.DropColumn(
                name: "GDCNumber",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "YearsOnPerformersList",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "TrainingCoursesAttended",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "CurrentSupervisionExperience",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "CurrentConditionsRestrictions",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "CPDCompliance",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "DeclarationSigned",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "DeclarationSignedDate",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "DeclarationSignedBy",
                table: "TestData");
        }
    }
}
