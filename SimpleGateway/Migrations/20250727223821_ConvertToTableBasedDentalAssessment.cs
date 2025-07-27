using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleGateway.Migrations
{
    /// <inheritdoc />
    public partial class ConvertToTableBasedDentalAssessment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfidenceLevelsText",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "GDCGapsExplanation",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "GDCRegistrationNumber",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "LastProcedureDatesText",
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
                name: "TrainingNeedsText",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "UKRegistrationDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "WorkedInNHS",
                table: "TestData2");

            migrationBuilder.RenameColumn(
                name: "NHSWorkYears",
                table: "TestData2",
                newName: "SixPPCNumber");

            migrationBuilder.RenameColumn(
                name: "NHSWorkPartTime",
                table: "TestData2",
                newName: "SixPPCNeedSupport");

            migrationBuilder.RenameColumn(
                name: "NHSWorkMonths",
                table: "TestData2",
                newName: "SixPPCConfidence");

            migrationBuilder.RenameColumn(
                name: "NHSWorkFullTime",
                table: "TestData2",
                newName: "SingleEndoNeedSupport");

            migrationBuilder.RenameColumn(
                name: "NHSWorkDaysPerWeek",
                table: "TestData2",
                newName: "SingleEndoNumber");

            migrationBuilder.AddColumn<int>(
                name: "AmalgamConfidence",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AmalgamLastDate",
                table: "TestData2",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AmalgamNeedSupport",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "AmalgamNumber",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BPEConfidence",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BPELastDate",
                table: "TestData2",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BPENeedSupport",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "BPENumber",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClinicalExamConfidence",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClinicalExamLastDate",
                table: "TestData2",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ClinicalExamNeedSupport",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ClinicalExamNumber",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompositeConfidence",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompositeLastDate",
                table: "TestData2",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CompositeNeedSupport",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CompositeNumber",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CrownsConfidence",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CrownsLastDate",
                table: "TestData2",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CrownsNeedSupport",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CrownsNumber",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DenturesConfidence",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DenturesLastDate",
                table: "TestData2",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DenturesNeedSupport",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DenturesNumber",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtractionsConfidence",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtractionsLastDate",
                table: "TestData2",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ExtractionsNeedSupport",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ExtractionsNumber",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MultiEndoConfidence",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MultiEndoLastDate",
                table: "TestData2",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MultiEndoNeedSupport",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MultiEndoNumber",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PeadsConfidence",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PeadsLastDate",
                table: "TestData2",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PeadsNeedSupport",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PeadsNumber",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RadiographsConfidence",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RadiographsLastDate",
                table: "TestData2",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RadiographsNeedSupport",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RadiographsNumber",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RootDivisionConfidence",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RootDivisionLastDate",
                table: "TestData2",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RootDivisionNeedSupport",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RootDivisionNumber",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScalingConfidence",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScalingLastDate",
                table: "TestData2",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ScalingNeedSupport",
                table: "TestData2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ScalingNumber",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SingleEndoConfidence",
                table: "TestData2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SingleEndoLastDate",
                table: "TestData2",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SixPPCLastDate",
                table: "TestData2",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmalgamConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "AmalgamLastDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "AmalgamNeedSupport",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "AmalgamNumber",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "BPEConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "BPELastDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "BPENeedSupport",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "BPENumber",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "ClinicalExamConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "ClinicalExamLastDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "ClinicalExamNeedSupport",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "ClinicalExamNumber",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "CompositeConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "CompositeLastDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "CompositeNeedSupport",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "CompositeNumber",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "CrownsConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "CrownsLastDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "CrownsNeedSupport",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "CrownsNumber",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "DenturesConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "DenturesLastDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "DenturesNeedSupport",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "DenturesNumber",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "ExtractionsConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "ExtractionsLastDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "ExtractionsNeedSupport",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "ExtractionsNumber",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "MultiEndoConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "MultiEndoLastDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "MultiEndoNeedSupport",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "MultiEndoNumber",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "PeadsConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "PeadsLastDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "PeadsNeedSupport",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "PeadsNumber",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "RadiographsConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "RadiographsLastDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "RadiographsNeedSupport",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "RadiographsNumber",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "RootDivisionConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "RootDivisionLastDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "RootDivisionNeedSupport",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "RootDivisionNumber",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "ScalingConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "ScalingLastDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "ScalingNeedSupport",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "ScalingNumber",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "SingleEndoConfidence",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "SingleEndoLastDate",
                table: "TestData2");

            migrationBuilder.DropColumn(
                name: "SixPPCLastDate",
                table: "TestData2");

            migrationBuilder.RenameColumn(
                name: "SixPPCNumber",
                table: "TestData2",
                newName: "NHSWorkYears");

            migrationBuilder.RenameColumn(
                name: "SixPPCNeedSupport",
                table: "TestData2",
                newName: "NHSWorkPartTime");

            migrationBuilder.RenameColumn(
                name: "SixPPCConfidence",
                table: "TestData2",
                newName: "NHSWorkMonths");

            migrationBuilder.RenameColumn(
                name: "SingleEndoNumber",
                table: "TestData2",
                newName: "NHSWorkDaysPerWeek");

            migrationBuilder.RenameColumn(
                name: "SingleEndoNeedSupport",
                table: "TestData2",
                newName: "NHSWorkFullTime");

            migrationBuilder.AddColumn<string>(
                name: "ConfidenceLevelsText",
                table: "TestData2",
                type: "text",
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

            migrationBuilder.AddColumn<string>(
                name: "LastProcedureDatesText",
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
                name: "TrainingNeedsText",
                table: "TestData2",
                type: "text",
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
    }
}
