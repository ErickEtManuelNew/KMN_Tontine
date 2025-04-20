using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KMN_Tontine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentPromiseReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "PaymentPromises",
                type: "TEXT",
                maxLength: 7,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reference",
                table: "PaymentPromises");
        }
    }
}
