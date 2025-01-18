using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NIA_CRM.Data.NIACRMigration
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContactName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Department = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    EMail = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    LinkedinUrl = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    IsVIP = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Industries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IndustryName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Industries", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MembershipTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TypeName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    TypeDescr = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrganizationName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    OrganizationSize = table.Column<int>(type: "INTEGER", nullable: true),
                    OrganizationWeb = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    IndustryID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Organizations_Industries_IndustryID",
                        column: x => x.IndustryID,
                        principalTable: "Industries",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContactOrganizations",
                columns: table => new
                {
                    ContactID = table.Column<int>(type: "INTEGER", nullable: false),
                    OrganizationID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactOrganizations", x => new { x.ContactID, x.OrganizationID });
                    table.ForeignKey(
                        name: "FK_ContactOrganizations_Contacts_ContactID",
                        column: x => x.ContactID,
                        principalTable: "Contacts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactOrganizations_Organizations_OrganizationID",
                        column: x => x.OrganizationID,
                        principalTable: "Organizations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MemberName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    JoinDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    StandingStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    OrganizationID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Members_Organizations_OrganizationID",
                        column: x => x.OrganizationID,
                        principalTable: "Organizations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Opportunities",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OpportunityName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    OpportunityDescr = table.Column<string>(type: "TEXT", nullable: true),
                    OpportunityStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    OrganizationID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opportunities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Opportunities_Organizations_OrganizationID",
                        column: x => x.OrganizationID,
                        principalTable: "Organizations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationCodes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CodeOrganization = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    OrganizationID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationCodes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrganizationCodes_Organizations_OrganizationID",
                        column: x => x.OrganizationID,
                        principalTable: "Organizations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AddressLineOne = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    AddressLineTwo = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    City = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    StateProvince = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "TEXT", maxLength: 6, nullable: true),
                    Country = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    MemberID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Addresses_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cancellations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CancellationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CancellationNote = table.Column<string>(type: "TEXT", nullable: true),
                    MemberID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cancellations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Cancellations_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemberMembershipTypes",
                columns: table => new
                {
                    MemberID = table.Column<int>(type: "INTEGER", nullable: false),
                    MembershipTypeID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberMembershipTypes", x => new { x.MemberID, x.MembershipTypeID });
                    table.ForeignKey(
                        name: "FK_MemberMembershipTypes_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberMembershipTypes_MembershipTypes_MembershipTypeID",
                        column: x => x.MembershipTypeID,
                        principalTable: "MembershipTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Interactions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InteractionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    InteractionNote = table.Column<string>(type: "TEXT", nullable: true),
                    ContactID = table.Column<int>(type: "INTEGER", nullable: false),
                    MemberID = table.Column<int>(type: "INTEGER", nullable: false),
                    OpportunityID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interactions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Interactions_Contacts_ContactID",
                        column: x => x.ContactID,
                        principalTable: "Contacts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Interactions_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Interactions_Opportunities_OpportunityID",
                        column: x => x.OpportunityID,
                        principalTable: "Opportunities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_MemberID",
                table: "Addresses",
                column: "MemberID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cancellations_MemberID",
                table: "Cancellations",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_ContactOrganizations_OrganizationID",
                table: "ContactOrganizations",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_ContactID",
                table: "Interactions",
                column: "ContactID");

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_MemberID",
                table: "Interactions",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_OpportunityID",
                table: "Interactions",
                column: "OpportunityID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberMembershipTypes_MembershipTypeID",
                table: "MemberMembershipTypes",
                column: "MembershipTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Members_OrganizationID",
                table: "Members",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_Opportunities_OrganizationID",
                table: "Opportunities",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationCodes_OrganizationID",
                table: "OrganizationCodes",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_IndustryID",
                table: "Organizations",
                column: "IndustryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Cancellations");

            migrationBuilder.DropTable(
                name: "ContactOrganizations");

            migrationBuilder.DropTable(
                name: "Interactions");

            migrationBuilder.DropTable(
                name: "MemberMembershipTypes");

            migrationBuilder.DropTable(
                name: "OrganizationCodes");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Opportunities");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "MembershipTypes");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Industries");
        }
    }
}
