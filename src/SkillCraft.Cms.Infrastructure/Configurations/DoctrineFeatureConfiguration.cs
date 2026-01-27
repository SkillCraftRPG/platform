using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Configurations;

internal class DoctrineFeatureConfiguration : IEntityTypeConfiguration<DoctrineFeatureEntity>
{
  public void Configure(EntityTypeBuilder<DoctrineFeatureEntity> builder)
  {
    builder.ToTable(RulesDb.DoctrineFeatures.Table.Table!, RulesDb.DoctrineFeatures.Table.Schema);
    builder.HasKey(x => new { x.DoctrineId, x.FeatureId });

    builder.HasIndex(x => x.DoctrineId);
    builder.HasIndex(x => x.DoctrineUid);
    builder.HasIndex(x => x.FeatureId);
    builder.HasIndex(x => x.FeatureUid);

    builder.HasOne(x => x.Doctrine).WithMany(x => x.Features).OnDelete(DeleteBehavior.Cascade);
    builder.HasOne(x => x.Feature).WithMany(x => x.Doctrines).OnDelete(DeleteBehavior.Cascade);
  }
}
