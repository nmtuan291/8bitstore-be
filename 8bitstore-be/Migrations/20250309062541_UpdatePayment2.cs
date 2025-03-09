using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _8bitstore_be.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePayment2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentVnPays",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    TransactionNo = table.Column<string>(type: "text", nullable: false),
                    BankCode = table.Column<string>(type: "text", nullable: false),
                    BankTranNo = table.Column<string>(type: "text", nullable: false),
                    CardType = table.Column<string>(type: "text", nullable: false),
                    PayDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ResponseCode = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    PaymentType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentVnPays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentVnPays_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVnPays_UserId",
                table: "PaymentVnPays",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentVnPays");
        }
    }
}
