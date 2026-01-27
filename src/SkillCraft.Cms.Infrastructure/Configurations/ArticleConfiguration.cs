using Krakenar.Core;
using Krakenar.EntityFrameworkCore.Relational.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Configurations;

internal class ArticleConfiguration : AggregateConfiguration<ArticleEntity>, IEntityTypeConfiguration<ArticleEntity>
{
  public override void Configure(EntityTypeBuilder<ArticleEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(RulesDb.Articles.Table.Table!, RulesDb.Articles.Table.Schema);
    builder.HasKey(x => x.ArticleId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.IsPublished);
    builder.HasIndex(x => x.Slug);
    builder.HasIndex(x => x.SlugNormalized).IsUnique();
    builder.HasIndex(x => x.Title);
    builder.HasIndex(x => x.CollectionId);
    builder.HasIndex(x => x.CollectionUid);
    builder.HasIndex(x => x.ParentId);
    builder.HasIndex(x => x.ParentUid);

    builder.Property(x => x.Slug).HasMaxLength(Slug.MaximumLength);
    builder.Property(x => x.SlugNormalized).HasMaxLength(Slug.MaximumLength);
    builder.Property(x => x.Title).HasMaxLength(DisplayName.MaximumLength);
    builder.Property(x => x.MetaDescription).HasMaxLength(Constants.MetaDescriptionMaximumLength);

    builder.HasOne(x => x.Collection).WithMany(x => x.Articles).OnDelete(DeleteBehavior.Restrict);
  }
}
