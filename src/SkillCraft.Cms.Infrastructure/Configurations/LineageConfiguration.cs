using Krakenar.Core;
using Krakenar.EntityFrameworkCore.Relational.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkillCraft.Cms.Core.Lineages;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Configurations;

internal class LineageConfiguration : AggregateConfiguration<LineageEntity>, IEntityTypeConfiguration<LineageEntity>
{
  public override void Configure(EntityTypeBuilder<LineageEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(RulesDb.Lineages.Table.Table!, RulesDb.Lineages.Table.Schema);
    builder.HasKey(x => x.LineageId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.IsPublished);
    builder.HasIndex(x => x.Slug);
    builder.HasIndex(x => x.SlugNormalized).IsUnique();
    builder.HasIndex(x => x.Name);
    builder.HasIndex(x => x.ParentId);
    builder.HasIndex(x => x.ParentUid);
    builder.HasIndex(x => x.SizeCategory);

    builder.Property(x => x.Slug).HasMaxLength(Slug.MaximumLength);
    builder.Property(x => x.SlugNormalized).HasMaxLength(Slug.MaximumLength);
    builder.Property(x => x.Name).HasMaxLength(DisplayName.MaximumLength);
    builder.Property(x => x.SizeCategory).HasMaxLength(16).HasConversion(new EnumToStringConverter<SizeCategory>());
    builder.Property(x => x.SizeRoll).HasMaxLength(Constants.RollMaximumLength);
    builder.Property(x => x.Malnutrition).HasMaxLength(Constants.RollMaximumLength);
    builder.Property(x => x.Skinny).HasMaxLength(Constants.RollMaximumLength);
    builder.Property(x => x.NormalWeight).HasMaxLength(Constants.RollMaximumLength);
    builder.Property(x => x.Overweight).HasMaxLength(Constants.RollMaximumLength);
    builder.Property(x => x.Obese).HasMaxLength(Constants.RollMaximumLength);
    builder.Property(x => x.MetaDescription).HasMaxLength(Constants.MetaDescriptionMaximumLength);
    builder.Property(x => x.Summary).HasMaxLength(Constants.SummaryMaximumLength);

    builder.HasOne(x => x.Parent).WithMany(x => x.Children)
      .HasPrincipalKey(x => x.LineageId).HasForeignKey(x => x.ParentId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}
