using Krakenar.Core;
using Krakenar.EntityFrameworkCore.Relational.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Configurations;

internal class MarkerConfiguration : AggregateConfiguration<MarkerEntity>, IEntityTypeConfiguration<MarkerEntity>
{
  public override void Configure(EntityTypeBuilder<MarkerEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(EncyclopediaDb.Markers.Table.Table!, EncyclopediaDb.Markers.Table.Schema);
    builder.HasKey(x => x.MarkerId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.IsPublished);
    builder.HasIndex(x => x.Key);
    builder.HasIndex(x => x.Title);
    builder.HasIndex(x => x.MapId);
    builder.HasIndex(x => x.MapUid);
    builder.HasIndex(x => x.X);
    builder.HasIndex(x => x.Y);

    builder.Property(x => x.Title).HasMaxLength(DisplayName.MaximumLength);

    builder.HasOne(x => x.Map).WithMany(x => x.Markers).OnDelete(DeleteBehavior.Restrict);
  }
}
