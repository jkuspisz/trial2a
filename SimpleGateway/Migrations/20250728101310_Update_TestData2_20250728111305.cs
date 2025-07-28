using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleGateway.Migrations
{
    /// <inheritdoc />
    public partial class Update_TestData2_20250728111305 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PrimaryQualificationYear",
                table: "TestData2",
                newName: "Qualification6Year");

            migrationBuilder.RenameColumn(
                name: "PrimaryQualificationInstitution",
                table: "TestData2",
                newName: "Qualification6Institution");

            migrationBuilder.RenameColumn(
                name: "PrimaryQualificationCountry",
                table: "TestData2",
                newName: "Qualification6Country");

            migrationBuilder.RenameColumn(
                name: "PrimaryQualification",
                table: "TestData2",
                newName: "Qualification6");

            migrationBuilder.RenameColumn(
                name: "NHSExperienceYears",
                table: "TestData2",
                newName: "Qualification5Year");

            migrationBuilder.RenameColumn(
                name: "NHSExperienceType",
                table: "TestData2",
                newName: "Qualification5Institution");

            migrationBuilder.RenameColumn(
                name: "NHSExperienceMonths",
                table: "TestData2",
                newName: "Qualification5Country");

            migrationBuilder.RenameColumn(
                name: "NHSExperienceDaysPerWeek",
                table: "TestData2",
                newName: "Qualification5");

            migrationBuilder.RenameColumn(
                name: "NHSExperience",
                table: "TestData2",
                newName: "Qualification4Year");

            migrationBuilder.RenameColumn(
                name: "MostRecentJobToDate",
                table: "TestData2",
                newName: "Qualification4Institution");

            migrationBuilder.RenameColumn(
                name: "MostRecentJobTitle",
                table: "TestData2",
                newName: "Qualification4Country");

            migrationBuilder.RenameColumn(
                name: "MostRecentJobFromDate",
                table: "TestData2",
                newName: "Qualification4");

            migrationBuilder.RenameColumn(
                name: "MostRecentJobChildPatients",
                table: "TestData2",
                newName: "Job6ChildPatients");

            migrationBuilder.RenameColumn(
                name: "MostRecentJobAdultPatients",
                table: "TestData2",
                newName: "Job6AdultPatients");

            migrationBuilder.RenameColumn(
                name: "MostRecentJobAddress",
                table: "TestData2",
                newName: "Qualification3Year");

            migrationBuilder.AddColumn<string>(
                name: "Job1Address",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Job1AdultPatients",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Job1ChildPatients",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Job1FromDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job1Title",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job1ToDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job1Type",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job2Address",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Job2AdultPatients",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Job2ChildPatients",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Job2FromDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job2Title",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job2ToDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job2Type",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job3Address",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Job3AdultPatients",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Job3ChildPatients",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Job3FromDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job3Title",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job3ToDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job3Type",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job4Address",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Job4AdultPatients",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Job4ChildPatients",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Job4FromDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job4Title",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job4ToDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job4Type",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job5Address",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Job5AdultPatients",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Job5ChildPatients",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Job5FromDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job5Title",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job5ToDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job5Type",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job6Address",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job6FromDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job6Title",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job6ToDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job6Type",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Qualification1",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Qualification1Country",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Qualification1Institution",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Qualification1Year",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Qualification2",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Qualification2Country",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Qualification2Institution",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Qualification2Year",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Qualification3",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Qualification3Country",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Qualification3Institution",
                table: "TestData2",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Job1Address",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job1AdultPatients",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job1ChildPatients",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job1FromDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job1Title",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job1ToDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job1Type",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job2Address",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job2AdultPatients",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job2ChildPatients",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job2FromDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job2Title",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job2ToDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job2Type",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job3Address",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job3AdultPatients",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job3ChildPatients",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job3FromDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job3Title",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job3ToDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job3Type",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job4Address",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job4AdultPatients",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job4ChildPatients",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job4FromDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job4Title",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job4ToDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job4Type",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job5Address",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job5AdultPatients",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job5ChildPatients",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job5FromDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job5Title",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job5ToDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job5Type",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job6Address",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job6FromDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job6Title",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job6ToDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Job6Type",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Qualification1",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Qualification1Country",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Qualification1Institution",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Qualification1Year",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Qualification2",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Qualification2Country",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Qualification2Institution",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Qualification2Year",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Qualification3",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Qualification3Country",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "Qualification3Institution",
                table: "TestData2");

            migrationBuilder.RenameColumn(
                name: "Qualification6Year",
                table: "TestData2",
                newName: "PrimaryQualificationYear");

            migrationBuilder.RenameColumn(
                name: "Qualification6Institution",
                table: "TestData2",
                newName: "PrimaryQualificationInstitution");

            migrationBuilder.RenameColumn(
                name: "Qualification6Country",
                table: "TestData2",
                newName: "PrimaryQualificationCountry");

            migrationBuilder.RenameColumn(
                name: "Qualification6",
                table: "TestData2",
                newName: "PrimaryQualification");

            migrationBuilder.RenameColumn(
                name: "Qualification5Year",
                table: "TestData2",
                newName: "NHSExperienceYears");

            migrationBuilder.RenameColumn(
                name: "Qualification5Institution",
                table: "TestData2",
                newName: "NHSExperienceType");

            migrationBuilder.RenameColumn(
                name: "Qualification5Country",
                table: "TestData2",
                newName: "NHSExperienceMonths");

            migrationBuilder.RenameColumn(
                name: "Qualification5",
                table: "TestData2",
                newName: "NHSExperienceDaysPerWeek");

            migrationBuilder.RenameColumn(
                name: "Qualification4Year",
                table: "TestData2",
                newName: "NHSExperience");

            migrationBuilder.RenameColumn(
                name: "Qualification4Institution",
                table: "TestData2",
                newName: "MostRecentJobToDate");

            migrationBuilder.RenameColumn(
                name: "Qualification4Country",
                table: "TestData2",
                newName: "MostRecentJobTitle");

            migrationBuilder.RenameColumn(
                name: "Qualification4",
                table: "TestData2",
                newName: "MostRecentJobFromDate");

            migrationBuilder.RenameColumn(
                name: "Qualification3Year",
                table: "TestData2",
                newName: "MostRecentJobAddress");

            migrationBuilder.RenameColumn(
                name: "Job6ChildPatients",
                table: "TestData2",
                newName: "MostRecentJobChildPatients");

            migrationBuilder.RenameColumn(
                name: "Job6AdultPatients",
                table: "TestData2",
                newName: "MostRecentJobAdultPatients");
        }
    }
}
