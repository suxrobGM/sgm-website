using Microsoft.EntityFrameworkCore.Migrations;

namespace SuxrobGM_Resume.Migrations
{
    public partial class add_cover_photo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverPhotoUrl",
                table: "Blogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverPhotoUrl",
                table: "Blogs");
        }
    }
}
