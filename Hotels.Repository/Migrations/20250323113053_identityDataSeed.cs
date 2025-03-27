using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hotels.Repository.Migrations
{
    /// <inheritdoc />
    public partial class identityDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "33B7ED72-9434-434A-82D4-3018B018CB87", null, "Admin", "ADMIN" },
                    { "477340A8-A64A-4F6F-816F-9066D38548A6", null, "Manager", "MANAGER" },
                    { "9C07F9F6-D3B0-458A-AB7F-218AA622FA5B", null, "Guest", "GUEST" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "8716071C-1D9B-48FD-B3D0-F059C4FB8031", 0, "7e16f046-35e6-48b0-b100-523b4e93c445", "admin@gmail.com", false, "Administrator", true, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAEMYS5Oy5PIvDCq1hLJAKsaOrKGONBK7FQTVbHz0+orKsJKL9eqPb6rMvuG/v4lw3TQ==", "555337681", false, "0e968a27-079e-40e6-a70b-575489e7d506", false, "admin@gmail.com" },
                    { "87746F88-DC38-4756-924A-B95CFF3A1D8A", 0, "2cd50d9d-7b5c-4009-922f-1e64e17c80c4", "mari@gmail.com", false, "Mariam Iniashvili", true, null, "MARI@GMAIL.COM", "MARI@GMAIL.COM", "AQAAAAIAAYagAAAAEO/GlmqsiPsSLKoxZ1Lxu3Ko+0Hu9T8tAd1eLaY7Gu57AiLo6uxznzc6eKnC4aXiCg==", "551446622", false, "aca4e436-63d6-49f4-b4e5-000e040ceb3a", false, "mari@gmail.com" },
                    { "D514EDC9-94BB-416F-AF9D-7C13669689C9", 0, "3af8da75-70a6-42cc-8746-318a093f3f67", "manager@gmail.com", false, "Manager", true, null, "MANAGER@GMAIL.COM", "MANAGER@GMAIL.COM", "AQAAAAIAAYagAAAAEFYGIbLZFg0YzEE/dJMwAWB/ESVGKkWOu0Ib41JcOi0cGPMCU4FYf2hFgUfHrBg9RA==", "558558866", false, "2f84a394-80ad-44fb-b11c-25bda32fd0e5", false, "manager@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "33B7ED72-9434-434A-82D4-3018B018CB87", "8716071C-1D9B-48FD-B3D0-F059C4FB8031" },
                    { "9C07F9F6-D3B0-458A-AB7F-218AA622FA5B", "87746F88-DC38-4756-924A-B95CFF3A1D8A" },
                    { "477340A8-A64A-4F6F-816F-9066D38548A6", "D514EDC9-94BB-416F-AF9D-7C13669689C9" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "33B7ED72-9434-434A-82D4-3018B018CB87", "8716071C-1D9B-48FD-B3D0-F059C4FB8031" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9C07F9F6-D3B0-458A-AB7F-218AA622FA5B", "87746F88-DC38-4756-924A-B95CFF3A1D8A" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "477340A8-A64A-4F6F-816F-9066D38548A6", "D514EDC9-94BB-416F-AF9D-7C13669689C9" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "33B7ED72-9434-434A-82D4-3018B018CB87");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "477340A8-A64A-4F6F-816F-9066D38548A6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9C07F9F6-D3B0-458A-AB7F-218AA622FA5B");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8716071C-1D9B-48FD-B3D0-F059C4FB8031");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "87746F88-DC38-4756-924A-B95CFF3A1D8A");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "D514EDC9-94BB-416F-AF9D-7C13669689C9");
        }
    }
}
