using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkillCraft.Cms.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class CreateMarkerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "Encyclopedia",
                table: "Maps",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Markers",
                schema: "Encyclopedia",
                columns: table => new
                {
                    MarkerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false),
                    KeyNormalized = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    MapId = table.Column<int>(type: "integer", nullable: false),
                    MapUid = table.Column<Guid>(type: "uuid", nullable: false),
                    X = table.Column<int>(type: "integer", nullable: false),
                    Y = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Markers", x => x.MarkerId);
                    table.ForeignKey(
                        name: "FK_Markers_Maps_MapId",
                        column: x => x.MapId,
                        principalSchema: "Encyclopedia",
                        principalTable: "Maps",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Markers_CreatedBy",
                schema: "Encyclopedia",
                table: "Markers",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Markers_CreatedOn",
                schema: "Encyclopedia",
                table: "Markers",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Markers_Id",
                schema: "Encyclopedia",
                table: "Markers",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Markers_IsPublished",
                schema: "Encyclopedia",
                table: "Markers",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Markers_Key",
                schema: "Encyclopedia",
                table: "Markers",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_Markers_MapId",
                schema: "Encyclopedia",
                table: "Markers",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_Markers_MapUid",
                schema: "Encyclopedia",
                table: "Markers",
                column: "MapUid");

            migrationBuilder.CreateIndex(
                name: "IX_Markers_StreamId",
                schema: "Encyclopedia",
                table: "Markers",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Markers_Title",
                schema: "Encyclopedia",
                table: "Markers",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Markers_UpdatedBy",
                schema: "Encyclopedia",
                table: "Markers",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Markers_UpdatedOn",
                schema: "Encyclopedia",
                table: "Markers",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Markers_Version",
                schema: "Encyclopedia",
                table: "Markers",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_Markers_X",
                schema: "Encyclopedia",
                table: "Markers",
                column: "X");

            migrationBuilder.CreateIndex(
                name: "IX_Markers_Y",
                schema: "Encyclopedia",
                table: "Markers",
                column: "Y");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Markers",
                schema: "Encyclopedia");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "Encyclopedia",
                table: "Maps",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);
        }
    }
}
