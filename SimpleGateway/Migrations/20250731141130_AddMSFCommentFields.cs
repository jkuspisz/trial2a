using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleGateway.Migrations
{
    /// <inheritdoc />
    public partial class AddMSFCommentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImprovementComments",
                table: "MSFResponses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PositiveComments",
                table: "MSFResponses",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImprovementComments",
                table: "MSFResponses");

            migrationBuilder.DropColumn(
                name: "PositiveComments",
                table: "MSFResponses");
        }
    }
}
