using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NIA_CRM.Data.NIACRMigration
{
    /// <inheritdoc />
    public partial class Auditable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ProductionEmails",
                type: "TEXT",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ProductionEmails",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ProductionEmails",
                type: "TEXT",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "ProductionEmails",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ProductionEmails");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ProductionEmails");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ProductionEmails");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "ProductionEmails");
        }
    }
}
