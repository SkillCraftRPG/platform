using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkillCraft.Cms.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class CreateSpellCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpellCategories",
                schema: "Rules",
                columns: table => new
                {
                    SpellCategoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Key = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    KeyNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    ParentUid = table.Column<Guid>(type: "uuid", nullable: true),
                    StreamId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpellCategories", x => x.SpellCategoryId);
                    table.ForeignKey(
                        name: "FK_SpellCategories_SpellCategories_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Rules",
                        principalTable: "SpellCategories",
                        principalColumn: "SpellCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpellCategories_CreatedBy",
                schema: "Rules",
                table: "SpellCategories",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SpellCategories_CreatedOn",
                schema: "Rules",
                table: "SpellCategories",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_SpellCategories_Id",
                schema: "Rules",
                table: "SpellCategories",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpellCategories_IsPublished",
                schema: "Rules",
                table: "SpellCategories",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_SpellCategories_Key",
                schema: "Rules",
                table: "SpellCategories",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_SpellCategories_KeyNormalized",
                schema: "Rules",
                table: "SpellCategories",
                column: "KeyNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpellCategories_Name",
                schema: "Rules",
                table: "SpellCategories",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_SpellCategories_ParentId",
                schema: "Rules",
                table: "SpellCategories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_SpellCategories_ParentUid",
                schema: "Rules",
                table: "SpellCategories",
                column: "ParentUid");

            migrationBuilder.CreateIndex(
                name: "IX_SpellCategories_StreamId",
                schema: "Rules",
                table: "SpellCategories",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpellCategories_UpdatedBy",
                schema: "Rules",
                table: "SpellCategories",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SpellCategories_UpdatedOn",
                schema: "Rules",
                table: "SpellCategories",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_SpellCategories_Version",
                schema: "Rules",
                table: "SpellCategories",
                column: "Version");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpellCategories",
                schema: "Rules");
        }
    }
}
