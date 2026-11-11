using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCraft.Cms.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class UnlimitedRangeSpells : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Range",
                schema: "Rules",
                table: "SpellEffects",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Range",
                schema: "Rules",
                table: "SpellEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
