using Krakenar.Core;
using Krakenar.EntityFrameworkCore.Relational.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Configurations;

internal class CollectionConfiguration : AggregateConfiguration<CollectionEntity>, IEntityTypeConfiguration<CollectionEntity>
{
  public override void Configure(EntityTypeBuilder<CollectionEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(EncyclopediaDb.Collections.Table.Table!, EncyclopediaDb.Collections.Table.Schema);
    builder.HasKey(x => x.CollectionId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.IsPublished);
    builder.HasIndex(x => x.Slug);
    builder.HasIndex(x => x.SlugNormalized).IsUnique();
    builder.HasIndex(x => x.Name);

    builder.Property(x => x.Slug).HasMaxLength(Slug.MaximumLength);
    builder.Property(x => x.SlugNormalized).HasMaxLength(Slug.MaximumLength);
    builder.Property(x => x.Name).HasMaxLength(DisplayName.MaximumLength);
  }
}
