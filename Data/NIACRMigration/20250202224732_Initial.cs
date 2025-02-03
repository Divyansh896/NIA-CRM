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
                name: "Members",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MemberName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    MemberSize = table.Column<int>(type: "INTEGER", nullable: false),
                    WebsiteUrl = table.Column<string>(type: "TEXT", nullable: true),
                    JoinDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.ID);
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
                name: "NAICSCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Label = table.Column<string>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NAICSCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionEmails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmailType = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Subject = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Body = table.Column<string>(type: "TEXT", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionEmails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false),
                    AddressLine1 = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    AddressLine2 = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    City = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    StateProvince = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Country = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Members_MemberId",
                        column: x => x.MemberId,
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
                    Canceled = table.Column<bool>(type: "INTEGER", nullable: false),
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
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    Department = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    LinkedInUrl = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    IsVip = table.Column<bool>(type: "INTEGER", nullable: false),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contacts_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberLogos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<byte[]>(type: "BLOB", nullable: true),
                    MimeType = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    MemberID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberLogos", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MemberLogos_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberNote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberNote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberNote_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemebrThumbnails",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<byte[]>(type: "BLOB", nullable: true),
                    MimeType = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    MemberID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemebrThumbnails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MemebrThumbnails_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
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
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opportunities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Opportunities_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemberMembershipTypes",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false),
                    MembershipTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberMembershipTypes", x => new { x.MemberId, x.MembershipTypeId });
                    table.ForeignKey(
                        name: "FK_MemberMembershipTypes_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberMembershipTypes_MembershipTypes_MembershipTypeId",
                        column: x => x.MembershipTypeId,
                        principalTable: "MembershipTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IndustryNAICSCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false),
                    NAICSCodeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndustryNAICSCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndustryNAICSCodes_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IndustryNAICSCodes_NAICSCode_NAICSCodeId",
                        column: x => x.NAICSCodeId,
                        principalTable: "NAICSCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContactNote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContactId = table.Column<int>(type: "INTEGER", nullable: false),
                    Note = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactNote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactNote_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Interactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false),
                    ContactId = table.Column<int>(type: "INTEGER", nullable: false),
                    InteractionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OpportunityId = table.Column<int>(type: "INTEGER", nullable: true),
                    InteractionNotes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Interactions_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Interactions_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Interactions_Opportunities_OpportunityId",
                        column: x => x.OpportunityId,
                        principalTable: "Opportunities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_MemberId",
                table: "Addresses",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Cancellations_MemberID",
                table: "Cancellations",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_ContactNote_ContactId",
                table: "ContactNote",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_MemberId",
                table: "Contacts",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_IndustryNAICSCodes_MemberId",
                table: "IndustryNAICSCodes",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_IndustryNAICSCodes_NAICSCodeId",
                table: "IndustryNAICSCodes",
                column: "NAICSCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_ContactId",
                table: "Interactions",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_MemberId",
                table: "Interactions",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_OpportunityId",
                table: "Interactions",
                column: "OpportunityId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberLogos_MemberID",
                table: "MemberLogos",
                column: "MemberID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberMembershipTypes_MembershipTypeId",
                table: "MemberMembershipTypes",
                column: "MembershipTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberNote_MemberId",
                table: "MemberNote",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemebrThumbnails_MemberID",
                table: "MemebrThumbnails",
                column: "MemberID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Opportunities_MemberId",
                table: "Opportunities",
                column: "MemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Cancellations");

            migrationBuilder.DropTable(
                name: "ContactNote");

            migrationBuilder.DropTable(
                name: "IndustryNAICSCodes");

            migrationBuilder.DropTable(
                name: "Interactions");

            migrationBuilder.DropTable(
                name: "MemberLogos");

            migrationBuilder.DropTable(
                name: "MemberMembershipTypes");

            migrationBuilder.DropTable(
                name: "MemberNote");

            migrationBuilder.DropTable(
                name: "MemebrThumbnails");

            migrationBuilder.DropTable(
                name: "ProductionEmails");

            migrationBuilder.DropTable(
                name: "NAICSCode");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Opportunities");

            migrationBuilder.DropTable(
                name: "MembershipTypes");

            migrationBuilder.DropTable(
                name: "Members");
        }
    }
}
