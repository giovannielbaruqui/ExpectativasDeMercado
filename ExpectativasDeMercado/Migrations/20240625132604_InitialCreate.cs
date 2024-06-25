using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpectativasDeMercado.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MarketExpectations",
                columns: table => new
                {
                    Indicador = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataReferencia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Media = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Mediana = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DesvioPadrao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Minimo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Maximo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumeroRespondentes = table.Column<int>(type: "int", nullable: false),
                    BaseCalculo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketExpectations", x => new { x.Indicador, x.Data });
                });

            migrationBuilder.CreateTable(
                name: "SelicExpectations",
                columns: table => new
                {
                    Indicador = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Reuniao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Media = table.Column<double>(type: "float", nullable: false),
                    Mediana = table.Column<double>(type: "float", nullable: false),
                    DesvioPadrao = table.Column<double>(type: "float", nullable: false),
                    Minimo = table.Column<double>(type: "float", nullable: false),
                    Maximo = table.Column<double>(type: "float", nullable: false),
                    NumeroRespondentes = table.Column<int>(type: "int", nullable: false),
                    BaseCalculo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelicExpectations", x => new { x.Indicador, x.Data });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarketExpectations");

            migrationBuilder.DropTable(
                name: "SelicExpectations");
        }
    }
}
