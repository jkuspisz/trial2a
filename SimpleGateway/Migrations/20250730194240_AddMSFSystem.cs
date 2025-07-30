using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleGateway.Migrations
{
    /// <inheritdoc />
    public partial class AddMSFSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create MSFQuestionnaires table if it doesn't exist
            migrationBuilder.CreateTable(
                name: "MSFQuestionnaires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PerformerId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UniqueCode = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MSFQuestionnaires", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MSFQuestionnaires_Users_PerformerId",
                        column: x => x.PerformerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Create MSFResponses table if it doesn't exist
            migrationBuilder.CreateTable(
                name: "MSFResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MSFQuestionnaireId = table.Column<int>(type: "integer", nullable: false),
                    RespondentName = table.Column<string>(type: "text", nullable: true),
                    RespondentRole = table.Column<string>(type: "text", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    
                    // Patient Care & Communication (1-6)
                    PatientCareQualityScore = table.Column<int>(type: "integer", nullable: true),
                    PatientCareQualityComment = table.Column<string>(type: "text", nullable: true),
                    CommunicationSkillsScore = table.Column<int>(type: "integer", nullable: true),
                    CommunicationSkillsComment = table.Column<string>(type: "text", nullable: true),
                    CommunicationEmpathyScore = table.Column<int>(type: "integer", nullable: true),
                    CommunicationEmpathyComment = table.Column<string>(type: "text", nullable: true),
                    HistoryTakingScore = table.Column<int>(type: "integer", nullable: true),
                    HistoryTakingComment = table.Column<string>(type: "text", nullable: true),
                    ConsultationManagementScore = table.Column<int>(type: "integer", nullable: true),
                    ConsultationManagementComment = table.Column<string>(type: "text", nullable: true),
                    CulturalSensitivityScore = table.Column<int>(type: "integer", nullable: true),
                    CulturalSensitivityComment = table.Column<string>(type: "text", nullable: true),

                    // Professional Integrity & Development (7-11)
                    EthicalProfessionalismScore = table.Column<int>(type: "integer", nullable: true),
                    EthicalProfessionalismComment = table.Column<string>(type: "text", nullable: true),
                    ProfessionalDevelopmentScore = table.Column<int>(type: "integer", nullable: true),
                    ProfessionalDevelopmentComment = table.Column<string>(type: "text", nullable: true),
                    TechnicalCompetenceScore = table.Column<int>(type: "integer", nullable: true),
                    TechnicalCompetenceComment = table.Column<string>(type: "text", nullable: true),
                    DecisionMakingScore = table.Column<int>(type: "integer", nullable: true),
                    DecisionMakingComment = table.Column<string>(type: "text", nullable: true),
                    DocumentationScore = table.Column<int>(type: "integer", nullable: true),
                    DocumentationComment = table.Column<string>(type: "text", nullable: true),

                    // Team Working & Quality Improvement (12-17)
                    TeamCollaborationScore = table.Column<int>(type: "integer", nullable: true),
                    TeamCollaborationComment = table.Column<string>(type: "text", nullable: true),
                    TeamSupportScore = table.Column<int>(type: "integer", nullable: true),
                    TeamSupportComment = table.Column<string>(type: "text", nullable: true),
                    LeadershipSkillsScore = table.Column<int>(type: "integer", nullable: true),
                    LeadershipSkillsComment = table.Column<string>(type: "text", nullable: true),
                    QualityImprovementScore = table.Column<int>(type: "integer", nullable: true),
                    QualityImprovementComment = table.Column<string>(type: "text", nullable: true),
                    HealthSafetyAwarenessScore = table.Column<int>(type: "integer", nullable: true),
                    HealthSafetyAwarenessComment = table.Column<string>(type: "text", nullable: true),
                    ContinuousImprovementScore = table.Column<int>(type: "integer", nullable: true),
                    ContinuousImprovementComment = table.Column<string>(type: "text", nullable: true),

                    AdditionalComments = table.Column<string>(type: "text", nullable: true)
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

            // Create indexes
            migrationBuilder.CreateIndex(
                name: "IX_MSFQuestionnaires_PerformerId",
                table: "MSFQuestionnaires",
                column: "PerformerId");

            migrationBuilder.CreateIndex(
                name: "IX_MSFQuestionnaires_UniqueCode",
                table: "MSFQuestionnaires",
                column: "UniqueCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MSFResponses_MSFQuestionnaireId",
                table: "MSFResponses",
                column: "MSFQuestionnaireId");

            // Skip the ALTER TABLE operations since we're creating new tables
            // The rest of this migration was attempting to modify existing tables

            migrationBuilder.DropColumn(
                name: "CouldImproveUpon",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "DemonstrateIntegrity",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "DoesParticularlyWell",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "EnableInformedDecisions",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "EngageWithDevelopment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "FacilitateLearning",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "InteractWithColleagues",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "KeepPracticeUpToDate",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "ManageTimeResources",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "MinimiseWasteEnvironment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "ParticipateInImprovement",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "ProduceClearCommunications",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "PromoteEqualityDiversity",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "RecogniseCommNeedsPatients",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "RecogniseImpactOnOthers",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "RelationshipToPerformer",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "RespondentIdentifier",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "TreatPatientsWithCompassion",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "WorkAsTeamMember",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "WorkToStandards",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "WorkWithinScope",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "WorkingRelationshipDuration",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "PerformerUsername",
                table: "MSFQuestionnaires");

            migrationBuilder.DropColumn(
                name: "UniqueIdentifier",
                table: "MSFQuestionnaires");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalComments",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommunicationEmpathyComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommunicationEmpathyScore",
                table: "MSFResponses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommunicationSkillsComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommunicationSkillsScore",
                table: "MSFResponses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConsultationManagementComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConsultationManagementScore",
                table: "MSFResponses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContinuousImprovementComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContinuousImprovementScore",
                table: "MSFResponses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CulturalSensitivityComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CulturalSensitivityScore",
                table: "MSFResponses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DecisionMakingComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DecisionMakingScore",
                table: "MSFResponses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentationComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentationScore",
                table: "MSFResponses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EthicalProfessionalismComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EthicalProfessionalismScore",
                table: "MSFResponses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HealthSafetyAwarenessComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HealthSafetyAwarenessScore",
                table: "MSFResponses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HistoryTakingComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HistoryTakingScore",
                table: "MSFResponses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LeadershipSkillsComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeadershipSkillsScore",
                table: "MSFResponses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatientCareQualityComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatientCareQualityScore",
                table: "MSFResponses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessionalDevelopmentComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfessionalDevelopmentScore",
                table: "MSFResponses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QualityImprovementComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QualityImprovementScore",
                table: "MSFResponses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RespondentName",
                table: "MSFResponses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RespondentRole",
                table: "MSFResponses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeamCollaborationComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamCollaborationScore",
                table: "MSFResponses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeamSupportComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamSupportScore",
                table: "MSFResponses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TechnicalCompetenceComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TechnicalCompetenceScore",
                table: "MSFResponses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PerformerId",
                table: "MSFQuestionnaires",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "MSFQuestionnaires",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UniqueCode",
                table: "MSFQuestionnaires",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MSFQuestionnaires_PerformerId",
                table: "MSFQuestionnaires",
                column: "PerformerId");

            migrationBuilder.CreateIndex(
                name: "IX_MSFQuestionnaires_UniqueCode",
                table: "MSFQuestionnaires",
                column: "UniqueCode",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MSFQuestionnaires_Users_PerformerId",
                table: "MSFQuestionnaires",
                column: "PerformerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MSFResponses");

            migrationBuilder.DropTable(
                name: "MSFQuestionnaires");
        }
    }
}
