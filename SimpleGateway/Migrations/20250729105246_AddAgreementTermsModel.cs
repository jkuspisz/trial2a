using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SimpleGateway.Migrations
{
    /// <inheritdoc />
    public partial class AddAgreementTermsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgreementTerms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Restriction1 = table.Column<bool>(type: "boolean", nullable: false),
                    Restriction2 = table.Column<bool>(type: "boolean", nullable: false),
                    Restriction3 = table.Column<bool>(type: "boolean", nullable: false),
                    Restriction4 = table.Column<bool>(type: "boolean", nullable: false),
                    Restriction5 = table.Column<bool>(type: "boolean", nullable: false),
                    Action1 = table.Column<bool>(type: "boolean", nullable: false),
                    Action2 = table.Column<bool>(type: "boolean", nullable: false),
                    Action3 = table.Column<bool>(type: "boolean", nullable: false),
                    Action4 = table.Column<bool>(type: "boolean", nullable: false),
                    Action5 = table.Column<bool>(type: "boolean", nullable: false),
                    Action6 = table.Column<bool>(type: "boolean", nullable: false),
                    Action7 = table.Column<bool>(type: "boolean", nullable: false),
                    Action8 = table.Column<bool>(type: "boolean", nullable: false),
                    Action9 = table.Column<bool>(type: "boolean", nullable: false),
                    Action10 = table.Column<bool>(type: "boolean", nullable: false),
                    Action11 = table.Column<bool>(type: "boolean", nullable: false),
                    Action12 = table.Column<bool>(type: "boolean", nullable: false),
                    Action13 = table.Column<bool>(type: "boolean", nullable: false),
                    Action14 = table.Column<bool>(type: "boolean", nullable: false),
                    Action15 = table.Column<bool>(type: "boolean", nullable: false),
                    Action16 = table.Column<bool>(type: "boolean", nullable: false),
                    Action17 = table.Column<bool>(type: "boolean", nullable: false),
                    CustomTerms = table.Column<string>(type: "text", nullable: true),
                    IsReleased = table.Column<bool>(type: "boolean", nullable: false),
                    ReleasedBy = table.Column<string>(type: "text", nullable: true),
                    ReleasedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsAgreedByPerformer = table.Column<bool>(type: "boolean", nullable: false),
                    AgreedBy = table.Column<string>(type: "text", nullable: true),
                    AgreedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgreementTerms", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgreementTerms");
        }
    }
}
