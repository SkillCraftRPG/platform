using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCraft.Cms.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class CreateArticleHierarchyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticleHierarchy",
                schema: "Encyclopedia",
                columns: table => new
                {
                    ArticleId = table.Column<int>(type: "integer", nullable: false),
                    ArticleUid = table.Column<Guid>(type: "uuid", nullable: false),
                    CollectionId = table.Column<int>(type: "integer", nullable: false),
                    CollectionUid = table.Column<Guid>(type: "uuid", nullable: false),
                    Depth = table.Column<int>(type: "integer", nullable: false),
                    IdPath = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    SlugPath = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleHierarchy", x => x.ArticleId);
                    table.ForeignKey(
                        name: "FK_ArticleHierarchy_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalSchema: "Encyclopedia",
                        principalTable: "Articles",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleHierarchy_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalSchema: "Encyclopedia",
                        principalTable: "Collections",
                        principalColumn: "CollectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleHierarchy_ArticleUid",
                schema: "Encyclopedia",
                table: "ArticleHierarchy",
                column: "ArticleUid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleHierarchy_CollectionId",
                schema: "Encyclopedia",
                table: "ArticleHierarchy",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleHierarchy_CollectionUid",
                schema: "Encyclopedia",
                table: "ArticleHierarchy",
                column: "CollectionUid");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleHierarchy_Depth",
                schema: "Encyclopedia",
                table: "ArticleHierarchy",
                column: "Depth");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleHierarchy_IdPath",
                schema: "Encyclopedia",
                table: "ArticleHierarchy",
                column: "IdPath");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleHierarchy_SlugPath",
                schema: "Encyclopedia",
                table: "ArticleHierarchy",
                column: "SlugPath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleHierarchy",
                schema: "Encyclopedia");
        }
    }
}
