using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SimpleGateway.Migrations.TestData2
{
    /// <inheritdoc />
    public partial class AddAdvisorCommentField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestData2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UKWorkExperience = table.Column<string>(type: "text", nullable: false),
                    LastPatientTreatment = table.Column<string>(type: "text", nullable: false),
                    GDCRegistrationNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UKRegistrationDate = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    GDCRegistrationGaps = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Qualification1 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Qualification1Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Qualification1Institution = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Qualification1Year = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Qualification2 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Qualification2Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Qualification2Institution = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Qualification2Year = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Qualification3 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Qualification3Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Qualification3Institution = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Qualification3Year = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Qualification4 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Qualification4Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Qualification4Institution = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Qualification4Year = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Qualification5 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Qualification5Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Qualification5Institution = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Qualification5Year = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Qualification6 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Qualification6Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Qualification6Institution = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Qualification6Year = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Job1FromDate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Job1ToDate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Job1Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Job1Address = table.Column<string>(type: "text", nullable: true),
                    Job1Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Job1AdultPatients = table.Column<bool>(type: "boolean", nullable: false),
                    Job1ChildPatients = table.Column<bool>(type: "boolean", nullable: false),
                    Job2FromDate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Job2ToDate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Job2Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Job2Address = table.Column<string>(type: "text", nullable: true),
                    Job2Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Job2AdultPatients = table.Column<bool>(type: "boolean", nullable: false),
                    Job2ChildPatients = table.Column<bool>(type: "boolean", nullable: false),
                    Job3FromDate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Job3ToDate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Job3Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Job3Address = table.Column<string>(type: "text", nullable: true),
                    Job3Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Job3AdultPatients = table.Column<bool>(type: "boolean", nullable: false),
                    Job3ChildPatients = table.Column<bool>(type: "boolean", nullable: false),
                    Job4FromDate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Job4ToDate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Job4Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Job4Address = table.Column<string>(type: "text", nullable: true),
                    Job4Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Job4AdultPatients = table.Column<bool>(type: "boolean", nullable: false),
                    Job4ChildPatients = table.Column<bool>(type: "boolean", nullable: false),
                    Job5FromDate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Job5ToDate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Job5Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Job5Address = table.Column<string>(type: "text", nullable: true),
                    Job5Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Job5AdultPatients = table.Column<bool>(type: "boolean", nullable: false),
                    Job5ChildPatients = table.Column<bool>(type: "boolean", nullable: false),
                    Job6FromDate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Job6ToDate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Job6Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Job6Address = table.Column<string>(type: "text", nullable: true),
                    Job6Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Job6AdultPatients = table.Column<bool>(type: "boolean", nullable: false),
                    Job6ChildPatients = table.Column<bool>(type: "boolean", nullable: false),
                    ClinicalExamNumber = table.Column<int>(type: "integer", nullable: true),
                    ClinicalExamConfidence = table.Column<int>(type: "integer", nullable: true),
                    ClinicalExamLastDate = table.Column<string>(type: "text", nullable: true),
                    ClinicalExamNeedSupport = table.Column<bool>(type: "boolean", nullable: false),
                    BPENumber = table.Column<int>(type: "integer", nullable: true),
                    BPEConfidence = table.Column<int>(type: "integer", nullable: true),
                    BPELastDate = table.Column<string>(type: "text", nullable: true),
                    BPENeedSupport = table.Column<bool>(type: "boolean", nullable: false),
                    SixPPCNumber = table.Column<int>(type: "integer", nullable: true),
                    SixPPCConfidence = table.Column<int>(type: "integer", nullable: true),
                    SixPPCLastDate = table.Column<string>(type: "text", nullable: true),
                    SixPPCNeedSupport = table.Column<bool>(type: "boolean", nullable: false),
                    RadiographsNumber = table.Column<int>(type: "integer", nullable: true),
                    RadiographsConfidence = table.Column<int>(type: "integer", nullable: true),
                    RadiographsLastDate = table.Column<string>(type: "text", nullable: true),
                    RadiographsNeedSupport = table.Column<bool>(type: "boolean", nullable: false),
                    ExtractionsNumber = table.Column<int>(type: "integer", nullable: true),
                    ExtractionsConfidence = table.Column<int>(type: "integer", nullable: true),
                    ExtractionsLastDate = table.Column<string>(type: "text", nullable: true),
                    ExtractionsNeedSupport = table.Column<bool>(type: "boolean", nullable: false),
                    RootDivisionNumber = table.Column<int>(type: "integer", nullable: true),
                    RootDivisionConfidence = table.Column<int>(type: "integer", nullable: true),
                    RootDivisionLastDate = table.Column<string>(type: "text", nullable: true),
                    RootDivisionNeedSupport = table.Column<bool>(type: "boolean", nullable: false),
                    ScalingNumber = table.Column<int>(type: "integer", nullable: true),
                    ScalingConfidence = table.Column<int>(type: "integer", nullable: true),
                    ScalingLastDate = table.Column<string>(type: "text", nullable: true),
                    ScalingNeedSupport = table.Column<bool>(type: "boolean", nullable: false),
                    AmalgamNumber = table.Column<int>(type: "integer", nullable: true),
                    AmalgamConfidence = table.Column<int>(type: "integer", nullable: true),
                    AmalgamLastDate = table.Column<string>(type: "text", nullable: true),
                    AmalgamNeedSupport = table.Column<bool>(type: "boolean", nullable: false),
                    CompositeNumber = table.Column<int>(type: "integer", nullable: true),
                    CompositeConfidence = table.Column<int>(type: "integer", nullable: true),
                    CompositeLastDate = table.Column<string>(type: "text", nullable: true),
                    CompositeNeedSupport = table.Column<bool>(type: "boolean", nullable: false),
                    CrownsNumber = table.Column<int>(type: "integer", nullable: true),
                    CrownsConfidence = table.Column<int>(type: "integer", nullable: true),
                    CrownsLastDate = table.Column<string>(type: "text", nullable: true),
                    CrownsNeedSupport = table.Column<bool>(type: "boolean", nullable: false),
                    DenturesNumber = table.Column<int>(type: "integer", nullable: true),
                    DenturesConfidence = table.Column<int>(type: "integer", nullable: true),
                    DenturesLastDate = table.Column<string>(type: "text", nullable: true),
                    DenturesNeedSupport = table.Column<bool>(type: "boolean", nullable: false),
                    SingleEndoNumber = table.Column<int>(type: "integer", nullable: true),
                    SingleEndoConfidence = table.Column<int>(type: "integer", nullable: true),
                    SingleEndoLastDate = table.Column<string>(type: "text", nullable: true),
                    SingleEndoNeedSupport = table.Column<bool>(type: "boolean", nullable: false),
                    MultiEndoNumber = table.Column<int>(type: "integer", nullable: true),
                    MultiEndoConfidence = table.Column<int>(type: "integer", nullable: true),
                    MultiEndoLastDate = table.Column<string>(type: "text", nullable: true),
                    MultiEndoNeedSupport = table.Column<bool>(type: "boolean", nullable: false),
                    PeadsNumber = table.Column<int>(type: "integer", nullable: true),
                    PeadsConfidence = table.Column<int>(type: "integer", nullable: true),
                    PeadsLastDate = table.Column<string>(type: "text", nullable: true),
                    PeadsNeedSupport = table.Column<bool>(type: "boolean", nullable: false),
                    AdvisorComment = table.Column<string>(type: "text", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestData2", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestData2_Username",
                table: "TestData2",
                column: "Username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestData2");
        }
    }
}
