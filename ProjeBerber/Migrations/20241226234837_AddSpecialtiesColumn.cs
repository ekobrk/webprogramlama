using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjeBerber.Migrations
{
    /// <inheritdoc />
    public partial class AddSpecialtiesColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Specialties",
                table: "Barbers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Specialties",
                table: "Barbers");
        }
    }
}
