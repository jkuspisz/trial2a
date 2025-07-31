using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleGateway.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAdditionalComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalComments",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "RespondentName",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "RespondentRole",
                table: "MSFResponses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalComments",
                table: "MSFResponses",
                type: "text",
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
        }
    }
}
