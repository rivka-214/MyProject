using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class AddVolunteerLocationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "LocationX",
                table: "VolunteersDb",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LocationY",
                table: "VolunteersDb",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationX",
                table: "VolunteersDb");

            migrationBuilder.DropColumn(
                name: "LocationY",
                table: "VolunteersDb");
        }
    }
}
