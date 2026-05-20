using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Configurations;

internal class ArticleMapConfiguration : IEntityTypeConfiguration<ArticleMapEntity>
{
  public void Configure(EntityTypeBuilder<ArticleMapEntity> builder)
  {
    builder.ToTable(EncyclopediaDb.ArticleMaps.Table.Table!, EncyclopediaDb.ArticleMaps.Table.Schema);
    builder.HasKey(x => new { x.ArticleId, x.MapId });

    builder.HasIndex(x => x.ArticleId);
    builder.HasIndex(x => x.ArticleUid);
    builder.HasIndex(x => x.MapId);
    builder.HasIndex(x => x.MapUid);

    builder.HasOne(x => x.Article).WithMany(x => x.Maps).OnDelete(DeleteBehavior.Cascade);
    builder.HasOne(x => x.Map).WithMany(x => x.Articles).OnDelete(DeleteBehavior.Cascade);
  }
}
