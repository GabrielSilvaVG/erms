using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventra.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCreatedAtLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "$2a$11$A3eucjdbTxCixx5UGEQwPetdr8T0OFvUxBBhv7XBTnpMMG3r32F9m" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 12, 2, 43, 5, 300, DateTimeKind.Utc).AddTicks(1660), "$2a$11$Lw.X1WmVmKF.4KM4o3QLYupqgUNR2aug53hFZyNB7yjbmLOBZk1AG" });
        }
    }
}
