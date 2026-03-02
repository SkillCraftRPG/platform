using Krakenar.Core;
using Krakenar.EntityFrameworkCore.Relational.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Configurations;

internal class SpellCategoryConfiguration : AggregateConfiguration<SpellCategoryEntity>, IEntityTypeConfiguration<SpellCategoryEntity>
{
  public override void Configure(EntityTypeBuilder<SpellCategoryEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(RulesDb.SpellCategories.Table.Table!, RulesDb.SpellCategories.Table.Schema);
    builder.HasKey(x => x.SpellCategoryId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.IsPublished);
    builder.HasIndex(x => x.Key);
    builder.HasIndex(x => x.KeyNormalized).IsUnique();
    builder.HasIndex(x => x.Name);
    builder.HasIndex(x => x.ParentId);
    builder.HasIndex(x => x.ParentUid);

    builder.Property(x => x.Key).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.KeyNormalized).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.Name).HasMaxLength(DisplayName.MaximumLength);

    builder.HasOne(x => x.Parent).WithMany(x => x.Children)
      .HasPrincipalKey(x => x.SpellCategoryId).HasForeignKey(x => x.ParentId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}
