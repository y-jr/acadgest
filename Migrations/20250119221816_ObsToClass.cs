using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace acadgest.Migrations
{
    /// <inheritdoc />
    public partial class ObsToClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Obs",
                table: "Classes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Obs",
                table: "Classes");
        }
    }
}
