using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KMN_Tontine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentPromiseIdToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentPromiseId",
                table: "Transactions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PaymentPromiseId",
                table: "Transactions",
                column: "PaymentPromiseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_PaymentPromises_PaymentPromiseId",
                table: "Transactions",
                column: "PaymentPromiseId",
                principalTable: "PaymentPromises",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_PaymentPromises_PaymentPromiseId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_PaymentPromiseId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "PaymentPromiseId",
                table: "Transactions");
        }
    }
}
