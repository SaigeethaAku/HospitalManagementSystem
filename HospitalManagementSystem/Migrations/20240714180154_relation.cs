using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Doctors_Username",
                table: "Users");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Doctors_Username",
                table: "Doctors");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_Username",
                table: "Users",
                column: "Username");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Users_Username",
                table: "Doctors",
                column: "Username",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Users_Username",
                table: "Doctors");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_Username",
                table: "Users");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Doctors_Username",
                table: "Doctors",
                column: "Username");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Doctors_Username",
                table: "Users",
                column: "Username",
                principalTable: "Doctors",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
