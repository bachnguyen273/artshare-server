using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace artshare_server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveWatermarkId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WatermarkId",
                table: "Artworks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WatermarkId",
                table: "Artworks",
                type: "int",
                nullable: true);
        }
    }
}
