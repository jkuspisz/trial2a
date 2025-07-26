using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SimpleGateway.Migrations
{
    /// <inheritdoc />
    public partial class RedesignPreviousExperienceToSingleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmploymentHistoryJobs");

            migrationBuilder.DropTable(
                name: "Qualifications");

            migrationBuilder.AddColumn<string>(
                name: "ClinicalExperienceJson",
                table: "PreviousExperiences",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PreviousExperiences",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EmploymentHistoryJson",
                table: "PreviousExperiences",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QualificationsJson",
                table: "PreviousExperiences",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PreviousExperiences",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClinicalExperienceJson",
                table: "PreviousExperiences");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PreviousExperiences");

            migrationBuilder.DropColumn(
                name: "EmploymentHistoryJson",
                table: "PreviousExperiences");

            migrationBuilder.DropColumn(
                name: "QualificationsJson",
                table: "PreviousExperiences");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PreviousExperiences");

            migrationBuilder.CreateTable(
                name: "EmploymentHistoryJobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    From = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    JobTitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    To = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TreatedAdults = table.Column<bool>(type: "boolean", nullable: false),
                    TreatedChildren = table.Column<bool>(type: "boolean", nullable: false),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploymentHistoryJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Qualifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Institution = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    QualificationName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Year = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualifications", x => x.Id);
                });
        }
    }
}
