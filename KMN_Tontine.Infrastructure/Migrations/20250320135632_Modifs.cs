using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KMN_Tontine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Modifs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MembreId",
                table: "Comptes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nom",
                table: "Comptes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Comptes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MembreId",
                table: "Comptes");

            migrationBuilder.DropColumn(
                name: "Nom",
                table: "Comptes");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Comptes");
        }
    }
}
