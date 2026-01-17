using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "Personas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroIdentificacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoIdentificacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdentificacionCompleta = table.Column<string>(type: "nvarchar(max)", nullable: true, computedColumnSql: "[NumeroIdentificacion] + [TipoIdentificacion]", stored: true),
                    NombreCompleto = table.Column<string>(type: "nvarchar(max)", nullable: true, computedColumnSql: "[Nombres] + [Apellidos]", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            var spSql = @"
                CREATE PROCEDURE [dbo].[GetAllUsers]
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SELECT * FROM Usuarios;
                END";
            migrationBuilder.Sql(spSql);
            }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personas");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
