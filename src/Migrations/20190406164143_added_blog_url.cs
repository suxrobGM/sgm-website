using Microsoft.EntityFrameworkCore.Migrations;

namespace SuxrobGM_Resume.Migrations
{
    public partial class added_blog_url : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Blogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Blogs");
        }
    }
}
