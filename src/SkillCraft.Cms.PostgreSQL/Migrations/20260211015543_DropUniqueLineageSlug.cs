using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCraft.Cms.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class DropUniqueLineageSlug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lineages_SlugNormalized",
                schema: "Rules",
                table: "Lineages");

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_SlugNormalized",
                schema: "Rules",
                table: "Lineages",
                column: "SlugNormalized");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lineages_SlugNormalized",
                schema: "Rules",
                table: "Lineages");

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_SlugNormalized",
                schema: "Rules",
                table: "Lineages",
                column: "SlugNormalized",
                unique: true);
        }
    }
}
