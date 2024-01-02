using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthInsurance.Server.Migrations
{
    /// <inheritdoc />
    public partial class v4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_PolicyOnEmployees_PolicyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PolicyId",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "PolicyOnEmployees",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PolicyOnEmployees_EmployeeId",
                table: "PolicyOnEmployees",
                column: "EmployeeId",
                unique: true,
                filter: "[EmployeeId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_PolicyOnEmployees_Users_EmployeeId",
                table: "PolicyOnEmployees",
                column: "EmployeeId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PolicyOnEmployees_Users_EmployeeId",
                table: "PolicyOnEmployees");

            migrationBuilder.DropIndex(
                name: "IX_PolicyOnEmployees_EmployeeId",
                table: "PolicyOnEmployees");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "PolicyOnEmployees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PolicyId",
                table: "Users",
                column: "PolicyId",
                unique: true,
                filter: "[PolicyId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_PolicyOnEmployees_PolicyId",
                table: "Users",
                column: "PolicyId",
                principalTable: "PolicyOnEmployees",
                principalColumn: "Id");
        }
    }
}
