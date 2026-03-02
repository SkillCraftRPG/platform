using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCraft.Cms.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class CompleteSpellEffectTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "KeyNormalized",
                schema: "Rules",
                table: "SpellEffects",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                schema: "Rules",
                table: "SpellEffects",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_SpellEffects_Key",
                schema: "Rules",
                table: "SpellEffects",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_SpellEffects_KeyNormalized",
                schema: "Rules",
                table: "SpellEffects",
                column: "KeyNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpellEffects_Name",
                schema: "Rules",
                table: "SpellEffects",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SpellEffects_Key",
                schema: "Rules",
                table: "SpellEffects");

            migrationBuilder.DropIndex(
                name: "IX_SpellEffects_KeyNormalized",
                schema: "Rules",
                table: "SpellEffects");

            migrationBuilder.DropIndex(
                name: "IX_SpellEffects_Name",
                schema: "Rules",
                table: "SpellEffects");

            migrationBuilder.AlterColumn<string>(
                name: "KeyNormalized",
                schema: "Rules",
                table: "SpellEffects",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                schema: "Rules",
                table: "SpellEffects",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);
        }
    }
}
