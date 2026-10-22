using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCraft.Cms.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddSpecializationOrigins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpecializationOrigins",
                schema: "Rules",
                columns: table => new
                {
                    SpecializationId = table.Column<int>(type: "integer", nullable: false),
                    OriginId = table.Column<int>(type: "integer", nullable: false),
                    SpecializationUid = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginUid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecializationOrigins", x => new { x.SpecializationId, x.OriginId });
                    table.ForeignKey(
                        name: "FK_SpecializationOrigins_Specializations_OriginId",
                        column: x => x.OriginId,
                        principalSchema: "Rules",
                        principalTable: "Specializations",
                        principalColumn: "SpecializationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecializationOrigins_Specializations_SpecializationId",
                        column: x => x.SpecializationId,
                        principalSchema: "Rules",
                        principalTable: "Specializations",
                        principalColumn: "SpecializationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpecializationOrigins_OriginId",
                schema: "Rules",
                table: "SpecializationOrigins",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecializationOrigins_OriginUid",
                schema: "Rules",
                table: "SpecializationOrigins",
                column: "OriginUid");

            migrationBuilder.CreateIndex(
                name: "IX_SpecializationOrigins_SpecializationId",
                schema: "Rules",
                table: "SpecializationOrigins",
                column: "SpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecializationOrigins_SpecializationUid",
                schema: "Rules",
                table: "SpecializationOrigins",
                column: "SpecializationUid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecializationOrigins",
                schema: "Rules");
        }
    }
}
