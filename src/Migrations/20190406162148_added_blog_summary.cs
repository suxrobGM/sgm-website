using Microsoft.EntityFrameworkCore.Migrations;

namespace SuxrobGM_Website.Migrations
{
    public partial class added_blog_summary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Blogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Blogs");
        }
    }
}
