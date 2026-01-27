using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Configurations;

internal class DoctrineDiscountedTalentConfiguration : IEntityTypeConfiguration<DoctrineDiscountedTalentEntity>
{
  public void Configure(EntityTypeBuilder<DoctrineDiscountedTalentEntity> builder)
  {
    builder.ToTable(RulesDb.DoctrineDiscountedTalents.Table.Table!, RulesDb.DoctrineDiscountedTalents.Table.Schema);
    builder.HasKey(x => new { x.DoctrineId, x.TalentId });

    builder.HasIndex(x => x.DoctrineId);
    builder.HasIndex(x => x.DoctrineUid);
    builder.HasIndex(x => x.TalentId);
    builder.HasIndex(x => x.TalentUid);

    builder.HasOne(x => x.Doctrine).WithMany(x => x.DiscountedTalents).OnDelete(DeleteBehavior.Cascade);
    builder.HasOne(x => x.Talent).WithMany(x => x.DoctrinesDiscounted).OnDelete(DeleteBehavior.Cascade);
  }
}
