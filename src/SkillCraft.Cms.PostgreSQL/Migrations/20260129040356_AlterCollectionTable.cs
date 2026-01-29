using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCraft.Cms.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class AlterCollectionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KeyNormalized",
                schema: "Encyclopedia",
                table: "Collections",
                newName: "SlugNormalized");

            migrationBuilder.RenameColumn(
                name: "Key",
                schema: "Encyclopedia",
                table: "Collections",
                newName: "Slug");

            migrationBuilder.RenameColumn(
                name: "Description",
                schema: "Encyclopedia",
                table: "Collections",
                newName: "MetaDescription");

            migrationBuilder.RenameIndex(
                name: "IX_Collections_KeyNormalized",
                schema: "Encyclopedia",
                table: "Collections",
                newName: "IX_Collections_SlugNormalized");

            migrationBuilder.RenameIndex(
                name: "IX_Collections_Key",
                schema: "Encyclopedia",
                table: "Collections",
                newName: "IX_Collections_Slug");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Encyclopedia",
                table: "Collections",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HtmlContent",
                schema: "Encyclopedia",
                table: "Collections",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HtmlContent",
                schema: "Encyclopedia",
                table: "Collections");

            migrationBuilder.RenameColumn(
                name: "SlugNormalized",
                schema: "Encyclopedia",
                table: "Collections",
                newName: "KeyNormalized");

            migrationBuilder.RenameColumn(
                name: "Slug",
                schema: "Encyclopedia",
                table: "Collections",
                newName: "Key");

            migrationBuilder.RenameColumn(
                name: "MetaDescription",
                schema: "Encyclopedia",
                table: "Collections",
                newName: "Description");

            migrationBuilder.RenameIndex(
                name: "IX_Collections_SlugNormalized",
                schema: "Encyclopedia",
                table: "Collections",
                newName: "IX_Collections_KeyNormalized");

            migrationBuilder.RenameIndex(
                name: "IX_Collections_Slug",
                schema: "Encyclopedia",
                table: "Collections",
                newName: "IX_Collections_Key");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Encyclopedia",
                table: "Collections",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);
        }
    }
}
