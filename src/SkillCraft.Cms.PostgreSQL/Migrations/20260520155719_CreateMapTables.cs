using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkillCraft.Cms.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class CreateMapTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Maps",
                schema: "Encyclopedia",
                columns: table => new
                {
                    MapId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Key = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    KeyNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Width = table.Column<int>(type: "integer", nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: false),
                    Source = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    StreamId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.MapId);
                });

            migrationBuilder.CreateTable(
                name: "ArticleMaps",
                schema: "Encyclopedia",
                columns: table => new
                {
                    ArticleId = table.Column<int>(type: "integer", nullable: false),
                    MapId = table.Column<int>(type: "integer", nullable: false),
                    ArticleUid = table.Column<Guid>(type: "uuid", nullable: false),
                    MapUid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleMaps", x => new { x.ArticleId, x.MapId });
                    table.ForeignKey(
                        name: "FK_ArticleMaps_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalSchema: "Encyclopedia",
                        principalTable: "Articles",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleMaps_Maps_MapId",
                        column: x => x.MapId,
                        principalSchema: "Encyclopedia",
                        principalTable: "Maps",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleMaps_ArticleId",
                schema: "Encyclopedia",
                table: "ArticleMaps",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleMaps_ArticleUid",
                schema: "Encyclopedia",
                table: "ArticleMaps",
                column: "ArticleUid");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleMaps_MapId",
                schema: "Encyclopedia",
                table: "ArticleMaps",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleMaps_MapUid",
                schema: "Encyclopedia",
                table: "ArticleMaps",
                column: "MapUid");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_CreatedBy",
                schema: "Encyclopedia",
                table: "Maps",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_CreatedOn",
                schema: "Encyclopedia",
                table: "Maps",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_Id",
                schema: "Encyclopedia",
                table: "Maps",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Maps_IsPublished",
                schema: "Encyclopedia",
                table: "Maps",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_Key",
                schema: "Encyclopedia",
                table: "Maps",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_KeyNormalized",
                schema: "Encyclopedia",
                table: "Maps",
                column: "KeyNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Maps_StreamId",
                schema: "Encyclopedia",
                table: "Maps",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Maps_Title",
                schema: "Encyclopedia",
                table: "Maps",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_UpdatedBy",
                schema: "Encyclopedia",
                table: "Maps",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_UpdatedOn",
                schema: "Encyclopedia",
                table: "Maps",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_Version",
                schema: "Encyclopedia",
                table: "Maps",
                column: "Version");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleMaps",
                schema: "Encyclopedia");

            migrationBuilder.DropTable(
                name: "Maps",
                schema: "Encyclopedia");
        }
    }
}
