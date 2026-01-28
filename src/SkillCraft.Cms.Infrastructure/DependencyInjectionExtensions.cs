using Krakenar.EntityFrameworkCore.Relational;
using Krakenar.Infrastructure;
using Krakenar.Infrastructure.Commands;
using Logitar.CQRS;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Cms.Core.Attributes;
using SkillCraft.Cms.Core.Skills;
using SkillCraft.Cms.Core.Statistics;
using SkillCraft.Cms.Infrastructure.Commands;
using SkillCraft.Cms.Infrastructure.Materialization;
using SkillCraft.Cms.Infrastructure.Queriers;

namespace SkillCraft.Cms.Infrastructure;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddSkillCraftCmsInfrastructure(this IServiceCollection services)
  {
    ContentMaterializationHandlers.Register(services);
    return services
      .AddKrakenarInfrastructure()
      .AddKrakenarEntityFrameworkCoreRelational()
      .AddQueriers()
      .AddTransient<ICommandHandler<MigrateDatabase, Unit>, MigrateDatabaseCommandHandler>();
  }

  private static IServiceCollection AddQueriers(this IServiceCollection services)
  {
    return services
      .AddTransient<IAttributeQuerier, AttributeQuerier>()
      .AddTransient<ISkillQuerier, SkillQuerier>()
      .AddTransient<IStatisticQuerier, StatisticQuerier>();
  }
}
