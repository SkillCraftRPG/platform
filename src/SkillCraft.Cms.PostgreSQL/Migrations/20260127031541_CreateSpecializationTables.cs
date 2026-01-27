using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkillCraft.Cms.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class CreateSpecializationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Specializations",
                schema: "Rules",
                columns: table => new
                {
                    SpecializationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SlugNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Tier = table.Column<int>(type: "integer", nullable: false),
                    MandatoryTalentId = table.Column<int>(type: "integer", nullable: true),
                    MandatoryTalentUid = table.Column<Guid>(type: "uuid", nullable: true),
                    OtherRequirements = table.Column<string>(type: "text", nullable: true),
                    OtherOptions = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_Specializations", x => x.SpecializationId);
                    table.ForeignKey(
                        name: "FK_Specializations_Talents_MandatoryTalentId",
                        column: x => x.MandatoryTalentId,
                        principalSchema: "Rules",
                        principalTable: "Talents",
                        principalColumn: "TalentId");
                });

            migrationBuilder.CreateTable(
                name: "Doctrines",
                schema: "Rules",
                columns: table => new
                {
                    DoctrineId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Key = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    KeyNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SpecializationId = table.Column<int>(type: "integer", nullable: false),
                    SpecializationUid = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_Doctrines", x => x.DoctrineId);
                    table.ForeignKey(
                        name: "FK_Doctrines_Specializations_SpecializationId",
                        column: x => x.SpecializationId,
                        principalSchema: "Rules",
                        principalTable: "Specializations",
                        principalColumn: "SpecializationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SpecializationOptionalTalents",
                schema: "Rules",
                columns: table => new
                {
                    SpecializationId = table.Column<int>(type: "integer", nullable: false),
                    TalentId = table.Column<int>(type: "integer", nullable: false),
                    SpecializationUid = table.Column<Guid>(type: "uuid", nullable: false),
                    TalentUid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecializationOptionalTalents", x => new { x.SpecializationId, x.TalentId });
                    table.ForeignKey(
                        name: "FK_SpecializationOptionalTalents_Specializations_Specializatio~",
                        column: x => x.SpecializationId,
                        principalSchema: "Rules",
                        principalTable: "Specializations",
                        principalColumn: "SpecializationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecializationOptionalTalents_Talents_TalentId",
                        column: x => x.TalentId,
                        principalSchema: "Rules",
                        principalTable: "Talents",
                        principalColumn: "TalentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctrineDiscountedTalents",
                schema: "Rules",
                columns: table => new
                {
                    DoctrineId = table.Column<int>(type: "integer", nullable: false),
                    TalentId = table.Column<int>(type: "integer", nullable: false),
                    DoctrineUid = table.Column<Guid>(type: "uuid", nullable: false),
                    TalentUid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctrineDiscountedTalents", x => new { x.DoctrineId, x.TalentId });
                    table.ForeignKey(
                        name: "FK_DoctrineDiscountedTalents_Doctrines_DoctrineId",
                        column: x => x.DoctrineId,
                        principalSchema: "Rules",
                        principalTable: "Doctrines",
                        principalColumn: "DoctrineId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctrineDiscountedTalents_Talents_TalentId",
                        column: x => x.TalentId,
                        principalSchema: "Rules",
                        principalTable: "Talents",
                        principalColumn: "TalentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctrineFeatures",
                schema: "Rules",
                columns: table => new
                {
                    DoctrineId = table.Column<int>(type: "integer", nullable: false),
                    FeatureId = table.Column<int>(type: "integer", nullable: false),
                    DoctrineUid = table.Column<Guid>(type: "uuid", nullable: false),
                    FeatureUid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctrineFeatures", x => new { x.DoctrineId, x.FeatureId });
                    table.ForeignKey(
                        name: "FK_DoctrineFeatures_Doctrines_DoctrineId",
                        column: x => x.DoctrineId,
                        principalSchema: "Rules",
                        principalTable: "Doctrines",
                        principalColumn: "DoctrineId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctrineFeatures_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalSchema: "Rules",
                        principalTable: "Features",
                        principalColumn: "FeatureId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctrineDiscountedTalents_DoctrineId",
                schema: "Rules",
                table: "DoctrineDiscountedTalents",
                column: "DoctrineId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctrineDiscountedTalents_DoctrineUid",
                schema: "Rules",
                table: "DoctrineDiscountedTalents",
                column: "DoctrineUid");

            migrationBuilder.CreateIndex(
                name: "IX_DoctrineDiscountedTalents_TalentId",
                schema: "Rules",
                table: "DoctrineDiscountedTalents",
                column: "TalentId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctrineDiscountedTalents_TalentUid",
                schema: "Rules",
                table: "DoctrineDiscountedTalents",
                column: "TalentUid");

            migrationBuilder.CreateIndex(
                name: "IX_DoctrineFeatures_DoctrineId",
                schema: "Rules",
                table: "DoctrineFeatures",
                column: "DoctrineId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctrineFeatures_DoctrineUid",
                schema: "Rules",
                table: "DoctrineFeatures",
                column: "DoctrineUid");

            migrationBuilder.CreateIndex(
                name: "IX_DoctrineFeatures_FeatureId",
                schema: "Rules",
                table: "DoctrineFeatures",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctrineFeatures_FeatureUid",
                schema: "Rules",
                table: "DoctrineFeatures",
                column: "FeatureUid");

            migrationBuilder.CreateIndex(
                name: "IX_Doctrines_CreatedBy",
                schema: "Rules",
                table: "Doctrines",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Doctrines_CreatedOn",
                schema: "Rules",
                table: "Doctrines",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Doctrines_Id",
                schema: "Rules",
                table: "Doctrines",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctrines_IsPublished",
                schema: "Rules",
                table: "Doctrines",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Doctrines_Key",
                schema: "Rules",
                table: "Doctrines",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_Doctrines_KeyNormalized",
                schema: "Rules",
                table: "Doctrines",
                column: "KeyNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctrines_Name",
                schema: "Rules",
                table: "Doctrines",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Doctrines_SpecializationId",
                schema: "Rules",
                table: "Doctrines",
                column: "SpecializationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctrines_SpecializationUid",
                schema: "Rules",
                table: "Doctrines",
                column: "SpecializationUid");

            migrationBuilder.CreateIndex(
                name: "IX_Doctrines_StreamId",
                schema: "Rules",
                table: "Doctrines",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctrines_UpdatedBy",
                schema: "Rules",
                table: "Doctrines",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Doctrines_UpdatedOn",
                schema: "Rules",
                table: "Doctrines",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Doctrines_Version",
                schema: "Rules",
                table: "Doctrines",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_SpecializationOptionalTalents_SpecializationId",
                schema: "Rules",
                table: "SpecializationOptionalTalents",
                column: "SpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecializationOptionalTalents_SpecializationUid",
                schema: "Rules",
                table: "SpecializationOptionalTalents",
                column: "SpecializationUid");

            migrationBuilder.CreateIndex(
                name: "IX_SpecializationOptionalTalents_TalentId",
                schema: "Rules",
                table: "SpecializationOptionalTalents",
                column: "TalentId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecializationOptionalTalents_TalentUid",
                schema: "Rules",
                table: "SpecializationOptionalTalents",
                column: "TalentUid");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_CreatedBy",
                schema: "Rules",
                table: "Specializations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_CreatedOn",
                schema: "Rules",
                table: "Specializations",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_Id",
                schema: "Rules",
                table: "Specializations",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_IsPublished",
                schema: "Rules",
                table: "Specializations",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_MandatoryTalentId",
                schema: "Rules",
                table: "Specializations",
                column: "MandatoryTalentId");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_MandatoryTalentUid",
                schema: "Rules",
                table: "Specializations",
                column: "MandatoryTalentUid");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_Name",
                schema: "Rules",
                table: "Specializations",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_Slug",
                schema: "Rules",
                table: "Specializations",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_SlugNormalized",
                schema: "Rules",
                table: "Specializations",
                column: "SlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_StreamId",
                schema: "Rules",
                table: "Specializations",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_Tier",
                schema: "Rules",
                table: "Specializations",
                column: "Tier");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_UpdatedBy",
                schema: "Rules",
                table: "Specializations",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_UpdatedOn",
                schema: "Rules",
                table: "Specializations",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_Version",
                schema: "Rules",
                table: "Specializations",
                column: "Version");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctrineDiscountedTalents",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "DoctrineFeatures",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "SpecializationOptionalTalents",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "Doctrines",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "Specializations",
                schema: "Rules");
        }
    }
}
