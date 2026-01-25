using Krakenar.Core;
using Krakenar.EntityFrameworkCore.Relational.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkillCraft.Cms.Core.Customizations;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Configurations;

internal class CustomizationConfiguration : AggregateConfiguration<CustomizationEntity>, IEntityTypeConfiguration<CustomizationEntity>
{
  public override void Configure(EntityTypeBuilder<CustomizationEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(RulesDb.Customizations.Table.Table!, RulesDb.Customizations.Table.Schema);
    builder.HasKey(x => x.CustomizationId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.IsPublished);
    builder.HasIndex(x => x.Slug);
    builder.HasIndex(x => x.SlugNormalized).IsUnique();
    builder.HasIndex(x => x.Name);
    builder.HasIndex(x => x.Kind);

    builder.Property(x => x.Slug).HasMaxLength(Slug.MaximumLength);
    builder.Property(x => x.SlugNormalized).HasMaxLength(Slug.MaximumLength);
    builder.Property(x => x.Name).HasMaxLength(DisplayName.MaximumLength);
    builder.Property(x => x.Kind).HasMaxLength(16).HasConversion(new EnumToStringConverter<CustomizationKind>());
    builder.Property(x => x.MetaDescription).HasMaxLength(Constants.MetaDescriptionMaximumLength);
    builder.Property(x => x.Summary).HasMaxLength(Constants.SummaryMaximumLength);
  }
}
