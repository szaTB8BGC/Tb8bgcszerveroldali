using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EstateFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExpenseAndPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    PropertyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RentAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    UtilitiesAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PeriodYear = table.Column<int>(type: "integer", nullable: false),
                    PeriodMonth = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    PropertyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Expenses",
                columns: new[] { "Id", "Amount", "Name", "Note", "PaidAt", "PropertyId", "Type" },
                values: new object[,]
                {
                    { new Guid("a3d92f10-4b6e-4c2a-9d1f-7e8b5c3a2001"), 28500m, "Vízvezeték-javítás a fürdőszobában", null, new DateTime(2026, 3, 12, 14, 20, 0, 0, DateTimeKind.Utc), new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"), "Repair" },
                    { new Guid("a3d92f10-4b6e-4c2a-9d1f-7e8b5c3a2002"), 18000m, "Gázkazán éves karbantartása", "kötelező éves felülvizsgálat", new DateTime(2026, 4, 5, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"), "Maintenance" },
                    { new Guid("a3d92f10-4b6e-4c2a-9d1f-7e8b5c3a2003"), 45200m, "Építményadó, I. félév", null, new DateTime(2026, 4, 20, 11, 30, 0, 0, DateTimeKind.Utc), new Guid("9b1deb4d-3b7d-4bad-9bdd-2b0d7b3dcb6d"), "Tax" },
                    { new Guid("a3d92f10-4b6e-4c2a-9d1f-7e8b5c3a2004"), 12900m, "Zárcsere a bejárati ajtón", null, new DateTime(2026, 5, 8, 16, 45, 0, 0, DateTimeKind.Utc), new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"), "Maintenance" }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "Note", "PaidAt", "PeriodMonth", "PeriodYear", "PropertyId", "RentAmount", "TenantId", "UtilitiesAmount" },
                values: new object[,]
                {
                    { new Guid("c7e14b20-8f3a-4d5b-a2c9-1f6e9d8b3001"), null, new DateTime(2026, 3, 3, 10, 15, 0, 0, DateTimeKind.Utc), 3, 2026, new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"), 240000m, new Guid("c56a4180-65aa-42ec-a945-5fd21dec0538"), 36500m },
                    { new Guid("c7e14b20-8f3a-4d5b-a2c9-1f6e9d8b3002"), null, new DateTime(2026, 4, 2, 8, 40, 0, 0, DateTimeKind.Utc), 4, 2026, new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"), 240000m, new Guid("c56a4180-65aa-42ec-a945-5fd21dec0538"), 41200m },
                    { new Guid("c7e14b20-8f3a-4d5b-a2c9-1f6e9d8b3003"), "két nap késéssel érkezett", new DateTime(2026, 5, 4, 19, 5, 0, 0, DateTimeKind.Utc), 5, 2026, new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"), 240000m, new Guid("c56a4180-65aa-42ec-a945-5fd21dec0538"), 33800m },
                    { new Guid("c7e14b20-8f3a-4d5b-a2c9-1f6e9d8b3004"), null, new DateTime(2026, 6, 2, 12, 0, 0, 0, DateTimeKind.Utc), 6, 2026, new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"), 240000m, new Guid("c56a4180-65aa-42ec-a945-5fd21dec0538"), 30150m },
                    { new Guid("c7e14b20-8f3a-4d5b-a2c9-1f6e9d8b3005"), null, new DateTime(2026, 5, 10, 7, 55, 0, 0, DateTimeKind.Utc), 5, 2026, new Guid("9b1deb4d-3b7d-4bad-9bdd-2b0d7b3dcb6d"), 150000m, new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"), 22400m },
                    { new Guid("c7e14b20-8f3a-4d5b-a2c9-1f6e9d8b3006"), null, new DateTime(2026, 6, 9, 17, 30, 0, 0, DateTimeKind.Utc), 6, 2026, new Guid("9b1deb4d-3b7d-4bad-9bdd-2b0d7b3dcb6d"), 150000m, new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"), 21750m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_PropertyId",
                table: "Expenses",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PropertyId",
                table: "Payments",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TenantId",
                table: "Payments",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}
