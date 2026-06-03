using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CONSEGO.Migrations
{
    /// <inheritdoc />
    public partial class ImplementacionAuditoriaCompleta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Plataformas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Plataformas",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Plataformas",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Plataformas",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Descripcion",
                value: "Acceso total");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Descripcion",
                value: "Revisa solicitudes");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "Descripcion",
                value: "Crea solicitudes");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "Descripcion",
                value: "Implementa accesos");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Descripcion", "Nombre" },
                values: new object[] { 5, "Solo lectura y revisión de logs", "Auditor" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "Nombre",
                value: "Ana García");

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Activo", "Email", "FechaCreacion", "Nombre", "PasswordHash", "RolId" },
                values: new object[] { 4, true, "auditor@idmtechnology.pe", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pedro Auditor", "WIxV884rhWmxU8WrvxP590MIuIogAXzGmbg1zJMZXRY=", 5 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.InsertData(
                table: "Plataformas",
                columns: new[] { "Id", "Activa", "Criticidad", "Nombre", "Tipo" },
                values: new object[,]
                {
                    { 4, true, "Media", "Microsoft 365", "Cloud" },
                    { 5, true, "Media", "Cloudflare", "Cloud" },
                    { 6, true, "Baja", "WordPress", "App" },
                    { 7, true, "Media", "GoDaddy", "Cloud" }
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Descripcion",
                value: "Administrador del sistema con acceso total");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Descripcion",
                value: "Analista de seguridad que revisa solicitudes");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "Descripcion",
                value: "Usuario que crea solicitudes de acceso");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "Descripcion",
                value: "Equipo de infraestructura que implementa accesos aprobados");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "Nombre",
                value: "Ana García (Analista)");

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Activo", "Email", "FechaCreacion", "Nombre", "PasswordHash", "RolId" },
                values: new object[] { 3, true, "solicitante@idmtechnology.pe", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Carlos López (Solicitante)", "WIxV884rhWmxU8WrvxP590MIuIogAXzGmbg1zJMZXRY=", 3 });
        }
    }
}
