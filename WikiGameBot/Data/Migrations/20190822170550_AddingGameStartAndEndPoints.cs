using Microsoft.EntityFrameworkCore.Migrations;

namespace WikiGameBot.Data.Migrations
{
    public partial class AddingGameStartAndEndPoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EndingUrl",
                table: "Games",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StartingUrl",
                table: "Games",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndingUrl",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "StartingUrl",
                table: "Games");
        }
    }
}
