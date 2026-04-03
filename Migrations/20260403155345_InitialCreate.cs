using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CONSEGO.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Plataformas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tipo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Criticidad = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Activa = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plataformas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SolicitudesAcceso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UsuarioSolicitanteId = table.Column<int>(type: "int", nullable: false),
                    PlataformaId = table.Column<int>(type: "int", nullable: false),
                    TipoAcceso = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Justificacion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AnalistaId = table.Column<int>(type: "int", nullable: true),
                    ObservacionesSeguridad = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaDecision = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MotivoRechazo = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudesAcceso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitudesAcceso_Plataformas_PlataformaId",
                        column: x => x.PlataformaId,
                        principalTable: "Plataformas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudesAcceso_Usuarios_AnalistaId",
                        column: x => x.AnalistaId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudesAcceso_Usuarios_UsuarioSolicitanteId",
                        column: x => x.UsuarioSolicitanteId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Plataformas",
                columns: new[] { "Id", "Activa", "Criticidad", "Nombre", "Tipo" },
                values: new object[,]
                {
                    { 1, true, "Alta", "GitHub Organization", "Cloud" },
                    { 2, true, "Alta", "AWS", "Cloud" },
                    { 3, true, "Alta", "Azure", "Cloud" },
                    { 4, true, "Media", "Microsoft 365", "Cloud" },
                    { 5, true, "Media", "Cloudflare", "Cloud" },
                    { 6, true, "Baja", "WordPress", "App" },
                    { 7, true, "Media", "GoDaddy", "Cloud" },
                    { 8, true, "Alta", "VMs On-Premise", "Infra" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Administrador del sistema con acceso total", "Admin" },
                    { 2, "Analista de seguridad que revisa solicitudes", "AnalistaSeguridad" },
                    { 3, "Usuario que crea solicitudes de acceso", "Solicitante" },
                    { 4, "Equipo de infraestructura que implementa accesos aprobados", "Infra" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Activo", "Email", "FechaCreacion", "Nombre", "PasswordHash", "RolId" },
                values: new object[,]
                {
                    { 1, true, "admin@idmtechnology.pe", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Administrador", "WIxV884rhWmxU8WrvxP590MIuIogAXzGmbg1zJMZXRY=", 1 },
                    { 2, true, "analista@idmtechnology.pe", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ana García (Analista)", "WIxV884rhWmxU8WrvxP590MIuIogAXzGmbg1zJMZXRY=", 2 },
                    { 3, true, "solicitante@idmtechnology.pe", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Carlos López (Solicitante)", "WIxV884rhWmxU8WrvxP590MIuIogAXzGmbg1zJMZXRY=", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plataformas_Nombre",
                table: "Plataformas",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesAcceso_AnalistaId",
                table: "SolicitudesAcceso",
                column: "AnalistaId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesAcceso_Codigo",
                table: "SolicitudesAcceso",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesAcceso_PlataformaId",
                table: "SolicitudesAcceso",
                column: "PlataformaId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesAcceso_UsuarioSolicitanteId",
                table: "SolicitudesAcceso",
                column: "UsuarioSolicitanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolId",
                table: "Usuarios",
                column: "RolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolicitudesAcceso");

            migrationBuilder.DropTable(
                name: "Plataformas");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
