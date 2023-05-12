using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagement.Migrations
{
    /// <inheritdoc />
    public partial class userShiftLogadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserShift_User_Userid",
                table: "UserShift");

            migrationBuilder.DropColumn(
                name: "UderId",
                table: "UserShift");

            migrationBuilder.RenameColumn(
                name: "Userid",
                table: "UserShift",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserShift_Userid",
                table: "UserShift",
                newName: "IX_UserShift_UserId");

            migrationBuilder.CreateTable(
                name: "UserDepartment",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "UserShiftLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CheckInTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOutTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserShiftId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserShiftLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserShiftLog_UserShift_UserShiftId",
                        column: x => x.UserShiftId,
                        principalTable: "UserShift",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserShiftLog_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserShiftLog_UserId",
                table: "UserShiftLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserShiftLog_UserShiftId",
                table: "UserShiftLog",
                column: "UserShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserShift_User_UserId",
                table: "UserShift",
                column: "UserId",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserShift_User_UserId",
                table: "UserShift");

            migrationBuilder.DropTable(
                name: "UserDepartment");

            migrationBuilder.DropTable(
                name: "UserShiftLog");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserShift",
                newName: "Userid");

            migrationBuilder.RenameIndex(
                name: "IX_UserShift_UserId",
                table: "UserShift",
                newName: "IX_UserShift_Userid");

            migrationBuilder.AddColumn<int>(
                name: "UderId",
                table: "UserShift",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_UserShift_User_Userid",
                table: "UserShift",
                column: "Userid",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
