using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LibrarianId",
                table: "Readers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Readers_LibrarianId",
                table: "Readers",
                column: "LibrarianId");

            migrationBuilder.AddForeignKey(
                name: "FK_Readers_Librarians_LibrarianId",
                table: "Readers",
                column: "LibrarianId",
                principalTable: "Librarians",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Readers_Librarians_LibrarianId",
                table: "Readers");

            migrationBuilder.DropIndex(
                name: "IX_Readers_LibrarianId",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "LibrarianId",
                table: "Readers");
        }
    }
}
