using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _8bitstore_be.Migrations
{
    /// <inheritdoc />
    public partial class ModifyImgUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string[]>(
                name: "ImgUrl",
                table: "Products",
                type: "text[]",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImgUrl",
                table: "Products",
                type: "text",
                nullable: false,
                oldClrType: typeof(string[]),
                oldType: "text[]");
        }
    }
}
