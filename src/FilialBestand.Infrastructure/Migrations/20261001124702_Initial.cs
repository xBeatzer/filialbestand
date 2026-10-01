using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FilialBestand.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artikel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SapProduktNummer = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Bezeichnung = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Basiseinheit = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Warengruppe = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ZuletztSynchronisiert = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artikel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Filialen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nummer = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Ort = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filialen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bestaende",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FilialeId = table.Column<int>(type: "integer", nullable: false),
                    ArtikelId = table.Column<int>(type: "integer", nullable: false),
                    Menge = table.Column<int>(type: "integer", nullable: false),
                    Mindestbestand = table.Column<int>(type: "integer", nullable: false),
                    Zielbestand = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bestaende", x => x.Id);
                    table.CheckConstraint("CK_Bestand_Menge", "\"Menge\" >= 0");
                    table.CheckConstraint("CK_Bestand_Ziel", "\"Zielbestand\" >= \"Mindestbestand\"");
                    table.ForeignKey(
                        name: "FK_Bestaende_Artikel_ArtikelId",
                        column: x => x.ArtikelId,
                        principalTable: "Artikel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bestaende_Filialen_FilialeId",
                        column: x => x.FilialeId,
                        principalTable: "Filialen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Nachbestellvorschlaege",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FilialeId = table.Column<int>(type: "integer", nullable: false),
                    ArtikelId = table.Column<int>(type: "integer", nullable: false),
                    Menge = table.Column<int>(type: "integer", nullable: false),
                    ErstelltAm = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nachbestellvorschlaege", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nachbestellvorschlaege_Artikel_ArtikelId",
                        column: x => x.ArtikelId,
                        principalTable: "Artikel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Nachbestellvorschlaege_Filialen_FilialeId",
                        column: x => x.FilialeId,
                        principalTable: "Filialen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artikel_SapProduktNummer",
                table: "Artikel",
                column: "SapProduktNummer",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bestaende_ArtikelId",
                table: "Bestaende",
                column: "ArtikelId");

            migrationBuilder.CreateIndex(
                name: "IX_Bestaende_FilialeId_ArtikelId",
                table: "Bestaende",
                columns: new[] { "FilialeId", "ArtikelId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nachbestellvorschlaege_ArtikelId",
                table: "Nachbestellvorschlaege",
                column: "ArtikelId");

            migrationBuilder.CreateIndex(
                name: "IX_Nachbestellvorschlaege_FilialeId_ArtikelId",
                table: "Nachbestellvorschlaege",
                columns: new[] { "FilialeId", "ArtikelId" },
                unique: true,
                filter: "\"Status\" = 'Offen'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bestaende");

            migrationBuilder.DropTable(
                name: "Nachbestellvorschlaege");

            migrationBuilder.DropTable(
                name: "Artikel");

            migrationBuilder.DropTable(
                name: "Filialen");
        }
    }
}
