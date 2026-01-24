using Microsoft.EntityFrameworkCore;

namespace SkillCraft.Cms.Infrastructure;

public class RulesContext : DbContext
{
  public RulesContext(DbContextOptions<RulesContext> options) : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}
