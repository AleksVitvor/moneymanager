using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddPeriods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransactionPeriodId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TransactionPeriods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionPeriods", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionPeriodId",
                table: "Transactions",
                column: "TransactionPeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionPeriods_TransactionPeriodId",
                table: "Transactions",
                column: "TransactionPeriodId",
                principalTable: "TransactionPeriods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionPeriods_TransactionPeriodId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "TransactionPeriods");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_TransactionPeriodId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TransactionPeriodId",
                table: "Transactions");
        }
    }
}
