using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myfinance_web_netcore.Migrations
{
    public partial class UpdatedSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanoContas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<int>(type: "INTEGER", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanoContas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Historico = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Data = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PlanoContasId = table.Column<int>(type: "INTEGER", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transacoes_PlanoContas_PlanoContasId",
                        column: x => x.PlanoContasId,
                        principalTable: "PlanoContas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "PlanoContas",
                columns: new[] { "Id", "Codigo", "DataCriacao", "Descricao", "Tipo" },
                values: new object[] { 1, 1, new DateTime(2025, 7, 5, 12, 46, 56, 146, DateTimeKind.Local).AddTicks(5325), "Combustível", 0 });

            migrationBuilder.InsertData(
                table: "PlanoContas",
                columns: new[] { "Id", "Codigo", "DataCriacao", "Descricao", "Tipo" },
                values: new object[] { 2, 2, new DateTime(2025, 7, 5, 12, 46, 56, 146, DateTimeKind.Local).AddTicks(5326), "Supermercado", 0 });

            migrationBuilder.InsertData(
                table: "PlanoContas",
                columns: new[] { "Id", "Codigo", "DataCriacao", "Descricao", "Tipo" },
                values: new object[] { 3, 3, new DateTime(2025, 7, 5, 12, 46, 56, 146, DateTimeKind.Local).AddTicks(5328), "Alimentação", 0 });

            migrationBuilder.InsertData(
                table: "PlanoContas",
                columns: new[] { "Id", "Codigo", "DataCriacao", "Descricao", "Tipo" },
                values: new object[] { 4, 4, new DateTime(2025, 7, 5, 12, 46, 56, 146, DateTimeKind.Local).AddTicks(5329), "IPTU", 0 });

            migrationBuilder.InsertData(
                table: "PlanoContas",
                columns: new[] { "Id", "Codigo", "DataCriacao", "Descricao", "Tipo" },
                values: new object[] { 5, 5, new DateTime(2025, 7, 5, 12, 46, 56, 146, DateTimeKind.Local).AddTicks(5330), "IPVA", 0 });

            migrationBuilder.InsertData(
                table: "PlanoContas",
                columns: new[] { "Id", "Codigo", "DataCriacao", "Descricao", "Tipo" },
                values: new object[] { 6, 6, new DateTime(2025, 7, 5, 12, 46, 56, 146, DateTimeKind.Local).AddTicks(5331), "Salário", 1 });

            migrationBuilder.InsertData(
                table: "PlanoContas",
                columns: new[] { "Id", "Codigo", "DataCriacao", "Descricao", "Tipo" },
                values: new object[] { 7, 7, new DateTime(2025, 7, 5, 12, 46, 56, 146, DateTimeKind.Local).AddTicks(5332), "Escola", 0 });

            migrationBuilder.InsertData(
                table: "PlanoContas",
                columns: new[] { "Id", "Codigo", "DataCriacao", "Descricao", "Tipo" },
                values: new object[] { 8, 8, new DateTime(2025, 7, 5, 12, 46, 56, 146, DateTimeKind.Local).AddTicks(5333), "Financiamento de Carro", 0 });

            migrationBuilder.InsertData(
                table: "Transacoes",
                columns: new[] { "Id", "Data", "DataCriacao", "Historico", "PlanoContasId", "Valor" },
                values: new object[] { 1, new DateTime(2025, 7, 5, 13, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 5, 12, 46, 56, 146, DateTimeKind.Local).AddTicks(5437), "Supermercado", 2, 450.00m });

            migrationBuilder.InsertData(
                table: "Transacoes",
                columns: new[] { "Id", "Data", "DataCriacao", "Historico", "PlanoContasId", "Valor" },
                values: new object[] { 2, new DateTime(2025, 6, 23, 20, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 5, 12, 46, 56, 146, DateTimeKind.Local).AddTicks(5440), "Jantar", 3, 250.00m });

            migrationBuilder.InsertData(
                table: "Transacoes",
                columns: new[] { "Id", "Data", "DataCriacao", "Historico", "PlanoContasId", "Valor" },
                values: new object[] { 3, new DateTime(2025, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 5, 12, 46, 56, 146, DateTimeKind.Local).AddTicks(5441), "Salário", 6, 2000.00m });

            migrationBuilder.InsertData(
                table: "Transacoes",
                columns: new[] { "Id", "Data", "DataCriacao", "Historico", "PlanoContasId", "Valor" },
                values: new object[] { 4, new DateTime(2025, 7, 1, 8, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 5, 12, 46, 56, 146, DateTimeKind.Local).AddTicks(5443), "Mensalidade Escola", 7, 870.00m });

            migrationBuilder.CreateIndex(
                name: "IX_PlanoContas_Codigo",
                table: "PlanoContas",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transacoes_PlanoContasId",
                table: "Transacoes",
                column: "PlanoContasId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transacoes");

            migrationBuilder.DropTable(
                name: "PlanoContas");
        }
    }
}
