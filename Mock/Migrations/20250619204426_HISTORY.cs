using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class HISTORY : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HospitalName",
                table: "CallsDb",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SentToHospital",
                table: "CallsDb",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "CallsDb",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HospitalName",
                table: "CallsDb");

            migrationBuilder.DropColumn(
                name: "SentToHospital",
                table: "CallsDb");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "CallsDb");
        }
    }
}
