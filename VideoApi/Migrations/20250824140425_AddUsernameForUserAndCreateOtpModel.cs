using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUsernameForUserAndCreateOtpModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "User",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "User");
        }
    }
}
