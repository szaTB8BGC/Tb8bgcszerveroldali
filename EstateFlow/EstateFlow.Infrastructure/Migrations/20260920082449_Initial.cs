using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EstateFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    SquareMeters = table.Column<int>(type: "integer", nullable: false),
                    AdvertisementText = table.Column<string>(type: "text", nullable: true),
                    CityId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Properties_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PropertyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyImages_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Budapest" },
                    { 2, "Debrecen" },
                    { 3, "Szeged" },
                    { 4, "Kecskemét" }
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "Address", "AdvertisementText", "CityId", "Code", "Name", "SquareMeters" },
                values: new object[,]
                {
                    { new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"), "1068 Budapest, Szondi utca 45.", "Világos, felújított másfél szobás lakás a VI. kerület szívében, a Nyugati pályaudvartól 10 percre.", 1, "BPSZK", "Szondi utcai lakás", 54 },
                    { new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"), "1092 Budapest, Ferenc körút 12.", null, 1, "BPFKR", "Ferenc körúti lakás", 68 },
                    { new Guid("9b1deb4d-3b7d-4bad-9bdd-2b0d7b3dcb6d"), "4026 Debrecen, Egyetem sugárút 8.", "Hangulatos, erkélyes lakás a Nagyerdő közelében, egyetemistáknak ideális.", 2, "DEEGY", "Egyetem sugárúti lakás", 47 }
                });

            migrationBuilder.InsertData(
                table: "PropertyImages",
                columns: new[] { "Id", "FilePath", "Name", "PropertyId", "UploadedAt" },
                values: new object[,]
                {
                    { new Guid("6ec0bd7f-11c0-43da-975e-2a8ad9ebae0b"), "uploads/properties/bpszk-haloszoba.jpg", "Hálószoba", new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"), new DateTime(2026, 1, 15, 10, 32, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b0788d2f-8003-43c1-92a4-edc76a7c5dde"), "uploads/properties/bpszk-nappali.jpg", "Nappali", new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"), new DateTime(2026, 1, 15, 10, 30, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d2719b1e-1c3b-4e5f-9a6d-8b7c6d5e4f3a"), "uploads/properties/deegy-konyha.jpg", "Konyha", new Guid("9b1deb4d-3b7d-4bad-9bdd-2b0d7b3dcb6d"), new DateTime(2026, 2, 10, 9, 15, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f9168c5e-ceb2-4faa-b6bf-329bf39fa1e4"), "uploads/properties/bpfkr-homlokzat.jpg", "Utcai homlokzat", new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"), new DateTime(2026, 2, 3, 14, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Properties_CityId",
                table: "Properties",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_Code",
                table: "Properties",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyImages_PropertyId",
                table: "PropertyImages",
                column: "PropertyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertyImages");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
