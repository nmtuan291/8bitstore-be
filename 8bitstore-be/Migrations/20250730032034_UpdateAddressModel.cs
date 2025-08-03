using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _8bitstore_be.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAddressModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Division",
                table: "Addresses",
                newName: "Ward");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ward",
                table: "Addresses",
                newName: "Division");
        }
    }
}
