using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KMN_Tontine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TontineAccount_MemberNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_AspNetUsers_MemberId",
                table: "Accounts");

            migrationBuilder.AlterColumn<string>(
                name: "MemberId",
                table: "Accounts",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_AspNetUsers_MemberId",
                table: "Accounts",
                column: "MemberId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_AspNetUsers_MemberId",
                table: "Accounts");

            migrationBuilder.AlterColumn<string>(
                name: "MemberId",
                table: "Accounts",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_AspNetUsers_MemberId",
                table: "Accounts",
                column: "MemberId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
