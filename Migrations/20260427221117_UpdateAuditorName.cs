using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CONSEGO.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuditorName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4,
                column: "Nombre",
                value: "Pedro Castro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4,
                column: "Nombre",
                value: "Pedro Auditor");
        }
    }
}
