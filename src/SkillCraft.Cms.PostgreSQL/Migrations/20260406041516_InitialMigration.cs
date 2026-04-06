using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkillCraft.Cms.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Encyclopedia");

            migrationBuilder.EnsureSchema(
                name: "Rules");

            migrationBuilder.CreateTable(
                name: "Attributes",
                schema: "Rules",
                columns: table => new
                {
                    AttributeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SlugNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Value = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Category = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
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
                    table.PrimaryKey("PK_Attributes", x => x.AttributeId);
                });

            migrationBuilder.CreateTable(
                name: "Collections",
                schema: "Encyclopedia",
                columns: table => new
                {
                    CollectionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SlugNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    MetaDescription = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_Collections", x => x.CollectionId);
                });

            migrationBuilder.CreateTable(
                name: "Customizations",
                schema: "Rules",
                columns: table => new
                {
                    CustomizationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SlugNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Kind = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
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
                    table.PrimaryKey("PK_Customizations", x => x.CustomizationId);
                });

            migrationBuilder.CreateTable(
                name: "Features",
                schema: "Rules",
                columns: table => new
                {
                    FeatureId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Key = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    KeyNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
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
                    table.PrimaryKey("PK_Features", x => x.FeatureId);
                });

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
                name: "Scripts",
                schema: "Rules",
                columns: table => new
                {
                    ScriptId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SlugNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
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
                    table.PrimaryKey("PK_Scripts", x => x.ScriptId);
                });

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
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
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
                name: "Skills",
                schema: "Rules",
                columns: table => new
                {
                    SkillId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SlugNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Value = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    AttributeId = table.Column<int>(type: "integer", nullable: true),
                    AttributeUid = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_Skills", x => x.SkillId);
                    table.ForeignKey(
                        name: "FK_Skills_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "Rules",
                        principalTable: "Attributes",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Statistics",
                schema: "Rules",
                columns: table => new
                {
                    StatisticId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SlugNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Value = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    AttributeId = table.Column<int>(type: "integer", nullable: false),
                    AttributeUid = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_Statistics", x => x.StatisticId);
                    table.ForeignKey(
                        name: "FK_Statistics_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "Rules",
                        principalTable: "Attributes",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                schema: "Encyclopedia",
                columns: table => new
                {
                    ArticleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SlugNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CollectionId = table.Column<int>(type: "integer", nullable: false),
                    CollectionUid = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    ParentUid = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_Articles", x => x.ArticleId);
                    table.ForeignKey(
                        name: "FK_Articles_Articles_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Encyclopedia",
                        principalTable: "Articles",
                        principalColumn: "ArticleId");
                    table.ForeignKey(
                        name: "FK_Articles_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalSchema: "Encyclopedia",
                        principalTable: "Collections",
                        principalColumn: "CollectionId",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateTable(
                name: "Languages",
                schema: "Rules",
                columns: table => new
                {
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SlugNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ScriptId = table.Column<int>(type: "integer", nullable: true),
                    ScriptUid = table.Column<Guid>(type: "uuid", nullable: true),
                    TypicalSpeakers = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_Languages", x => x.LanguageId);
                    table.ForeignKey(
                        name: "FK_Languages_Scripts_ScriptId",
                        column: x => x.ScriptId,
                        principalSchema: "Rules",
                        principalTable: "Scripts",
                        principalColumn: "ScriptId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Lineages",
                schema: "Rules",
                columns: table => new
                {
                    LineageId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SlugNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SpeciesCategoryId = table.Column<int>(type: "integer", nullable: true),
                    SpeciesCategoryUid = table.Column<Guid>(type: "uuid", nullable: true),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    ParentUid = table.Column<Guid>(type: "uuid", nullable: true),
                    ExtraLanguages = table.Column<int>(type: "integer", nullable: false),
                    LanguagesText = table.Column<string>(type: "text", nullable: true),
                    FamilyNames = table.Column<string>(type: "text", nullable: true),
                    FemaleNames = table.Column<string>(type: "text", nullable: true),
                    MaleNames = table.Column<string>(type: "text", nullable: true),
                    UnisexNames = table.Column<string>(type: "text", nullable: true),
                    CustomNames = table.Column<string>(type: "text", nullable: true),
                    NamesText = table.Column<string>(type: "text", nullable: true),
                    Walk = table.Column<int>(type: "integer", nullable: false),
                    Climb = table.Column<int>(type: "integer", nullable: false),
                    Swim = table.Column<int>(type: "integer", nullable: false),
                    Fly = table.Column<int>(type: "integer", nullable: false),
                    Hover = table.Column<bool>(type: "boolean", nullable: false),
                    Burrow = table.Column<int>(type: "integer", nullable: false),
                    SizeCategory = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    SizeRoll = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Malnutrition = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Skinny = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    NormalWeight = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Overweight = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Obese = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Teenager = table.Column<int>(type: "integer", nullable: false),
                    Adult = table.Column<int>(type: "integer", nullable: false),
                    Mature = table.Column<int>(type: "integer", nullable: false),
                    Venerable = table.Column<int>(type: "integer", nullable: false),
                    MetaDescription = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    Summary = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    HtmlContent = table.Column<string>(type: "text", nullable: true),
                    Morphology = table.Column<string>(type: "text", nullable: true),
                    Psychology = table.Column<string>(type: "text", nullable: true),
                    Culture = table.Column<string>(type: "text", nullable: true),
                    History = table.Column<string>(type: "text", nullable: true),
                    Geography = table.Column<string>(type: "text", nullable: true),
                    Politics = table.Column<string>(type: "text", nullable: true),
                    Relations = table.Column<string>(type: "text", nullable: true),
                    StreamId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lineages", x => x.LineageId);
                    table.ForeignKey(
                        name: "FK_Lineages_Lineages_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Rules",
                        principalTable: "Lineages",
                        principalColumn: "LineageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lineages_SpeciesCategories_SpeciesCategoryId",
                        column: x => x.SpeciesCategoryId,
                        principalSchema: "Rules",
                        principalTable: "SpeciesCategories",
                        principalColumn: "SpeciesCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SpellCategoryAssociations",
                schema: "Rules",
                columns: table => new
                {
                    SpellId = table.Column<int>(type: "integer", nullable: false),
                    SpellCategoryId = table.Column<int>(type: "integer", nullable: false),
                    SpellUid = table.Column<Guid>(type: "uuid", nullable: false),
                    SpellCategoryUid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpellCategoryAssociations", x => new { x.SpellId, x.SpellCategoryId });
                    table.ForeignKey(
                        name: "FK_SpellCategoryAssociations_SpellCategories_SpellCategoryId",
                        column: x => x.SpellCategoryId,
                        principalSchema: "Rules",
                        principalTable: "SpellCategories",
                        principalColumn: "SpellCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpellCategoryAssociations_Spells_SpellId",
                        column: x => x.SpellId,
                        principalSchema: "Rules",
                        principalTable: "Spells",
                        principalColumn: "SpellId",
                        onDelete: ReferentialAction.Cascade);
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
                    Key = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    KeyNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
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

            migrationBuilder.CreateTable(
                name: "Castes",
                schema: "Rules",
                columns: table => new
                {
                    CasteId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SlugNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    WealthRoll = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    SkillId = table.Column<int>(type: "integer", nullable: true),
                    SkillUid = table.Column<Guid>(type: "uuid", nullable: true),
                    FeatureId = table.Column<int>(type: "integer", nullable: true),
                    FeatureUid = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_Castes", x => x.CasteId);
                    table.ForeignKey(
                        name: "FK_Castes_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalSchema: "Rules",
                        principalTable: "Features",
                        principalColumn: "FeatureId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Castes_Skills_SkillId",
                        column: x => x.SkillId,
                        principalSchema: "Rules",
                        principalTable: "Skills",
                        principalColumn: "SkillId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Educations",
                schema: "Rules",
                columns: table => new
                {
                    EducationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SlugNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    WealthMultiplier = table.Column<int>(type: "integer", nullable: false),
                    SkillId = table.Column<int>(type: "integer", nullable: true),
                    SkillUid = table.Column<Guid>(type: "uuid", nullable: true),
                    FeatureId = table.Column<int>(type: "integer", nullable: true),
                    FeatureUid = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_Educations", x => x.EducationId);
                    table.ForeignKey(
                        name: "FK_Educations_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalSchema: "Rules",
                        principalTable: "Features",
                        principalColumn: "FeatureId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Educations_Skills_SkillId",
                        column: x => x.SkillId,
                        principalSchema: "Rules",
                        principalTable: "Skills",
                        principalColumn: "SkillId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Talents",
                schema: "Rules",
                columns: table => new
                {
                    TalentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SlugNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Tier = table.Column<int>(type: "integer", nullable: false),
                    AllowMultiplePurchases = table.Column<bool>(type: "boolean", nullable: false),
                    SkillId = table.Column<int>(type: "integer", nullable: true),
                    SkillUid = table.Column<Guid>(type: "uuid", nullable: true),
                    RequiredTalentId = table.Column<int>(type: "integer", nullable: true),
                    RequiredTalentUid = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_Talents", x => x.TalentId);
                    table.ForeignKey(
                        name: "FK_Talents_Skills_SkillId",
                        column: x => x.SkillId,
                        principalSchema: "Rules",
                        principalTable: "Skills",
                        principalColumn: "SkillId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Talents_Talents_RequiredTalentId",
                        column: x => x.RequiredTalentId,
                        principalSchema: "Rules",
                        principalTable: "Talents",
                        principalColumn: "TalentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleHierarchy",
                schema: "Encyclopedia",
                columns: table => new
                {
                    ArticleId = table.Column<int>(type: "integer", nullable: false),
                    ArticleUid = table.Column<Guid>(type: "uuid", nullable: false),
                    CollectionId = table.Column<int>(type: "integer", nullable: false),
                    CollectionUid = table.Column<Guid>(type: "uuid", nullable: false),
                    Depth = table.Column<int>(type: "integer", nullable: false),
                    IdPath = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    SlugPath = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleHierarchy", x => x.ArticleId);
                    table.ForeignKey(
                        name: "FK_ArticleHierarchy_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalSchema: "Encyclopedia",
                        principalTable: "Articles",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleHierarchy_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalSchema: "Encyclopedia",
                        principalTable: "Collections",
                        principalColumn: "CollectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LineageFeatures",
                schema: "Rules",
                columns: table => new
                {
                    LineageId = table.Column<int>(type: "integer", nullable: false),
                    FeatureId = table.Column<int>(type: "integer", nullable: false),
                    LineageUid = table.Column<Guid>(type: "uuid", nullable: false),
                    FeatureUid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineageFeatures", x => new { x.LineageId, x.FeatureId });
                    table.ForeignKey(
                        name: "FK_LineageFeatures_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalSchema: "Rules",
                        principalTable: "Features",
                        principalColumn: "FeatureId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineageFeatures_Lineages_LineageId",
                        column: x => x.LineageId,
                        principalSchema: "Rules",
                        principalTable: "Lineages",
                        principalColumn: "LineageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LineageLanguages",
                schema: "Rules",
                columns: table => new
                {
                    LineageId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    LineageUid = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageUid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineageLanguages", x => new { x.LineageId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_LineageLanguages_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "Rules",
                        principalTable: "Languages",
                        principalColumn: "LanguageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineageLanguages_Lineages_LineageId",
                        column: x => x.LineageId,
                        principalSchema: "Rules",
                        principalTable: "Lineages",
                        principalColumn: "LineageId",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_ArticleHierarchy_ArticleUid",
                schema: "Encyclopedia",
                table: "ArticleHierarchy",
                column: "ArticleUid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleHierarchy_CollectionId",
                schema: "Encyclopedia",
                table: "ArticleHierarchy",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleHierarchy_CollectionUid",
                schema: "Encyclopedia",
                table: "ArticleHierarchy",
                column: "CollectionUid");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleHierarchy_Depth",
                schema: "Encyclopedia",
                table: "ArticleHierarchy",
                column: "Depth");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleHierarchy_IdPath",
                schema: "Encyclopedia",
                table: "ArticleHierarchy",
                column: "IdPath");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleHierarchy_SlugPath",
                schema: "Encyclopedia",
                table: "ArticleHierarchy",
                column: "SlugPath");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CollectionId",
                schema: "Encyclopedia",
                table: "Articles",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CollectionUid",
                schema: "Encyclopedia",
                table: "Articles",
                column: "CollectionUid");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CreatedBy",
                schema: "Encyclopedia",
                table: "Articles",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CreatedOn",
                schema: "Encyclopedia",
                table: "Articles",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Id",
                schema: "Encyclopedia",
                table: "Articles",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_IsPublished",
                schema: "Encyclopedia",
                table: "Articles",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ParentId",
                schema: "Encyclopedia",
                table: "Articles",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ParentUid",
                schema: "Encyclopedia",
                table: "Articles",
                column: "ParentUid");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Slug",
                schema: "Encyclopedia",
                table: "Articles",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_SlugNormalized",
                schema: "Encyclopedia",
                table: "Articles",
                column: "SlugNormalized");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_StreamId",
                schema: "Encyclopedia",
                table: "Articles",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Title",
                schema: "Encyclopedia",
                table: "Articles",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_UpdatedBy",
                schema: "Encyclopedia",
                table: "Articles",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_UpdatedOn",
                schema: "Encyclopedia",
                table: "Articles",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Version",
                schema: "Encyclopedia",
                table: "Articles",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_Category",
                schema: "Rules",
                table: "Attributes",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_CreatedBy",
                schema: "Rules",
                table: "Attributes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_CreatedOn",
                schema: "Rules",
                table: "Attributes",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_Id",
                schema: "Rules",
                table: "Attributes",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_IsPublished",
                schema: "Rules",
                table: "Attributes",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_Name",
                schema: "Rules",
                table: "Attributes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_Slug",
                schema: "Rules",
                table: "Attributes",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_SlugNormalized",
                schema: "Rules",
                table: "Attributes",
                column: "SlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_StreamId",
                schema: "Rules",
                table: "Attributes",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_UpdatedBy",
                schema: "Rules",
                table: "Attributes",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_UpdatedOn",
                schema: "Rules",
                table: "Attributes",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_Value",
                schema: "Rules",
                table: "Attributes",
                column: "Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_Version",
                schema: "Rules",
                table: "Attributes",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_Castes_CreatedBy",
                schema: "Rules",
                table: "Castes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Castes_CreatedOn",
                schema: "Rules",
                table: "Castes",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Castes_FeatureId",
                schema: "Rules",
                table: "Castes",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Castes_FeatureUid",
                schema: "Rules",
                table: "Castes",
                column: "FeatureUid");

            migrationBuilder.CreateIndex(
                name: "IX_Castes_Id",
                schema: "Rules",
                table: "Castes",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Castes_IsPublished",
                schema: "Rules",
                table: "Castes",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Castes_Name",
                schema: "Rules",
                table: "Castes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Castes_SkillId",
                schema: "Rules",
                table: "Castes",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Castes_SkillUid",
                schema: "Rules",
                table: "Castes",
                column: "SkillUid");

            migrationBuilder.CreateIndex(
                name: "IX_Castes_Slug",
                schema: "Rules",
                table: "Castes",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Castes_SlugNormalized",
                schema: "Rules",
                table: "Castes",
                column: "SlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Castes_StreamId",
                schema: "Rules",
                table: "Castes",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Castes_UpdatedBy",
                schema: "Rules",
                table: "Castes",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Castes_UpdatedOn",
                schema: "Rules",
                table: "Castes",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Castes_Version",
                schema: "Rules",
                table: "Castes",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_Collections_CreatedBy",
                schema: "Encyclopedia",
                table: "Collections",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Collections_CreatedOn",
                schema: "Encyclopedia",
                table: "Collections",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Collections_Id",
                schema: "Encyclopedia",
                table: "Collections",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Collections_IsPublished",
                schema: "Encyclopedia",
                table: "Collections",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Collections_Name",
                schema: "Encyclopedia",
                table: "Collections",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Collections_Slug",
                schema: "Encyclopedia",
                table: "Collections",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Collections_SlugNormalized",
                schema: "Encyclopedia",
                table: "Collections",
                column: "SlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Collections_StreamId",
                schema: "Encyclopedia",
                table: "Collections",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Collections_UpdatedBy",
                schema: "Encyclopedia",
                table: "Collections",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Collections_UpdatedOn",
                schema: "Encyclopedia",
                table: "Collections",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Collections_Version",
                schema: "Encyclopedia",
                table: "Collections",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_CreatedBy",
                schema: "Rules",
                table: "Customizations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_CreatedOn",
                schema: "Rules",
                table: "Customizations",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_Id",
                schema: "Rules",
                table: "Customizations",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_IsPublished",
                schema: "Rules",
                table: "Customizations",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_Kind",
                schema: "Rules",
                table: "Customizations",
                column: "Kind");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_Name",
                schema: "Rules",
                table: "Customizations",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_Slug",
                schema: "Rules",
                table: "Customizations",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_SlugNormalized",
                schema: "Rules",
                table: "Customizations",
                column: "SlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_StreamId",
                schema: "Rules",
                table: "Customizations",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_UpdatedBy",
                schema: "Rules",
                table: "Customizations",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_UpdatedOn",
                schema: "Rules",
                table: "Customizations",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_Version",
                schema: "Rules",
                table: "Customizations",
                column: "Version");

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
                name: "IX_Educations_CreatedBy",
                schema: "Rules",
                table: "Educations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_CreatedOn",
                schema: "Rules",
                table: "Educations",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_FeatureId",
                schema: "Rules",
                table: "Educations",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_FeatureUid",
                schema: "Rules",
                table: "Educations",
                column: "FeatureUid");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_Id",
                schema: "Rules",
                table: "Educations",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Educations_IsPublished",
                schema: "Rules",
                table: "Educations",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_Name",
                schema: "Rules",
                table: "Educations",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_SkillId",
                schema: "Rules",
                table: "Educations",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_SkillUid",
                schema: "Rules",
                table: "Educations",
                column: "SkillUid");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_Slug",
                schema: "Rules",
                table: "Educations",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_SlugNormalized",
                schema: "Rules",
                table: "Educations",
                column: "SlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Educations_StreamId",
                schema: "Rules",
                table: "Educations",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Educations_UpdatedBy",
                schema: "Rules",
                table: "Educations",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_UpdatedOn",
                schema: "Rules",
                table: "Educations",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_Version",
                schema: "Rules",
                table: "Educations",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_Features_CreatedBy",
                schema: "Rules",
                table: "Features",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Features_CreatedOn",
                schema: "Rules",
                table: "Features",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Features_Id",
                schema: "Rules",
                table: "Features",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Features_IsPublished",
                schema: "Rules",
                table: "Features",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Features_Key",
                schema: "Rules",
                table: "Features",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_Features_KeyNormalized",
                schema: "Rules",
                table: "Features",
                column: "KeyNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Features_Name",
                schema: "Rules",
                table: "Features",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Features_StreamId",
                schema: "Rules",
                table: "Features",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Features_UpdatedBy",
                schema: "Rules",
                table: "Features",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Features_UpdatedOn",
                schema: "Rules",
                table: "Features",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Features_Version",
                schema: "Rules",
                table: "Features",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_CreatedBy",
                schema: "Rules",
                table: "Languages",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_CreatedOn",
                schema: "Rules",
                table: "Languages",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Id",
                schema: "Rules",
                table: "Languages",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_IsPublished",
                schema: "Rules",
                table: "Languages",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Name",
                schema: "Rules",
                table: "Languages",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_ScriptId",
                schema: "Rules",
                table: "Languages",
                column: "ScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_ScriptUid",
                schema: "Rules",
                table: "Languages",
                column: "ScriptUid");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Slug",
                schema: "Rules",
                table: "Languages",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_SlugNormalized",
                schema: "Rules",
                table: "Languages",
                column: "SlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_StreamId",
                schema: "Rules",
                table: "Languages",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_UpdatedBy",
                schema: "Rules",
                table: "Languages",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_UpdatedOn",
                schema: "Rules",
                table: "Languages",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Version",
                schema: "Rules",
                table: "Languages",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_LineageFeatures_FeatureId",
                schema: "Rules",
                table: "LineageFeatures",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_LineageFeatures_FeatureUid",
                schema: "Rules",
                table: "LineageFeatures",
                column: "FeatureUid");

            migrationBuilder.CreateIndex(
                name: "IX_LineageFeatures_LineageId",
                schema: "Rules",
                table: "LineageFeatures",
                column: "LineageId");

            migrationBuilder.CreateIndex(
                name: "IX_LineageFeatures_LineageUid",
                schema: "Rules",
                table: "LineageFeatures",
                column: "LineageUid");

            migrationBuilder.CreateIndex(
                name: "IX_LineageLanguages_LanguageId",
                schema: "Rules",
                table: "LineageLanguages",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_LineageLanguages_LanguageUid",
                schema: "Rules",
                table: "LineageLanguages",
                column: "LanguageUid");

            migrationBuilder.CreateIndex(
                name: "IX_LineageLanguages_LineageId",
                schema: "Rules",
                table: "LineageLanguages",
                column: "LineageId");

            migrationBuilder.CreateIndex(
                name: "IX_LineageLanguages_LineageUid",
                schema: "Rules",
                table: "LineageLanguages",
                column: "LineageUid");

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_CreatedBy",
                schema: "Rules",
                table: "Lineages",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_CreatedOn",
                schema: "Rules",
                table: "Lineages",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_Id",
                schema: "Rules",
                table: "Lineages",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_IsPublished",
                schema: "Rules",
                table: "Lineages",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_Name",
                schema: "Rules",
                table: "Lineages",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_ParentId",
                schema: "Rules",
                table: "Lineages",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_ParentUid",
                schema: "Rules",
                table: "Lineages",
                column: "ParentUid");

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_SizeCategory",
                schema: "Rules",
                table: "Lineages",
                column: "SizeCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_Slug",
                schema: "Rules",
                table: "Lineages",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_SlugNormalized",
                schema: "Rules",
                table: "Lineages",
                column: "SlugNormalized");

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
                name: "IX_Lineages_StreamId",
                schema: "Rules",
                table: "Lineages",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_UpdatedBy",
                schema: "Rules",
                table: "Lineages",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_UpdatedOn",
                schema: "Rules",
                table: "Lineages",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_Version",
                schema: "Rules",
                table: "Lineages",
                column: "Version");

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

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_CreatedBy",
                schema: "Rules",
                table: "Scripts",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_CreatedOn",
                schema: "Rules",
                table: "Scripts",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_Id",
                schema: "Rules",
                table: "Scripts",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_IsPublished",
                schema: "Rules",
                table: "Scripts",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_Name",
                schema: "Rules",
                table: "Scripts",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_Slug",
                schema: "Rules",
                table: "Scripts",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_SlugNormalized",
                schema: "Rules",
                table: "Scripts",
                column: "SlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_StreamId",
                schema: "Rules",
                table: "Scripts",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_UpdatedBy",
                schema: "Rules",
                table: "Scripts",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_UpdatedOn",
                schema: "Rules",
                table: "Scripts",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_Version",
                schema: "Rules",
                table: "Scripts",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_AttributeId",
                schema: "Rules",
                table: "Skills",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_AttributeUid",
                schema: "Rules",
                table: "Skills",
                column: "AttributeUid");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_CreatedBy",
                schema: "Rules",
                table: "Skills",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_CreatedOn",
                schema: "Rules",
                table: "Skills",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Id",
                schema: "Rules",
                table: "Skills",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_IsPublished",
                schema: "Rules",
                table: "Skills",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Name",
                schema: "Rules",
                table: "Skills",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Slug",
                schema: "Rules",
                table: "Skills",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_SlugNormalized",
                schema: "Rules",
                table: "Skills",
                column: "SlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_StreamId",
                schema: "Rules",
                table: "Skills",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_UpdatedBy",
                schema: "Rules",
                table: "Skills",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_UpdatedOn",
                schema: "Rules",
                table: "Skills",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Value",
                schema: "Rules",
                table: "Skills",
                column: "Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Version",
                schema: "Rules",
                table: "Skills",
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

            migrationBuilder.CreateIndex(
                name: "IX_SpellCategoryAssociations_SpellCategoryId",
                schema: "Rules",
                table: "SpellCategoryAssociations",
                column: "SpellCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SpellCategoryAssociations_SpellCategoryUid",
                schema: "Rules",
                table: "SpellCategoryAssociations",
                column: "SpellCategoryUid");

            migrationBuilder.CreateIndex(
                name: "IX_SpellCategoryAssociations_SpellId",
                schema: "Rules",
                table: "SpellCategoryAssociations",
                column: "SpellId");

            migrationBuilder.CreateIndex(
                name: "IX_SpellCategoryAssociations_SpellUid",
                schema: "Rules",
                table: "SpellCategoryAssociations",
                column: "SpellUid");

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
                name: "IX_SpellEffects_Key",
                schema: "Rules",
                table: "SpellEffects",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_SpellEffects_KeyNormalized",
                schema: "Rules",
                table: "SpellEffects",
                column: "KeyNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpellEffects_Name",
                schema: "Rules",
                table: "SpellEffects",
                column: "Name");

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

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_AttributeId",
                schema: "Rules",
                table: "Statistics",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_AttributeUid",
                schema: "Rules",
                table: "Statistics",
                column: "AttributeUid");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_CreatedBy",
                schema: "Rules",
                table: "Statistics",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_CreatedOn",
                schema: "Rules",
                table: "Statistics",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_Id",
                schema: "Rules",
                table: "Statistics",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_IsPublished",
                schema: "Rules",
                table: "Statistics",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_Name",
                schema: "Rules",
                table: "Statistics",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_Slug",
                schema: "Rules",
                table: "Statistics",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_SlugNormalized",
                schema: "Rules",
                table: "Statistics",
                column: "SlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_StreamId",
                schema: "Rules",
                table: "Statistics",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_UpdatedBy",
                schema: "Rules",
                table: "Statistics",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_UpdatedOn",
                schema: "Rules",
                table: "Statistics",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_Value",
                schema: "Rules",
                table: "Statistics",
                column: "Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_Version",
                schema: "Rules",
                table: "Statistics",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_AllowMultiplePurchases",
                schema: "Rules",
                table: "Talents",
                column: "AllowMultiplePurchases");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_CreatedBy",
                schema: "Rules",
                table: "Talents",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_CreatedOn",
                schema: "Rules",
                table: "Talents",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_Id",
                schema: "Rules",
                table: "Talents",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Talents_IsPublished",
                schema: "Rules",
                table: "Talents",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_Name",
                schema: "Rules",
                table: "Talents",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_RequiredTalentId",
                schema: "Rules",
                table: "Talents",
                column: "RequiredTalentId");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_RequiredTalentUid",
                schema: "Rules",
                table: "Talents",
                column: "RequiredTalentUid");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_SkillId",
                schema: "Rules",
                table: "Talents",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_SkillUid",
                schema: "Rules",
                table: "Talents",
                column: "SkillUid");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_Slug",
                schema: "Rules",
                table: "Talents",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_SlugNormalized",
                schema: "Rules",
                table: "Talents",
                column: "SlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Talents_StreamId",
                schema: "Rules",
                table: "Talents",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Talents_Tier",
                schema: "Rules",
                table: "Talents",
                column: "Tier");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_UpdatedBy",
                schema: "Rules",
                table: "Talents",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_UpdatedOn",
                schema: "Rules",
                table: "Talents",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_Version",
                schema: "Rules",
                table: "Talents",
                column: "Version");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleHierarchy",
                schema: "Encyclopedia");

            migrationBuilder.DropTable(
                name: "Castes",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "Customizations",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "DoctrineDiscountedTalents",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "DoctrineFeatures",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "Educations",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "LineageFeatures",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "LineageLanguages",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "Quests",
                schema: "Encyclopedia");

            migrationBuilder.DropTable(
                name: "SpecializationOptionalTalents",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "SpellCategoryAssociations",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "SpellEffects",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "Statistics",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "Articles",
                schema: "Encyclopedia");

            migrationBuilder.DropTable(
                name: "Doctrines",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "Features",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "Languages",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "Lineages",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "QuestGroups",
                schema: "Encyclopedia");

            migrationBuilder.DropTable(
                name: "QuestLogs",
                schema: "Encyclopedia");

            migrationBuilder.DropTable(
                name: "SpellCategories",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "Spells",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "Collections",
                schema: "Encyclopedia");

            migrationBuilder.DropTable(
                name: "Specializations",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "Scripts",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "SpeciesCategories",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "Talents",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "Skills",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "Attributes",
                schema: "Rules");
        }
    }
}
