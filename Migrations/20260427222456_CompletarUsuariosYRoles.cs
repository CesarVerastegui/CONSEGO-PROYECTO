using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CONSEGO.Migrations
{
    /// <inheritdoc />
    public partial class CompletarUsuariosYRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Activo", "Email", "FechaCreacion", "Nombre", "PasswordHash", "RolId" },
                values: new object[] { 3, true, "solicitante@idmtechnology.pe", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Juan Asto", "WIxV884rhWmxU8WrvxP590MIuIogAXzGmbg1zJMZXRY=", 3 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
