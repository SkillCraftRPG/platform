using Microsoft.EntityFrameworkCore;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure;

public class EncyclopediaContext : DbContext
{
  internal const string Schema = "Encyclopedia";

  public EncyclopediaContext(DbContextOptions<EncyclopediaContext> options) : base(options)
  {
  }

  internal DbSet<CollectionEntity> Collections => Set<CollectionEntity>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}
