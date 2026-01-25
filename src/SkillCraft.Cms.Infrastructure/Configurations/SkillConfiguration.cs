using Krakenar.Core;
using Krakenar.EntityFrameworkCore.Relational.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkillCraft.Cms.Core.Skills;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Configurations;

internal class SkillConfiguration : AggregateConfiguration<SkillEntity>, IEntityTypeConfiguration<SkillEntity>
{
  public override void Configure(EntityTypeBuilder<SkillEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(RulesDb.Skills.Table.Table!, RulesDb.Skills.Table.Schema);
    builder.HasKey(x => x.SkillId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.IsPublished);
    builder.HasIndex(x => x.Slug);
    builder.HasIndex(x => x.SlugNormalized).IsUnique();
    builder.HasIndex(x => x.Value).IsUnique();
    builder.HasIndex(x => x.Name);
    builder.HasIndex(x => x.AttributeId);
    builder.HasIndex(x => x.AttributeUid);

    builder.Property(x => x.Slug).HasMaxLength(Slug.MaximumLength);
    builder.Property(x => x.SlugNormalized).HasMaxLength(Slug.MaximumLength);
    builder.Property(x => x.Value).HasMaxLength(16).HasConversion(new EnumToStringConverter<GameSkill>());
    builder.Property(x => x.Name).HasMaxLength(DisplayName.MaximumLength);
    builder.Property(x => x.MetaDescription).HasMaxLength(Constants.MetaDescriptionMaximumLength);
    builder.Property(x => x.Summary).HasMaxLength(Constants.SummaryMaximumLength);

    builder.HasOne(x => x.Attribute).WithMany(x => x.Skills).OnDelete(DeleteBehavior.Restrict);
  }
}
