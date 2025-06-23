using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TreatmentDateTime",
                table: "VolunteerCallsDb");

            migrationBuilder.AddColumn<DateTime>(
                name: "ResponseTime",
                table: "VolunteerCallsDb",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponseTime",
                table: "VolunteerCallsDb");

            migrationBuilder.AddColumn<DateTime>(
                name: "TreatmentDateTime",
                table: "VolunteerCallsDb",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
