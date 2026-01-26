using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkillCraft.Cms.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class CreateSpellTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Spells",
                schema: "Rules",
                columns: table => new
                {
                    SpellId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SlugNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Tier = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Spells", x => x.SpellId);
                });

            migrationBuilder.CreateTable(
                name: "SpellEffects",
                schema: "Rules",
                columns: table => new
                {
                    SpellEffectId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false),
                    KeyNormalized = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SpellId = table.Column<int>(type: "integer", nullable: false),
                    SpellUid = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    CastingTime = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    IsRitual = table.Column<bool>(type: "boolean", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: true),
                    DurationUnit = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsConcentration = table.Column<bool>(type: "boolean", nullable: false),
                    Range = table.Column<int>(type: "integer", nullable: false),
                    IsSomatic = table.Column<bool>(type: "boolean", nullable: false),
                    IsVerbal = table.Column<bool>(type: "boolean", nullable: false),
                    Focus = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    Material = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
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
                    table.PrimaryKey("PK_SpellEffects", x => x.SpellEffectId);
                    table.ForeignKey(
                        name: "FK_SpellEffects_Spells_SpellId",
                        column: x => x.SpellId,
                        principalSchema: "Rules",
                        principalTable: "Spells",
                        principalColumn: "SpellId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpellEffects_CreatedBy",
                schema: "Rules",
                table: "SpellEffects",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SpellEffects_CreatedOn",
                schema: "Rules",
                table: "SpellEffects",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_SpellEffects_Id",
                schema: "Rules",
                table: "SpellEffects",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpellEffects_IsPublished",
                schema: "Rules",
                table: "SpellEffects",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_SpellEffects_SpellId",
                schema: "Rules",
                table: "SpellEffects",
                column: "SpellId");

            migrationBuilder.CreateIndex(
                name: "IX_SpellEffects_SpellUid",
                schema: "Rules",
                table: "SpellEffects",
                column: "SpellUid");

            migrationBuilder.CreateIndex(
                name: "IX_SpellEffects_StreamId",
                schema: "Rules",
                table: "SpellEffects",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpellEffects_UpdatedBy",
                schema: "Rules",
                table: "SpellEffects",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SpellEffects_UpdatedOn",
                schema: "Rules",
                table: "SpellEffects",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_SpellEffects_Version",
                schema: "Rules",
                table: "SpellEffects",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_Spells_CreatedBy",
                schema: "Rules",
                table: "Spells",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Spells_CreatedOn",
                schema: "Rules",
                table: "Spells",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Spells_Id",
                schema: "Rules",
                table: "Spells",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Spells_IsPublished",
                schema: "Rules",
                table: "Spells",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Spells_Name",
                schema: "Rules",
                table: "Spells",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Spells_Slug",
                schema: "Rules",
                table: "Spells",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Spells_SlugNormalized",
                schema: "Rules",
                table: "Spells",
                column: "SlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Spells_StreamId",
                schema: "Rules",
                table: "Spells",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Spells_Tier",
                schema: "Rules",
                table: "Spells",
                column: "Tier");

            migrationBuilder.CreateIndex(
                name: "IX_Spells_UpdatedBy",
                schema: "Rules",
                table: "Spells",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Spells_UpdatedOn",
                schema: "Rules",
                table: "Spells",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Spells_Version",
                schema: "Rules",
                table: "Spells",
                column: "Version");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpellEffects",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "Spells",
                schema: "Rules");
        }
    }
}
