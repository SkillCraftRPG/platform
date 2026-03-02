using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCraft.Cms.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class CreateSpellCategoryAssociationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpellCategoryAssociations",
                schema: "Rules",
                columns: table => new
                {
                    SpellId = table.Column<int>(type: "integer", nullable: false),
                    SpellCategoryId = table.Column<int>(type: "integer", nullable: false),
                    SpellUid = table.Column<Guid>(type: "uuid", nullable: false),
                    SpellCategoryUid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpellCategoryAssociations", x => new { x.SpellId, x.SpellCategoryId });
                    table.ForeignKey(
                        name: "FK_SpellCategoryAssociations_SpellCategories_SpellCategoryId",
                        column: x => x.SpellCategoryId,
                        principalSchema: "Rules",
                        principalTable: "SpellCategories",
                        principalColumn: "SpellCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpellCategoryAssociations_Spells_SpellId",
                        column: x => x.SpellId,
                        principalSchema: "Rules",
                        principalTable: "Spells",
                        principalColumn: "SpellId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpellCategoryAssociations_SpellCategoryId",
                schema: "Rules",
                table: "SpellCategoryAssociations",
                column: "SpellCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SpellCategoryAssociations_SpellCategoryUid",
                schema: "Rules",
                table: "SpellCategoryAssociations",
                column: "SpellCategoryUid");

            migrationBuilder.CreateIndex(
                name: "IX_SpellCategoryAssociations_SpellId",
                schema: "Rules",
                table: "SpellCategoryAssociations",
                column: "SpellId");

            migrationBuilder.CreateIndex(
                name: "IX_SpellCategoryAssociations_SpellUid",
                schema: "Rules",
                table: "SpellCategoryAssociations",
                column: "SpellUid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpellCategoryAssociations",
                schema: "Rules");
        }
    }
}
