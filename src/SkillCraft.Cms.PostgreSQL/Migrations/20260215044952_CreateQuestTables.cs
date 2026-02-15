using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkillCraft.Cms.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class CreateQuestTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestGroups",
                schema: "Encyclopedia",
                columns: table => new
                {
                    QuestGroupId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    StreamId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestGroups", x => x.QuestGroupId);
                });

            migrationBuilder.CreateTable(
                name: "QuestLogs",
                schema: "Encyclopedia",
                columns: table => new
                {
                    QuestLogId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SlugNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    MetaDescription = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
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
                    table.PrimaryKey("PK_QuestLogs", x => x.QuestLogId);
                });

            migrationBuilder.CreateTable(
                name: "Quests",
                schema: "Encyclopedia",
                columns: table => new
                {
                    QuestId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    QuestLogId = table.Column<int>(type: "integer", nullable: false),
                    QuestLogUid = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestGroupId = table.Column<int>(type: "integer", nullable: true),
                    QuestGroupUid = table.Column<Guid>(type: "uuid", nullable: true),
                    GrantedLevels = table.Column<int>(type: "integer", nullable: false),
                    ProgressRatio = table.Column<double>(type: "double precision", nullable: false),
                    HtmlContent = table.Column<string>(type: "text", nullable: true),
                    CompletedEntries = table.Column<string>(type: "text", nullable: true),
                    ActiveEntries = table.Column<string>(type: "text", nullable: true),
                    StreamId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quests", x => x.QuestId);
                    table.ForeignKey(
                        name: "FK_Quests_QuestGroups_QuestGroupId",
                        column: x => x.QuestGroupId,
                        principalSchema: "Encyclopedia",
                        principalTable: "QuestGroups",
                        principalColumn: "QuestGroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quests_QuestLogs_QuestLogId",
                        column: x => x.QuestLogId,
                        principalSchema: "Encyclopedia",
                        principalTable: "QuestLogs",
                        principalColumn: "QuestLogId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestGroups_CreatedBy",
                schema: "Encyclopedia",
                table: "QuestGroups",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QuestGroups_CreatedOn",
                schema: "Encyclopedia",
                table: "QuestGroups",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_QuestGroups_Id",
                schema: "Encyclopedia",
                table: "QuestGroups",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestGroups_IsPublished",
                schema: "Encyclopedia",
                table: "QuestGroups",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_QuestGroups_Name",
                schema: "Encyclopedia",
                table: "QuestGroups",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_QuestGroups_StreamId",
                schema: "Encyclopedia",
                table: "QuestGroups",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestGroups_UpdatedBy",
                schema: "Encyclopedia",
                table: "QuestGroups",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QuestGroups_UpdatedOn",
                schema: "Encyclopedia",
                table: "QuestGroups",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_QuestGroups_Version",
                schema: "Encyclopedia",
                table: "QuestGroups",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_QuestLogs_CreatedBy",
                schema: "Encyclopedia",
                table: "QuestLogs",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QuestLogs_CreatedOn",
                schema: "Encyclopedia",
                table: "QuestLogs",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_QuestLogs_Id",
                schema: "Encyclopedia",
                table: "QuestLogs",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestLogs_IsPublished",
                schema: "Encyclopedia",
                table: "QuestLogs",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_QuestLogs_Slug",
                schema: "Encyclopedia",
                table: "QuestLogs",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_QuestLogs_SlugNormalized",
                schema: "Encyclopedia",
                table: "QuestLogs",
                column: "SlugNormalized");

            migrationBuilder.CreateIndex(
                name: "IX_QuestLogs_StreamId",
                schema: "Encyclopedia",
                table: "QuestLogs",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestLogs_Title",
                schema: "Encyclopedia",
                table: "QuestLogs",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_QuestLogs_UpdatedBy",
                schema: "Encyclopedia",
                table: "QuestLogs",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QuestLogs_UpdatedOn",
                schema: "Encyclopedia",
                table: "QuestLogs",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_QuestLogs_Version",
                schema: "Encyclopedia",
                table: "QuestLogs",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_CreatedBy",
                schema: "Encyclopedia",
                table: "Quests",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_CreatedOn",
                schema: "Encyclopedia",
                table: "Quests",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_GrantedLevels",
                schema: "Encyclopedia",
                table: "Quests",
                column: "GrantedLevels");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_Id",
                schema: "Encyclopedia",
                table: "Quests",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quests_IsPublished",
                schema: "Encyclopedia",
                table: "Quests",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_ProgressRatio",
                schema: "Encyclopedia",
                table: "Quests",
                column: "ProgressRatio");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_QuestGroupId",
                schema: "Encyclopedia",
                table: "Quests",
                column: "QuestGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_QuestGroupUid",
                schema: "Encyclopedia",
                table: "Quests",
                column: "QuestGroupUid");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_QuestLogId",
                schema: "Encyclopedia",
                table: "Quests",
                column: "QuestLogId");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_QuestLogUid",
                schema: "Encyclopedia",
                table: "Quests",
                column: "QuestLogUid");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_StreamId",
                schema: "Encyclopedia",
                table: "Quests",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quests_Title",
                schema: "Encyclopedia",
                table: "Quests",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_UpdatedBy",
                schema: "Encyclopedia",
                table: "Quests",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_UpdatedOn",
                schema: "Encyclopedia",
                table: "Quests",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_Version",
                schema: "Encyclopedia",
                table: "Quests",
                column: "Version");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Quests",
                schema: "Encyclopedia");

            migrationBuilder.DropTable(
                name: "QuestGroups",
                schema: "Encyclopedia");

            migrationBuilder.DropTable(
                name: "QuestLogs",
                schema: "Encyclopedia");
        }
    }
}
