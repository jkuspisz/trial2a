using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleGateway.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyTestDataModel2ToTextFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmalgamFillingsCount",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "BPEConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "BPEExaminationsCount",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "BPELastDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "BPETraining",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "ClinicalAssessmentConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "ClinicalAssessmentLastDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "ClinicalAssessmentTraining",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "ClinicalAssessmentsCount",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "DeciduousTeethConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "DeciduousTeethCount",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "DeciduousTeethLastDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "DeciduousTeethTraining",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "DenturesConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "DenturesCount",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "DenturesLastDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "DenturesTraining",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "MechanicalDebridementConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "MechanicalDebridementCount",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "MechanicalDebridementLastDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "MechanicalDebridementTraining",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "PocketChartsConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "PocketChartsCount",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "PocketChartsTraining",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "RadiographsConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "RadiographsCount",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "RadiographsTraining",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "RootDivisionConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "RootDivisionExtractionsCount",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "RootDivisionTraining",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "SimpleExtractionsConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "SimpleExtractionsCount",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "SimpleExtractionsTraining",
                table: "TestData2");

            migrationBuilder.RenameColumn(
                name: "SimpleExtractionsLastDate",
                table: "TestData2",
                newName: "TrainingNeedsText");

            migrationBuilder.RenameColumn(
                name: "RootDivisionLastDate",
                table: "TestData2",
                newName: "LastProcedureDatesText");

            migrationBuilder.RenameColumn(
                name: "RadiographsLastDate",
                table: "TestData2",
                newName: "ConfidenceLevelsText");

            migrationBuilder.RenameColumn(
                name: "PocketChartsLastDate",
                table: "TestData2",
                newName: "ClinicalExperienceText");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrainingNeedsText",
                table: "TestData2",
                newName: "SimpleExtractionsLastDate");

            migrationBuilder.RenameColumn(
                name: "LastProcedureDatesText",
                table: "TestData2",
                newName: "RootDivisionLastDate");

            migrationBuilder.RenameColumn(
                name: "ConfidenceLevelsText",
                table: "TestData2",
                newName: "RadiographsLastDate");

            migrationBuilder.RenameColumn(
                name: "ClinicalExperienceText",
                table: "TestData2",
                newName: "PocketChartsLastDate");

            migrationBuilder.AddColumn<int>(
                name: "AmalgamFillingsCount",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BPEConfidence",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BPEExaminationsCount",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BPELastDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BPETraining",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ClinicalAssessmentConfidence",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClinicalAssessmentLastDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ClinicalAssessmentTraining",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ClinicalAssessmentsCount",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeciduousTeethConfidence",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeciduousTeethCount",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeciduousTeethLastDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DeciduousTeethTraining",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DenturesConfidence",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DenturesCount",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DenturesLastDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DenturesTraining",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MechanicalDebridementConfidence",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MechanicalDebridementCount",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MechanicalDebridementLastDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MechanicalDebridementTraining",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PocketChartsConfidence",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PocketChartsCount",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PocketChartsTraining",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RadiographsConfidence",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RadiographsCount",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RadiographsTraining",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RootDivisionConfidence",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RootDivisionExtractionsCount",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RootDivisionTraining",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SimpleExtractionsConfidence",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SimpleExtractionsCount",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SimpleExtractionsTraining",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
