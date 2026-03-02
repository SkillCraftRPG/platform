using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Configurations;

internal class SpellCategoryAssociationConfiguration : IEntityTypeConfiguration<SpellCategoryAssociationEntity>
{
  public void Configure(EntityTypeBuilder<SpellCategoryAssociationEntity> builder)
  {
    builder.ToTable(RulesDb.SpellCategoryAssociations.Table.Table!, RulesDb.SpellCategoryAssociations.Table.Schema);
    builder.HasKey(x => new { x.SpellId, x.SpellCategoryId });

    builder.HasIndex(x => x.SpellId);
    builder.HasIndex(x => x.SpellUid);
    builder.HasIndex(x => x.SpellCategoryId);
    builder.HasIndex(x => x.SpellCategoryUid);

    builder.HasOne(x => x.Spell).WithMany(x => x.Categories).OnDelete(DeleteBehavior.Cascade);
    builder.HasOne(x => x.SpellCategory).WithMany(x => x.Spells).OnDelete(DeleteBehavior.Cascade);
  }
}
