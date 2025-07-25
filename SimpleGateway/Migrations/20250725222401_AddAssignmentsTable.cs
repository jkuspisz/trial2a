using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SimpleGateway.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignmentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileUploads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    HasIndemnityEvidence = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUploads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PerformerDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GDCNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ContactNumber = table.Column<string>(type: "TEXT", nullable: false),
                    SupportingDentist = table.Column<string>(type: "TEXT", nullable: false),
                    SupportingDentistContactNumber = table.Column<string>(type: "TEXT", nullable: false),
                    PracticeAddress = table.Column<string>(type: "TEXT", nullable: false),
                    PracticePostCode = table.Column<string>(type: "TEXT", nullable: false),
                    DateOfUKRegistration = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateOfDentalQualification = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UniversityCountryOfQualification = table.Column<string>(type: "TEXT", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformerDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Department = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileUploadEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileId = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    FileSize = table.Column<long>(type: "INTEGER", nullable: false),
                    ContentType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    IsRequired = table.Column<bool>(type: "INTEGER", nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    FileUploadModelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUploadEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileUploadEntries_FileUploads_FileUploadModelId",
                        column: x => x.FileUploadModelId,
                        principalTable: "FileUploads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PerformerId = table.Column<int>(type: "INTEGER", nullable: false),
                    SupervisorId = table.Column<int>(type: "INTEGER", nullable: true),
                    AdvisorId = table.Column<int>(type: "INTEGER", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignments_Users_AdvisorId",
                        column: x => x.AdvisorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Assignments_Users_PerformerId",
                        column: x => x.PerformerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_Users_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "Department", "DisplayName", "Email", "FirstName", "IsActive", "LastLoginDate", "LastName", "Password", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 25, 22, 24, 0, 328, DateTimeKind.Utc).AddTicks(8204), null, "John Smith", "john.smith@example.com", "John", true, null, "Smith", "password123", "performer", "performer1" },
                    { 2, new DateTime(2025, 7, 25, 22, 24, 0, 328, DateTimeKind.Utc).AddTicks(8625), null, "Jane Johnson", "jane.johnson@example.com", "Jane", true, null, "Johnson", "password123", "performer", "performer2" },
                    { 3, new DateTime(2025, 7, 25, 22, 24, 0, 328, DateTimeKind.Utc).AddTicks(8629), null, "Mike Wilson", "mike.wilson@example.com", "Mike", true, null, "Wilson", "password123", "performer", "performer3" },
                    { 4, new DateTime(2025, 7, 25, 22, 24, 0, 328, DateTimeKind.Utc).AddTicks(8633), null, "Dr. Sarah Davis", "sarah.davis@example.com", "Dr. Sarah", true, null, "Davis", "password123", "advisor", "advisor1" },
                    { 5, new DateTime(2025, 7, 25, 22, 24, 0, 328, DateTimeKind.Utc).AddTicks(8637), null, "Prof. Robert Brown", "robert.brown@example.com", "Prof. Robert", true, null, "Brown", "password123", "supervisor", "supervisor1" },
                    { 6, new DateTime(2025, 7, 25, 22, 24, 0, 328, DateTimeKind.Utc).AddTicks(8641), null, "Admin User", "admin@example.com", "Admin", true, null, "User", "password123", "admin", "admin1" },
                    { 7, new DateTime(2025, 7, 25, 22, 24, 0, 328, DateTimeKind.Utc).AddTicks(8645), null, "Super User", "superuser@example.com", "Super", true, null, "User", "password123", "superuser", "superuser" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AdvisorId",
                table: "Assignments",
                column: "AdvisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_PerformerId",
                table: "Assignments",
                column: "PerformerId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_SupervisorId",
                table: "Assignments",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_FileUploadEntries_FileUploadModelId",
                table: "FileUploadEntries",
                column: "FileUploadModelId");

            migrationBuilder.CreateIndex(
                name: "IX_FileUploads_Username",
                table: "FileUploads",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "FileUploadEntries");

            migrationBuilder.DropTable(
                name: "PerformerDetails");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "FileUploads");
        }
    }
}
