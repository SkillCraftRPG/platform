using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Configurations;

internal class ArticleHierarchyConfiguration : IEntityTypeConfiguration<ArticleHierarchyEntity>
{
  public void Configure(EntityTypeBuilder<ArticleHierarchyEntity> builder)
  {
    builder.ToTable(EncyclopediaDb.ArticleHierarchy.Table.Table!, EncyclopediaDb.ArticleHierarchy.Table.Schema);
    builder.HasKey(x => x.ArticleId);

    builder.HasIndex(x => x.ArticleUid).IsUnique();
    builder.HasIndex(x => x.CollectionId);
    builder.HasIndex(x => x.CollectionUid);
    builder.HasIndex(x => x.Depth);
    builder.HasIndex(x => x.IdPath);
    builder.HasIndex(x => x.SlugPath);

    builder.Property(x => x.IdPath).HasMaxLength(1024);
    builder.Property(x => x.SlugPath).HasMaxLength(1024);

    builder.HasOne(x => x.Article).WithOne(x => x.Hierarchy)
      .HasPrincipalKey<ArticleEntity>(x => x.ArticleId).HasForeignKey<ArticleHierarchyEntity>(x => x.ArticleId)
      .OnDelete(DeleteBehavior.Cascade);
    builder.HasOne(x => x.Collection).WithMany(x => x.ArticleHierarchies).OnDelete(DeleteBehavior.Cascade);
  }
}
