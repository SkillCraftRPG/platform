using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkillCraft.Cms.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class CreateScriptLanguageTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Scripts",
                schema: "Rules",
                columns: table => new
                {
                    ScriptId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SlugNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    MetaDescription = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    Summary = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    HtmlContent = table.Column<string>(type: "text", nullable: true),
                    StreamId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scripts", x => x.ScriptId);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                schema: "Rules",
                columns: table => new
                {
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SlugNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ScriptId = table.Column<int>(type: "integer", nullable: true),
                    ScriptUid = table.Column<Guid>(type: "uuid", nullable: true),
                    TypicalSpeakers = table.Column<string>(type: "text", nullable: true),
                    MetaDescription = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    Summary = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    HtmlContent = table.Column<string>(type: "text", nullable: true),
                    StreamId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.LanguageId);
                    table.ForeignKey(
                        name: "FK_Languages_Scripts_ScriptId",
                        column: x => x.ScriptId,
                        principalSchema: "Rules",
                        principalTable: "Scripts",
                        principalColumn: "ScriptId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Languages_CreatedBy",
                schema: "Rules",
                table: "Languages",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_CreatedOn",
                schema: "Rules",
                table: "Languages",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Id",
                schema: "Rules",
                table: "Languages",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_IsPublished",
                schema: "Rules",
                table: "Languages",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Name",
                schema: "Rules",
                table: "Languages",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_ScriptId",
                schema: "Rules",
                table: "Languages",
                column: "ScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_ScriptUid",
                schema: "Rules",
                table: "Languages",
                column: "ScriptUid");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Slug",
                schema: "Rules",
                table: "Languages",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_SlugNormalized",
                schema: "Rules",
                table: "Languages",
                column: "SlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_StreamId",
                schema: "Rules",
                table: "Languages",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_UpdatedBy",
                schema: "Rules",
                table: "Languages",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_UpdatedOn",
                schema: "Rules",
                table: "Languages",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Version",
                schema: "Rules",
                table: "Languages",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_CreatedBy",
                schema: "Rules",
                table: "Scripts",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_CreatedOn",
                schema: "Rules",
                table: "Scripts",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_Id",
                schema: "Rules",
                table: "Scripts",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_IsPublished",
                schema: "Rules",
                table: "Scripts",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_Name",
                schema: "Rules",
                table: "Scripts",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_Slug",
                schema: "Rules",
                table: "Scripts",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_SlugNormalized",
                schema: "Rules",
                table: "Scripts",
                column: "SlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_StreamId",
                schema: "Rules",
                table: "Scripts",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_UpdatedBy",
                schema: "Rules",
                table: "Scripts",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_UpdatedOn",
                schema: "Rules",
                table: "Scripts",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_Version",
                schema: "Rules",
                table: "Scripts",
                column: "Version");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Languages",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "Scripts",
                schema: "Rules");
        }
    }
}
