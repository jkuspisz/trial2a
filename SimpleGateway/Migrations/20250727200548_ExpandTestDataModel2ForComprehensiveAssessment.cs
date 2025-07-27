using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleGateway.Migrations
{
    /// <inheritdoc />
    public partial class ExpandTestDataModel2ForComprehensiveAssessment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmalgamFillingsCount",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "BPEConfidence",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "BPEExaminationsCount",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "BPELastDate",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "BPETraining",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "ClinicalAssessmentConfidence",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "ClinicalAssessmentLastDate",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "ClinicalAssessmentTraining",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "ClinicalAssessmentsCount",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "DeciduousTeethConfidence",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "DeciduousTeethCount",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "DeciduousTeethLastDate",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "DeciduousTeethTraining",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "DenturesConfidence",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "DenturesCount",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "DenturesLastDate",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "DenturesTraining",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "GDCGapsExplanation",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "GDCRegistrationNumber",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "MechanicalDebridementConfidence",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "MechanicalDebridementCount",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "MechanicalDebridementLastDate",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "MechanicalDebridementTraining",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "NHSWorkDaysPerWeek",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "NHSWorkFullTime",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "NHSWorkMonths",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "NHSWorkPartTime",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "NHSWorkYears",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "PocketChartsConfidence",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "PocketChartsCount",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "PocketChartsLastDate",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "PocketChartsTraining",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "PrimaryQualification",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "PrimaryQualificationCountry",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "PrimaryQualificationInstitution",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "PrimaryQualificationYear",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "RadiographsConfidence",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "RadiographsCount",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "RadiographsLastDate",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "RadiographsTraining",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "RootDivisionConfidence",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "RootDivisionExtractionsCount",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "RootDivisionLastDate",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "RootDivisionTraining",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "SimpleExtractionsConfidence",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "SimpleExtractionsCount",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "SimpleExtractionsLastDate",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "SimpleExtractionsTraining",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "UKRegistrationDate",
                table: "TestData");

            migrationBuilder.DropColumn(
                name: "WorkedInNHS",
                table: "TestData");

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
                nullable: true);

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
                nullable: true);

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
                nullable: true);

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
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GDCGapsExplanation",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GDCRegistrationNumber",
                table: "TestData2",
                type: "text",
                nullable: true);

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
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NHSWorkDaysPerWeek",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NHSWorkFullTime",
                table: "TestData2",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NHSWorkMonths",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NHSWorkPartTime",
                table: "TestData2",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NHSWorkYears",
                table: "TestData2",
                type: "integer",
                nullable: true);

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

            migrationBuilder.AddColumn<string>(
                name: "PocketChartsLastDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PocketChartsTraining",
                table: "TestData2",
                type: "boolean",
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

            migrationBuilder.AddColumn<string>(
                name: "RadiographsLastDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RadiographsTraining",
                table: "TestData2",
                type: "boolean",
                nullable: true);

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

            migrationBuilder.AddColumn<string>(
                name: "RootDivisionLastDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RootDivisionTraining",
                table: "TestData2",
                type: "boolean",
                nullable: true);

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

            migrationBuilder.AddColumn<string>(
                name: "SimpleExtractionsLastDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SimpleExtractionsTraining",
                table: "TestData2",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UKRegistrationDate",
                table: "TestData2",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkedInNHS",
                table: "TestData2",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "GDCGapsExplanation",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "GDCRegistrationNumber",
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
                name: "NHSWorkDaysPerWeek",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "NHSWorkFullTime",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "NHSWorkMonths",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "NHSWorkPartTime",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "NHSWorkYears",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "PocketChartsConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "PocketChartsCount",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "PocketChartsLastDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "PocketChartsTraining",
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
                name: "RadiographsConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "RadiographsCount",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "RadiographsLastDate",
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
                name: "RootDivisionLastDate",
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
                name: "SimpleExtractionsLastDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "SimpleExtractionsTraining",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "UKRegistrationDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "WorkedInNHS",
                table: "TestData2");

            migrationBuilder.AddColumn<int>(
                name: "AmalgamFillingsCount",
                table: "TestData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BPEConfidence",
                table: "TestData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BPEExaminationsCount",
                table: "TestData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BPELastDate",
                table: "TestData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BPETraining",
                table: "TestData",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClinicalAssessmentConfidence",
                table: "TestData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClinicalAssessmentLastDate",
                table: "TestData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ClinicalAssessmentTraining",
                table: "TestData",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClinicalAssessmentsCount",
                table: "TestData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeciduousTeethConfidence",
                table: "TestData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeciduousTeethCount",
                table: "TestData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeciduousTeethLastDate",
                table: "TestData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DeciduousTeethTraining",
                table: "TestData",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DenturesConfidence",
                table: "TestData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DenturesCount",
                table: "TestData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DenturesLastDate",
                table: "TestData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DenturesTraining",
                table: "TestData",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GDCGapsExplanation",
                table: "TestData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GDCRegistrationNumber",
                table: "TestData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MechanicalDebridementConfidence",
                table: "TestData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MechanicalDebridementCount",
                table: "TestData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MechanicalDebridementLastDate",
                table: "TestData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MechanicalDebridementTraining",
                table: "TestData",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NHSWorkDaysPerWeek",
                table: "TestData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NHSWorkFullTime",
                table: "TestData",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NHSWorkMonths",
                table: "TestData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NHSWorkPartTime",
                table: "TestData",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NHSWorkYears",
                table: "TestData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PocketChartsConfidence",
                table: "TestData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PocketChartsCount",
                table: "TestData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PocketChartsLastDate",
                table: "TestData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PocketChartsTraining",
                table: "TestData",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryQualification",
                table: "TestData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryQualificationCountry",
                table: "TestData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryQualificationInstitution",
                table: "TestData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryQualificationYear",
                table: "TestData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RadiographsConfidence",
                table: "TestData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RadiographsCount",
                table: "TestData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RadiographsLastDate",
                table: "TestData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RadiographsTraining",
                table: "TestData",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RootDivisionConfidence",
                table: "TestData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RootDivisionExtractionsCount",
                table: "TestData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RootDivisionLastDate",
                table: "TestData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RootDivisionTraining",
                table: "TestData",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SimpleExtractionsConfidence",
                table: "TestData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SimpleExtractionsCount",
                table: "TestData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SimpleExtractionsLastDate",
                table: "TestData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SimpleExtractionsTraining",
                table: "TestData",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UKRegistrationDate",
                table: "TestData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkedInNHS",
                table: "TestData",
                type: "text",
                nullable: true);
        }
    }
}
