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

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}
