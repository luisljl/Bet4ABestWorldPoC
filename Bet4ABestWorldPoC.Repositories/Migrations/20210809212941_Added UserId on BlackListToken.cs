using Microsoft.EntityFrameworkCore.Migrations;

namespace Bet4ABestWorldPoC.Repositories.Migrations
{
    public partial class AddedUserIdonBlackListToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "BlackListToken",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BlackListToken");
        }
    }
}
