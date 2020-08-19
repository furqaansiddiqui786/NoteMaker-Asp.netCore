using Microsoft.EntityFrameworkCore.Migrations;

namespace NoteMaker.Data.Migrations
{
    public partial class AddUserIdToNotesDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Notes",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Notes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
