using Krakenar.Core;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Cms.Core.Attributes;
using SkillCraft.Cms.Core.Castes;
using SkillCraft.Cms.Core.Customizations;
using SkillCraft.Cms.Core.Educations;
using SkillCraft.Cms.Core.Skills;
using SkillCraft.Cms.Core.Statistics;

namespace SkillCraft.Cms.Core;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddSkillCraftCmsCore(this IServiceCollection services)
  {
    return services
      .AddKrakenarCore()
      .AddCoreServices();
  }

  private static IServiceCollection AddCoreServices(this IServiceCollection services)
  {
    AttributeService.Register(services);
    CasteService.Register(services);
    CustomizationService.Register(services);
    EducationService.Register(services);
    SkillService.Register(services);
    StatisticService.Register(services);
    return services;
  }
}
