using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektDb.Migrations
{
    /// <inheritdoc />
    public partial class Poprawionemodele4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MyProperty",
                table: "Logs",
                newName: "DeleteDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeleteDate",
                table: "Logs",
                newName: "MyProperty");
        }
    }
}
