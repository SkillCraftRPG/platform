using Microsoft.EntityFrameworkCore;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure;

public class RulesContext : DbContext
{
  internal const string Schema = "Rules";

  public RulesContext(DbContextOptions<RulesContext> options) : base(options)
  {
  }

  internal DbSet<AttributeEntity> Attributes => Set<AttributeEntity>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}
