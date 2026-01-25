using Krakenar.Core;
using Krakenar.EntityFrameworkCore.Relational.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkillCraft.Cms.Core.Attributes;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Configurations;

internal class AttributeConfiguration : AggregateConfiguration<AttributeEntity>, IEntityTypeConfiguration<AttributeEntity>
{
  public override void Configure(EntityTypeBuilder<AttributeEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(RulesDb.Attributes.Table.Table!, RulesDb.Attributes.Table.Schema);
    builder.HasKey(x => x.AttributeId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.IsPublished);
    builder.HasIndex(x => x.Slug);
    builder.HasIndex(x => x.SlugNormalized).IsUnique();
    builder.HasIndex(x => x.Value).IsUnique();
    builder.HasIndex(x => x.Name);
    builder.HasIndex(x => x.Category);

    builder.Property(x => x.Slug).HasMaxLength(Slug.MaximumLength);
    builder.Property(x => x.SlugNormalized).HasMaxLength(Slug.MaximumLength);
    builder.Property(x => x.Value).HasMaxLength(16).HasConversion(new EnumToStringConverter<GameAttribute>());
    builder.Property(x => x.Name).HasMaxLength(DisplayName.MaximumLength);
    builder.Property(x => x.Category).HasMaxLength(8).HasConversion(new EnumToStringConverter<AttributeCategory>());
    builder.Property(x => x.MetaDescription).HasMaxLength(Constants.MetaDescriptionMaximumLength);
    builder.Property(x => x.Summary).HasMaxLength(Constants.SummaryMaximumLength);
  }
}
