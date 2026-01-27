using Krakenar.Core;
using Krakenar.EntityFrameworkCore.Relational.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Configurations;

internal class DoctrineConfiguration : AggregateConfiguration<DoctrineEntity>, IEntityTypeConfiguration<DoctrineEntity>
{
  public override void Configure(EntityTypeBuilder<DoctrineEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(RulesDb.Doctrines.Table.Table!, RulesDb.Doctrines.Table.Schema);
    builder.HasKey(x => x.DoctrineId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.IsPublished);
    builder.HasIndex(x => x.Key);
    builder.HasIndex(x => x.KeyNormalized).IsUnique();
    builder.HasIndex(x => x.Name);
    builder.HasIndex(x => x.SpecializationId).IsUnique();
    builder.HasIndex(x => x.SpecializationUid);

    builder.Property(x => x.Key).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.KeyNormalized).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.Name).HasMaxLength(DisplayName.MaximumLength);

    builder.HasOne(x => x.Specialization).WithOne(x => x.Doctrine)
      .HasPrincipalKey<SpecializationEntity>(x => x.SpecializationId).HasForeignKey<DoctrineEntity>(x => x.SpecializationId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}
