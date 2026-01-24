using Krakenar.EntityFrameworkCore.Relational;
using Krakenar.Infrastructure;
using Krakenar.Infrastructure.Commands;
using Logitar.CQRS;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Cms.Infrastructure.Commands;

namespace SkillCraft.Cms.Infrastructure;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddSkillCraftCmsInfrastructure(this IServiceCollection services)
  {
    return services
      .AddKrakenarInfrastructure()
      .AddKrakenarEntityFrameworkCoreRelational()
      .AddTransient<ICommandHandler<MigrateDatabase, Unit>, MigrateDatabaseCommandHandler>();
  }
}
