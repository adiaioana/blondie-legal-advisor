using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace legal_document_analyzer.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "legaldocuments",
                columns: table => new
                {
                    legaldocumentid = table.Column<Guid>(type: "uuid", nullable: false),
                    userid = table.Column<Guid>(type: "uuid", nullable: false),
                    filename = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "TEXT", nullable: false),
                    uploadedat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_legaldocuments", x => x.legaldocumentid);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userid = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: true),
                    passwordhash = table.Column<string>(type: "text", nullable: true),
                    passwordsalt = table.Column<string>(type: "text", nullable: true),
                    refreshtoken = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.userid);
                });

            migrationBuilder.CreateTable(
                name: "clauses",
                columns: table => new
                {
                    clauseid = table.Column<Guid>(type: "uuid", nullable: false),
                    documentid = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    text = table.Column<string>(type: "text", nullable: false),
                    explanation = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clauses", x => x.clauseid);
                    table.ForeignKey(
                        name: "FK_clauses_legaldocuments_documentid",
                        column: x => x.documentid,
                        principalTable: "legaldocuments",
                        principalColumn: "legaldocumentid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "documentsummaries",
                columns: table => new
                {
                    documentsummaryid = table.Column<Guid>(type: "uuid", nullable: false),
                    documentid = table.Column<Guid>(type: "uuid", nullable: false),
                    style = table.Column<int>(type: "integer", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documentsummaries", x => x.documentsummaryid);
                    table.ForeignKey(
                        name: "FK_documentsummaries_legaldocuments_documentid",
                        column: x => x.documentid,
                        principalTable: "legaldocuments",
                        principalColumn: "legaldocumentid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_clauses_documentid",
                table: "clauses",
                column: "documentid");

            migrationBuilder.CreateIndex(
                name: "IX_documentsummaries_documentid",
                table: "documentsummaries",
                column: "documentid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clauses");

            migrationBuilder.DropTable(
                name: "documentsummaries");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "legaldocuments");
        }
    }
}
