using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WOF.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChatCommandDirtyAndFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ChangedData",
                table: "ChatCommands",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ChatConversationId",
                table: "ChatCommands",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ChatCommands_ChatConversationId",
                table: "ChatCommands",
                column: "ChatConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatCommands_ChatConversations_ChatConversationId",
                table: "ChatCommands",
                column: "ChatConversationId",
                principalTable: "ChatConversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatCommands_ChatConversations_ChatConversationId",
                table: "ChatCommands");

            migrationBuilder.DropIndex(
                name: "IX_ChatCommands_ChatConversationId",
                table: "ChatCommands");

            migrationBuilder.DropColumn(
                name: "ChangedData",
                table: "ChatCommands");

            migrationBuilder.DropColumn(
                name: "ChatConversationId",
                table: "ChatCommands");
        }
    }
}
