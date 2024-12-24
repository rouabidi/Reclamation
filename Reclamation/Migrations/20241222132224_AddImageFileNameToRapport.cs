using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reclamation.Migrations
{
    /// <inheritdoc />
    public partial class AddImageFileNameToRapport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "Rapports",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "Rapports");
        }
    }
}
