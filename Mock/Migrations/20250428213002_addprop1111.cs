using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class addprop1111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CallsDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationX = table.Column<double>(type: "float", nullable: false),
                    LocationY = table.Column<double>(type: "float", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrgencyLevel = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    numVolanteer = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallsDb", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastNAme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gmail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersDb", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VolunteersDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteersDb", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VolunteerCallsDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CallsId = table.Column<int>(type: "int", nullable: false),
                    VolunteerId = table.Column<int>(type: "int", nullable: false),
                    TreatmentDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerCallsDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VolunteerCallsDb_CallsDb_CallsId",
                        column: x => x.CallsId,
                        principalTable: "CallsDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VolunteerCallsDb_VolunteersDb_VolunteerId",
                        column: x => x.VolunteerId,
                        principalTable: "VolunteersDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerCallsDb_CallsId",
                table: "VolunteerCallsDb",
                column: "CallsId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerCallsDb_VolunteerId",
                table: "VolunteerCallsDb",
                column: "VolunteerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersDb");

            migrationBuilder.DropTable(
                name: "VolunteerCallsDb");

            migrationBuilder.DropTable(
                name: "CallsDb");

            migrationBuilder.DropTable(
                name: "VolunteersDb");
        }
    }
}
