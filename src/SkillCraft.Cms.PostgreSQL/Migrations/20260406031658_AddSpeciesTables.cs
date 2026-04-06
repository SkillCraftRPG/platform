using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkillCraft.Cms.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddSpeciesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpeciesCategoryId",
                schema: "Rules",
                table: "Lineages",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SpeciesCategoryUid",
                schema: "Rules",
                table: "Lineages",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SpeciesCategories",
                schema: "Rules",
                columns: table => new
                {
                    SpeciesCategoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Key = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    KeyNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    Columns = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_SpeciesCategories", x => x.SpeciesCategoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_SpeciesCategoryId",
                schema: "Rules",
                table: "Lineages",
                column: "SpeciesCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_SpeciesCategoryUid",
                schema: "Rules",
                table: "Lineages",
                column: "SpeciesCategoryUid");

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesCategories_CreatedBy",
                schema: "Rules",
                table: "SpeciesCategories",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesCategories_CreatedOn",
                schema: "Rules",
                table: "SpeciesCategories",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesCategories_Id",
                schema: "Rules",
                table: "SpeciesCategories",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesCategories_IsPublished",
                schema: "Rules",
                table: "SpeciesCategories",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesCategories_Key",
                schema: "Rules",
                table: "SpeciesCategories",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesCategories_KeyNormalized",
                schema: "Rules",
                table: "SpeciesCategories",
                column: "KeyNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesCategories_Name",
                schema: "Rules",
                table: "SpeciesCategories",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesCategories_Order",
                schema: "Rules",
                table: "SpeciesCategories",
                column: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesCategories_StreamId",
                schema: "Rules",
                table: "SpeciesCategories",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesCategories_UpdatedBy",
                schema: "Rules",
                table: "SpeciesCategories",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesCategories_UpdatedOn",
                schema: "Rules",
                table: "SpeciesCategories",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesCategories_Version",
                schema: "Rules",
                table: "SpeciesCategories",
                column: "Version");

            migrationBuilder.AddForeignKey(
                name: "FK_Lineages_SpeciesCategories_SpeciesCategoryId",
                schema: "Rules",
                table: "Lineages",
                column: "SpeciesCategoryId",
                principalSchema: "Rules",
                principalTable: "SpeciesCategories",
                principalColumn: "SpeciesCategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lineages_SpeciesCategories_SpeciesCategoryId",
                schema: "Rules",
                table: "Lineages");

            migrationBuilder.DropTable(
                name: "SpeciesCategories",
                schema: "Rules");

            migrationBuilder.DropIndex(
                name: "IX_Lineages_SpeciesCategoryId",
                schema: "Rules",
                table: "Lineages");

            migrationBuilder.DropIndex(
                name: "IX_Lineages_SpeciesCategoryUid",
                schema: "Rules",
                table: "Lineages");

            migrationBuilder.DropColumn(
                name: "SpeciesCategoryId",
                schema: "Rules",
                table: "Lineages");

            migrationBuilder.DropColumn(
                name: "SpeciesCategoryUid",
                schema: "Rules",
                table: "Lineages");
        }
    }
}
