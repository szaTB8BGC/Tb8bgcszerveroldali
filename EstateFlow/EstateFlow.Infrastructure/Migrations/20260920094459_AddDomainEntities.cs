using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EstateFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDomainEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Meters",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProviderId",
                table: "Meters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ReadingPeriod",
                table: "Meters",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UtilityTypeId",
                table: "Meters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MeterReadings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MeasuredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Value = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: false),
                    ReportedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PhotoPath = table.Column<string>(type: "text", nullable: true),
                    MeterId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeterReadings_Meters_MeterId",
                        column: x => x.MeterId,
                        principalTable: "Meters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UtilityTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilityTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rentals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MonthlyRent = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PropertyId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rentals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rentals_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rentals_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    UtilityTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Providers_UtilityTypes_UtilityTypeId",
                        column: x => x.UtilityTypeId,
                        principalTable: "UtilityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("c56a4180-65aa-42ec-a945-5fd21dec0538"), "kovacs.anna@example.com", "Anna", "Kovács", "+36 20 123 4567" },
                    { new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"), "szabo.peter@example.com", "Péter", "Szabó", null }
                });

            migrationBuilder.InsertData(
                table: "UtilityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Electricity" },
                    { 2, "Gas" },
                    { 3, "Water" }
                });

            migrationBuilder.InsertData(
                table: "Providers",
                columns: new[] { "Id", "Name", "Note", "UtilityTypeId" },
                values: new object[,]
                {
                    { 1, "MVM Next", null, 1 },
                    { 2, "FŐGÁZ", "a gázórákat évente egyszer olvassák le", 2 },
                    { 3, "Fővárosi Vízművek", null, 3 },
                    { 4, "Debreceni Vízmű", null, 3 }
                });

            migrationBuilder.InsertData(
                table: "Rentals",
                columns: new[] { "Id", "EndDate", "MonthlyRent", "PropertyId", "StartDate", "TenantId" },
                values: new object[,]
                {
                    { new Guid("0b28a2f0-762c-4b7f-8b3e-9a5d1c4e6f01"), null, 240000m, new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("c56a4180-65aa-42ec-a945-5fd21dec0538") },
                    { new Guid("0b28a2f0-762c-4b7f-8b3e-9a5d1c4e6f02"), new DateTime(2026, 8, 31, 0, 0, 0, 0, DateTimeKind.Utc), 150000m, new Guid("9b1deb4d-3b7d-4bad-9bdd-2b0d7b3dcb6d"), new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479") }
                });

            migrationBuilder.InsertData(
                table: "Meters",
                columns: new[] { "Id", "Location", "Name", "Note", "PropertyId", "ProviderId", "ReadingPeriod", "SerialNumber", "UtilityTypeId" },
                values: new object[,]
                {
                    { new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3301"), "előszobai szekrényben", "Villanyóra", null, new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"), 1, "minden hónap elején", "EL-2019-448723", 1 },
                    { new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3302"), "konyhában a bejárati ajtó mellett", "Gázóra", "2027-ben hitelesítés esedékes", new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"), 2, null, "GA-2017-902211", 2 },
                    { new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3303"), "fürdőszobában a kád alatt", "Vízóra", null, new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"), 3, "minden páros hónapban van leolvasás", "VZ-2021-115847", 3 },
                    { new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3304"), "folyosón a bejárati ajtó mellett", "Villanyóra", null, new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"), 1, "minden hónap elején", "EL-2020-771139", 1 },
                    { new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3305"), "konyhában a mosogató alatt", "Vízóra", null, new Guid("9b1deb4d-3b7d-4bad-9bdd-2b0d7b3dcb6d"), 4, null, "VZ-2018-660254", 3 }
                });

            migrationBuilder.InsertData(
                table: "MeterReadings",
                columns: new[] { "Id", "ApprovedAt", "MeasuredAt", "MeterId", "PhotoPath", "ReportedAt", "Value" },
                values: new object[,]
                {
                    { new Guid("8d4d4e51-6d1a-4c62-a1f8-58a0e0a4c001"), new DateTime(2026, 3, 2, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3301"), "uploads/readings/bpszk-villany-2026-03.jpg", new DateTime(2026, 3, 1, 8, 5, 0, 0, DateTimeKind.Utc), 12450m },
                    { new Guid("8d4d4e51-6d1a-4c62-a1f8-58a0e0a4c002"), new DateTime(2026, 4, 2, 10, 15, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 1, 7, 40, 0, 0, DateTimeKind.Utc), new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3301"), "uploads/readings/bpszk-villany-2026-04.jpg", new DateTime(2026, 4, 1, 7, 45, 0, 0, DateTimeKind.Utc), 12588m },
                    { new Guid("8d4d4e51-6d1a-4c62-a1f8-58a0e0a4c003"), new DateTime(2026, 5, 4, 8, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 1, 9, 30, 0, 0, DateTimeKind.Utc), new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3301"), null, new DateTime(2026, 5, 3, 18, 20, 0, 0, DateTimeKind.Utc), 12731m },
                    { new Guid("8d4d4e51-6d1a-4c62-a1f8-58a0e0a4c004"), null, new DateTime(2026, 6, 1, 8, 10, 0, 0, DateTimeKind.Utc), new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3301"), "uploads/readings/bpszk-villany-2026-06.jpg", new DateTime(2026, 6, 1, 9, 10, 0, 0, DateTimeKind.Utc), 12876m },
                    { new Guid("8d4d4e51-6d1a-4c62-a1f8-58a0e0a4c005"), new DateTime(2026, 4, 3, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 1, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3303"), "uploads/readings/bpszk-viz-2026-04.jpg", new DateTime(2026, 4, 1, 10, 5, 0, 0, DateTimeKind.Utc), 341.605m },
                    { new Guid("8d4d4e51-6d1a-4c62-a1f8-58a0e0a4c006"), null, new DateTime(2026, 6, 1, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3303"), "uploads/readings/bpszk-viz-2026-06.jpg", new DateTime(2026, 6, 2, 16, 45, 0, 0, DateTimeKind.Utc), 348.112m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meters_ProviderId",
                table: "Meters",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Meters_UtilityTypeId",
                table: "Meters",
                column: "UtilityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MeterReadings_MeterId",
                table: "MeterReadings",
                column: "MeterId");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_UtilityTypeId",
                table: "Providers",
                column: "UtilityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_PropertyId",
                table: "Rentals",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_TenantId",
                table: "Rentals",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meters_Providers_ProviderId",
                table: "Meters",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Meters_UtilityTypes_UtilityTypeId",
                table: "Meters",
                column: "UtilityTypeId",
                principalTable: "UtilityTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meters_Providers_ProviderId",
                table: "Meters");

            migrationBuilder.DropForeignKey(
                name: "FK_Meters_UtilityTypes_UtilityTypeId",
                table: "Meters");

            migrationBuilder.DropTable(
                name: "MeterReadings");

            migrationBuilder.DropTable(
                name: "Providers");

            migrationBuilder.DropTable(
                name: "Rentals");

            migrationBuilder.DropTable(
                name: "UtilityTypes");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Meters_ProviderId",
                table: "Meters");

            migrationBuilder.DropIndex(
                name: "IX_Meters_UtilityTypeId",
                table: "Meters");

            migrationBuilder.DeleteData(
                table: "Meters",
                keyColumn: "Id",
                keyValue: new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3301"));

            migrationBuilder.DeleteData(
                table: "Meters",
                keyColumn: "Id",
                keyValue: new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3302"));

            migrationBuilder.DeleteData(
                table: "Meters",
                keyColumn: "Id",
                keyValue: new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3303"));

            migrationBuilder.DeleteData(
                table: "Meters",
                keyColumn: "Id",
                keyValue: new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3304"));

            migrationBuilder.DeleteData(
                table: "Meters",
                keyColumn: "Id",
                keyValue: new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3305"));

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Meters");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "Meters");

            migrationBuilder.DropColumn(
                name: "ReadingPeriod",
                table: "Meters");

            migrationBuilder.DropColumn(
                name: "UtilityTypeId",
                table: "Meters");
        }
    }
}
