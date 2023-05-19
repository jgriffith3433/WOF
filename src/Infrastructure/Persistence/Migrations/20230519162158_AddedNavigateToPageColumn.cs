using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WOF.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedNavigateToPageColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NavigateToPage",
                table: "ChatCommands",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NavigateToPage",
                table: "ChatCommands");
        }
    }
}
