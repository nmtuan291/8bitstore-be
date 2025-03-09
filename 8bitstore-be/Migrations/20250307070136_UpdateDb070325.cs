using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _8bitstore_be.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb070325 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubDistrict",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubDistrict",
                table: "AspNetUsers");
        }
    }
}
