using Krakenar.Core;
using Krakenar.EntityFrameworkCore.Relational.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkillCraft.Cms.Core.Spells;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Configurations;

internal class SpellEffectConfiguration : AggregateConfiguration<SpellEffectEntity>, IEntityTypeConfiguration<SpellEffectEntity>
{
  private const int CastingTimeMaximumLength = 8;
  private const int IngredientMaximumLength = 1024;

  public override void Configure(EntityTypeBuilder<SpellEffectEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(RulesDb.SpellEffects.Table.Table!, RulesDb.SpellEffects.Table.Schema);
    builder.HasKey(x => x.SpellEffectId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.IsPublished);
    builder.HasIndex(x => x.SpellId);
    builder.HasIndex(x => x.SpellUid);

    builder.Property(x => x.Name).HasMaxLength(DisplayName.MaximumLength);
    builder.Property(x => x.CastingTime).HasMaxLength(CastingTimeMaximumLength);
    builder.Property(x => x.DurationUnit).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<TimeUnit>());
    builder.Property(x => x.Focus).HasMaxLength(IngredientMaximumLength);
    builder.Property(x => x.Material).HasMaxLength(IngredientMaximumLength);

    builder.HasOne(x => x.Spell).WithMany(x => x.Effects).OnDelete(DeleteBehavior.Restrict);
  }
}
