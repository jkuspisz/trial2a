using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleGateway.Migrations
{
    /// <inheritdoc />
    public partial class Update_TestData2_AddRegistrationAndEmploymentFields_20250728104254 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GDCRegistrationGaps",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GDCRegistrationNumber",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MostRecentJobAddress",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MostRecentJobAdultPatients",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MostRecentJobChildPatients",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MostRecentJobFromDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MostRecentJobTitle",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MostRecentJobToDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NHSExperience",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NHSExperienceDaysPerWeek",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NHSExperienceMonths",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NHSExperienceType",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NHSExperienceYears",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryQualification",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryQualificationCountry",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryQualificationInstitution",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryQualificationYear",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UKRegistrationDate",
                table: "TestData2",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GDCRegistrationGaps",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "GDCRegistrationNumber",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "MostRecentJobAddress",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "MostRecentJobAdultPatients",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "MostRecentJobChildPatients",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "MostRecentJobFromDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "MostRecentJobTitle",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "MostRecentJobToDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "NHSExperience",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "NHSExperienceDaysPerWeek",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "NHSExperienceMonths",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "NHSExperienceType",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "NHSExperienceYears",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "PrimaryQualification",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "PrimaryQualificationCountry",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "PrimaryQualificationInstitution",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "PrimaryQualificationYear",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "UKRegistrationDate",
                table: "TestData2");
        }
    }
}
