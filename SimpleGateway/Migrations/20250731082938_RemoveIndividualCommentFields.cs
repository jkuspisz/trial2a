using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleGateway.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIndividualCommentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommunicationEmpathyComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "CommunicationSkillsComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "ConsultationManagementComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "ContinuousImprovementComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "CulturalSensitivityComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "DecisionMakingComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "DocumentationComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "EthicalProfessionalismComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "HealthSafetyAwarenessComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "HistoryTakingComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "LeadershipSkillsComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "PatientCareQualityComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "ProfessionalDevelopmentComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "QualityImprovementComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "TeamCollaborationComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "TeamSupportComment",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "TechnicalCompetenceComment",
                table: "MSFResponses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommunicationEmpathyComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommunicationSkillsComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConsultationManagementComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContinuousImprovementComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CulturalSensitivityComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DecisionMakingComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentationComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EthicalProfessionalismComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HealthSafetyAwarenessComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HistoryTakingComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LeadershipSkillsComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatientCareQualityComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessionalDevelopmentComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QualityImprovementComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeamCollaborationComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeamSupportComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TechnicalCompetenceComment",
                table: "MSFResponses",
                type: "text",
                nullable: true);
        }
    }
}
