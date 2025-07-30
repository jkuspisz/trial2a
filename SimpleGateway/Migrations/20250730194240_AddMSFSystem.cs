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
            migrationBuilder.DropIndex(
                name: "IX_MSFQuestionnaires_UniqueIdentifier",
                table: "MSFQuestionnaires");

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
            migrationBuilder.DropForeignKey(
                name: "FK_MSFQuestionnaires_Users_PerformerId",
                table: "MSFQuestionnaires");

            migrationBuilder.DropIndex(
                name: "IX_MSFQuestionnaires_PerformerId",
                table: "MSFQuestionnaires");

            migrationBuilder.DropIndex(
                name: "IX_MSFQuestionnaires_UniqueCode",
                table: "MSFQuestionnaires");

            migrationBuilder.DropColumn(
                name: "AdditionalComments",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "CommunicationEmpathyComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "CommunicationEmpathyScore",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "CommunicationSkillsComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "CommunicationSkillsScore",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "ConsultationManagementComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "ConsultationManagementScore",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "ContinuousImprovementComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "ContinuousImprovementScore",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "CulturalSensitivityComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "CulturalSensitivityScore",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "DecisionMakingComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "DecisionMakingScore",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "DocumentationComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "DocumentationScore",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "EthicalProfessionalismComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "EthicalProfessionalismScore",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "HealthSafetyAwarenessComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "HealthSafetyAwarenessScore",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "HistoryTakingComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "HistoryTakingScore",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "LeadershipSkillsComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "LeadershipSkillsScore",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "PatientCareQualityComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "PatientCareQualityScore",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "ProfessionalDevelopmentComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "ProfessionalDevelopmentScore",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "QualityImprovementComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "QualityImprovementScore",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "RespondentName",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "RespondentRole",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "TeamCollaborationComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "TeamCollaborationScore",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "TeamSupportComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "TeamSupportScore",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "TechnicalCompetenceComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "TechnicalCompetenceScore",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "PerformerId",
                table: "MSFQuestionnaires");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "MSFQuestionnaires");

            migrationBuilder.DropColumn(
                name: "UniqueCode",
                table: "MSFQuestionnaires");

            migrationBuilder.AddColumn<string>(
                name: "CouldImproveUpon",
                table: "MSFResponses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DemonstrateIntegrity",
                table: "MSFResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DoesParticularlyWell",
                table: "MSFResponses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EnableInformedDecisions",
                table: "MSFResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EngageWithDevelopment",
                table: "MSFResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FacilitateLearning",
                table: "MSFResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InteractWithColleagues",
                table: "MSFResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "KeepPracticeUpToDate",
                table: "MSFResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ManageTimeResources",
                table: "MSFResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinimiseWasteEnvironment",
                table: "MSFResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParticipateInImprovement",
                table: "MSFResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProduceClearCommunications",
                table: "MSFResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PromoteEqualityDiversity",
                table: "MSFResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecogniseCommNeedsPatients",
                table: "MSFResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecogniseImpactOnOthers",
                table: "MSFResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RelationshipToPerformer",
                table: "MSFResponses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RespondentIdentifier",
                table: "MSFResponses",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TreatPatientsWithCompassion",
                table: "MSFResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkAsTeamMember",
                table: "MSFResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkToStandards",
                table: "MSFResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkWithinScope",
                table: "MSFResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WorkingRelationshipDuration",
                table: "MSFResponses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PerformerUsername",
                table: "MSFQuestionnaires",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UniqueIdentifier",
                table: "MSFQuestionnaires",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MSFQuestionnaires_UniqueIdentifier",
                table: "MSFQuestionnaires",
                column: "UniqueIdentifier",
                unique: true);
        }
    }
}
