using Krakenar.Core;
using Krakenar.EntityFrameworkCore.Relational.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Configurations;

internal class QuestGroupConfiguration : AggregateConfiguration<QuestGroupEntity>, IEntityTypeConfiguration<QuestGroupEntity>
{
  public override void Configure(EntityTypeBuilder<QuestGroupEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(EncyclopediaDb.QuestGroups.Table.Table!, EncyclopediaDb.QuestGroups.Table.Schema);
    builder.HasKey(x => x.QuestGroupId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.IsPublished);
    builder.HasIndex(x => x.Name);

    builder.Property(x => x.Name).HasMaxLength(DisplayName.MaximumLength);
  }
}
