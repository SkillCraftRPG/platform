using Microsoft.EntityFrameworkCore;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure;

public class EncyclopediaContext : DbContext
{
  internal const string Schema = "Encyclopedia";

  public EncyclopediaContext(DbContextOptions<EncyclopediaContext> options) : base(options)
  {
  }

  internal DbSet<ArticleEntity> Articles => Set<ArticleEntity>();
  internal DbSet<ArticleHierarchyEntity> ArticleHierarchy => Set<ArticleHierarchyEntity>();
  internal DbSet<CollectionEntity> Collections => Set<CollectionEntity>();
  internal DbSet<QuestEntity> Quests => Set<QuestEntity>();
  internal DbSet<QuestLogEntity> QuestLogs => Set<QuestLogEntity>();
  internal DbSet<QuestGroupEntity> QuestGroups => Set<QuestGroupEntity>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}
