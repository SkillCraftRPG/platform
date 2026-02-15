using Krakenar.Core;
using Krakenar.EntityFrameworkCore.Relational.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Configurations;

internal class QuestConfiguration : AggregateConfiguration<QuestEntity>, IEntityTypeConfiguration<QuestEntity>
{
  public override void Configure(EntityTypeBuilder<QuestEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(EncyclopediaDb.Quests.Table.Table!, EncyclopediaDb.Quests.Table.Schema);
    builder.HasKey(x => x.QuestId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.IsPublished);
    builder.HasIndex(x => x.Title);
    builder.HasIndex(x => x.QuestLogId);
    builder.HasIndex(x => x.QuestLogUid);
    builder.HasIndex(x => x.QuestGroupId);
    builder.HasIndex(x => x.QuestGroupUid);
    builder.HasIndex(x => x.GrantedLevels);
    builder.HasIndex(x => x.ProgressRatio);

    builder.Property(x => x.Title).HasMaxLength(DisplayName.MaximumLength);

    builder.HasOne(x => x.QuestLog).WithMany(x => x.Quests).OnDelete(DeleteBehavior.Restrict);
    builder.HasOne(x => x.QuestGroup).WithMany(x => x.Quests).OnDelete(DeleteBehavior.Restrict);
  }
}
