using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace artshare_server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaypalCreator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaypalClientId",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaypalSercretKey",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaypalClientId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "PaypalSercretKey",
                table: "Accounts");
        }
    }
}
