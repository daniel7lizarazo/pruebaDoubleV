using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class CalculatedElementsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NombreCompleto",
                table: "Personas",
                type: "nvarchar(max)",
                nullable: true,
                computedColumnSql: "[Nombres] + ' ' + [Apellidos]",
                stored: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComputedColumnSql: "[Nombres] + [Apellidos]",
                oldStored: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NombreCompleto",
                table: "Personas",
                type: "nvarchar(max)",
                nullable: true,
                computedColumnSql: "[Nombres] + [Apellidos]",
                stored: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComputedColumnSql: "[Nombres] + ' ' + [Apellidos]",
                oldStored: true);
        }
    }
}
