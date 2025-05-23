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
                name: "chatsessions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    userid = table.Column<string>(type: "text", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isactive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatsessions", x => x.id);
                });

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
                name: "chatmessages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    chatsessionid = table.Column<Guid>(type: "uuid", nullable: false),
                    sender = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    inputmode = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatmessages", x => x.id);
                    table.ForeignKey(
                        name: "FK_chatmessages_chatsessions_chatsessionid",
                        column: x => x.chatsessionid,
                        principalTable: "chatsessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_chatmessages_chatsessionid",
                table: "chatmessages",
                column: "chatsessionid");

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
                name: "chatmessages");

            migrationBuilder.DropTable(
                name: "clauses");

            migrationBuilder.DropTable(
                name: "documentsummaries");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "chatsessions");

            migrationBuilder.DropTable(
                name: "legaldocuments");
        }
    }
}
