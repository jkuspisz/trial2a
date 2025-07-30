using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SimpleGateway.Migrations
{
    /// <inheritdoc />
    public partial class AddMSFQuestionnaires : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "WorkBasedAssessments",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "WorkBasedAssessments",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "WorkBasedAssessments",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CompletedBySupervisor",
                table: "WorkBasedAssessments",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AssessmentType",
                table: "WorkBasedAssessments",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "AgreementTerms",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "MSFQuestionnaires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PerformerUsername = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    UniqueIdentifier = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MSFQuestionnaires", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MSFResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MSFQuestionnaireId = table.Column<int>(type: "integer", nullable: false),
                    RespondentIdentifier = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TreatPatientsWithCompassion = table.Column<int>(type: "integer", nullable: false),
                    EnableInformedDecisions = table.Column<int>(type: "integer", nullable: false),
                    RecogniseCommNeedsPatients = table.Column<int>(type: "integer", nullable: false),
                    ProduceClearCommunications = table.Column<int>(type: "integer", nullable: false),
                    DemonstrateIntegrity = table.Column<int>(type: "integer", nullable: false),
                    WorkWithinScope = table.Column<int>(type: "integer", nullable: false),
                    EngageWithDevelopment = table.Column<int>(type: "integer", nullable: false),
                    KeepPracticeUpToDate = table.Column<int>(type: "integer", nullable: false),
                    FacilitateLearning = table.Column<int>(type: "integer", nullable: false),
                    InteractWithColleagues = table.Column<int>(type: "integer", nullable: false),
                    PromoteEqualityDiversity = table.Column<int>(type: "integer", nullable: false),
                    RecogniseImpactOnOthers = table.Column<int>(type: "integer", nullable: false),
                    ManageTimeResources = table.Column<int>(type: "integer", nullable: false),
                    WorkAsTeamMember = table.Column<int>(type: "integer", nullable: false),
                    WorkToStandards = table.Column<int>(type: "integer", nullable: false),
                    ParticipateInImprovement = table.Column<int>(type: "integer", nullable: false),
                    MinimiseWasteEnvironment = table.Column<int>(type: "integer", nullable: false),
                    DoesParticularlyWell = table.Column<string>(type: "text", nullable: false),
                    CouldImproveUpon = table.Column<string>(type: "text", nullable: false),
                    RelationshipToPerformer = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    WorkingRelationshipDuration = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MSFResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MSFResponses_MSFQuestionnaires_MSFQuestionnaireId",
                        column: x => x.MSFQuestionnaireId,
                        principalTable: "MSFQuestionnaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MSFQuestionnaires_UniqueIdentifier",
                table: "MSFQuestionnaires",
                column: "UniqueIdentifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MSFResponses_MSFQuestionnaireId",
                table: "MSFResponses",
                column: "MSFQuestionnaireId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MSFResponses");

            migrationBuilder.DropTable(
                name: "MSFQuestionnaires");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "WorkBasedAssessments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "WorkBasedAssessments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "WorkBasedAssessments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CompletedBySupervisor",
                table: "WorkBasedAssessments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AssessmentType",
                table: "WorkBasedAssessments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "AgreementTerms",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);
        }
    }
}
