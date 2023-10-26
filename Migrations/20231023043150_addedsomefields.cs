using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediPortal_AuthService.Migrations
{
    /// <inheritdoc />
    public partial class addedsomefields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HospitalName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "speciality",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HospitalName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "speciality",
                table: "AspNetUsers");
        }
    }
}
