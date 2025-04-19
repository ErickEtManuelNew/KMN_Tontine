using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KMN_Tontine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTransactionPaymentPromiseRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_PaymentPromises_PaymentPromiseId",
                table: "Transactions");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_PaymentPromises_PaymentPromiseId",
                table: "Transactions",
                column: "PaymentPromiseId",
                principalTable: "PaymentPromises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_PaymentPromises_PaymentPromiseId",
                table: "Transactions");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_PaymentPromises_PaymentPromiseId",
                table: "Transactions",
                column: "PaymentPromiseId",
                principalTable: "PaymentPromises",
                principalColumn: "Id");
        }
    }
}
