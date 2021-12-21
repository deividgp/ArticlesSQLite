using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArticlesSQLite.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Families",
                columns: table => new
                {
                    CodiFamilia = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Descripcio = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Familia", x => x.CodiFamilia);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    CodiArticle = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CodiFamilia = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    Descripcio = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Envas = table.Column<int>(type: "int", nullable: false),
                    Pes = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    CodiIVA = table.Column<byte>(type: "tinyint", nullable: true),
                    DataAlta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataUltSortida = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NoFacturar = table.Column<bool>(type: "bit", nullable: false),
                    NoSortirInventaris = table.Column<bool>(type: "bit", nullable: false),
                    StockMinim = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    StockRuptura = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ControlStock = table.Column<bool>(type: "bit", nullable: false),
                    StockPendRebre = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    DataPendRebre = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Comisio = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    DataCost = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PreuCostMig = table.Column<decimal>(type: "decimal(13,5)", precision: 13, scale: 5, nullable: false),
                    StockCost = table.Column<decimal>(type: "decimal(13,5)", precision: 13, scale: 5, nullable: false),
                    PreuCostUltim = table.Column<decimal>(type: "decimal(13,5)", precision: 13, scale: 5, nullable: false),
                    PreuVenda = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false),
                    Observacions = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.CodiArticle);
                    table.ForeignKey(
                        name: "FK_Articles_Families_CodiFamilia",
                        column: x => x.CodiFamilia,
                        principalTable: "Families",
                        principalColumn: "CodiFamilia");
                });

            migrationBuilder.CreateIndex(
                name: "K_Article_CodiFamilia_CodiArticle",
                table: "Articles",
                columns: new[] { "CodiFamilia", "CodiArticle" });

            migrationBuilder.CreateIndex(
                name: "K_Article_Descripcio",
                table: "Articles",
                column: "Descripcio");

            migrationBuilder.CreateIndex(
                name: "K_Article_FK_Families",
                table: "Articles",
                column: "CodiFamilia");

            migrationBuilder.CreateIndex(
                name: "K_Article_FK_TipusIVA",
                table: "Articles",
                column: "CodiIVA");

            migrationBuilder.CreateIndex(
                name: "K_Familia_Descripcio",
                table: "Families",
                column: "Descripcio");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Families");
        }
    }
}
