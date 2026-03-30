using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingLite.Api.Migrations
{
    /// <inheritdoc />
    public partial class transactmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    sender_id = table.Column<int>(type: "int", nullable: false),
                    recipient_id = table.Column<int>(type: "int", nullable: false),
                    sender_account_id = table.Column<int>(type: "int", nullable: false),
                    recipient_account_id = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.id);
                    table.ForeignKey(
                        name: "FK_transactions_account_recipient_account_id",
                        column: x => x.recipient_account_id,
                        principalTable: "account",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transactions_account_sender_account_id",
                        column: x => x.sender_account_id,
                        principalTable: "account",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transactions_user_recipient_id",
                        column: x => x.recipient_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transactions_user_sender_id",
                        column: x => x.sender_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_recipient_account_id",
                table: "transactions",
                column: "recipient_account_id");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_recipient_id",
                table: "transactions",
                column: "recipient_id");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_sender_account_id",
                table: "transactions",
                column: "sender_account_id");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_sender_id",
                table: "transactions",
                column: "sender_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transactions");
        }
    }
}
