using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NIA_CRM.Data.NIACRMigration
{
    /// <inheritdoc />
    public partial class ConcurrencyProductionEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "ProductionEmails",
                type: "BLOB",
                rowVersion: true,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "ProductionEmails");
        }
    }
}
