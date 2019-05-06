using Microsoft.EntityFrameworkCore.Migrations;

namespace SuxrobGM_Resume.Migrations
{
    public partial class changed_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePhotoSrc",
                table: "Users",
                newName: "ProfilePhotoUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePhotoUrl",
                table: "Users",
                newName: "ProfilePhotoSrc");
        }
    }
}
