using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OdapApi.Migrations
{
    public partial class RAngleApproveMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateReceived",
                table: "RAngles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EmailToSendTo",
                table: "RAngles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "RAngles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LocationFrom",
                table: "RAngles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationTo",
                table: "RAngles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateReceived",
                table: "RAngles");

            migrationBuilder.DropColumn(
                name: "EmailToSendTo",
                table: "RAngles");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "RAngles");

            migrationBuilder.DropColumn(
                name: "LocationFrom",
                table: "RAngles");

            migrationBuilder.DropColumn(
                name: "LocationTo",
                table: "RAngles");
        }
    }
}
