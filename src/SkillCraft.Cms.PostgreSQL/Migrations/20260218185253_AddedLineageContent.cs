using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCraft.Cms.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddedLineageContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Culture",
                schema: "Rules",
                table: "Lineages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Geography",
                schema: "Rules",
                table: "Lineages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "History",
                schema: "Rules",
                table: "Lineages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Morphology",
                schema: "Rules",
                table: "Lineages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Politics",
                schema: "Rules",
                table: "Lineages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Psychology",
                schema: "Rules",
                table: "Lineages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Relations",
                schema: "Rules",
                table: "Lineages",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Culture",
                schema: "Rules",
                table: "Lineages");

            migrationBuilder.DropColumn(
                name: "Geography",
                schema: "Rules",
                table: "Lineages");

            migrationBuilder.DropColumn(
                name: "History",
                schema: "Rules",
                table: "Lineages");

            migrationBuilder.DropColumn(
                name: "Morphology",
                schema: "Rules",
                table: "Lineages");

            migrationBuilder.DropColumn(
                name: "Politics",
                schema: "Rules",
                table: "Lineages");

            migrationBuilder.DropColumn(
                name: "Psychology",
                schema: "Rules",
                table: "Lineages");

            migrationBuilder.DropColumn(
                name: "Relations",
                schema: "Rules",
                table: "Lineages");
        }
    }
}
