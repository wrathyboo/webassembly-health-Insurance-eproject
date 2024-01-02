using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthInsurance.Server.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
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

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "PolicyOnEmployees");

            migrationBuilder.AlterColumn<int>(
                name: "PolicyId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_PolicyOnEmployees_PolicyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PolicyId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "PolicyId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "PolicyOnEmployees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PolicyId",
                table: "Users",
                column: "PolicyId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_PolicyOnEmployees_PolicyId",
                table: "Users",
                column: "PolicyId",
                principalTable: "PolicyOnEmployees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
