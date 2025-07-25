using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleGateway.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformerDetailsToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PerformerDetails_Username",
                table: "PerformerDetails",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PerformerDetails_Username",
                table: "PerformerDetails");
        }
    }
}
