using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GtpApi.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class AddedChatSetupMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChatSetupMessage",
                table: "ChatInfos",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatSetupMessage",
                table: "ChatInfos");
        }
    }
}
