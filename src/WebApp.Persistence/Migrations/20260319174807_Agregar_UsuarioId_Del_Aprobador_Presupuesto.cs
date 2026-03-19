using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Agregar_UsuarioId_Del_Aprobador_Presupuesto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsuarioId_Aprobador_Combustible",
                table: "comisiones",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsuarioId_Aprobador_Combustible",
                table: "comisiones");
        }
    }
}
