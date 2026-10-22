using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Configurations;

internal class SpecializationOriginConfiguration : IEntityTypeConfiguration<SpecializationOriginEntity>
{
  public void Configure(EntityTypeBuilder<SpecializationOriginEntity> builder)
  {
    builder.ToTable(RulesDb.SpecializationOrigins.Table.Table!, RulesDb.SpecializationOrigins.Table.Schema);
    builder.HasKey(x => new { x.SpecializationId, x.OriginId });

    builder.HasIndex(x => x.SpecializationId);
    builder.HasIndex(x => x.SpecializationUid);
    builder.HasIndex(x => x.OriginId);
    builder.HasIndex(x => x.OriginUid);

    builder.HasOne(x => x.Specialization).WithMany(x => x.Origins)
      .HasPrincipalKey(x => x.SpecializationId).HasForeignKey(x => x.SpecializationId)
      .OnDelete(DeleteBehavior.Cascade);
    builder.HasOne(x => x.Origin).WithMany(x => x.LeadsTo)
      .HasPrincipalKey(x => x.SpecializationId).HasForeignKey(x => x.OriginId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}
