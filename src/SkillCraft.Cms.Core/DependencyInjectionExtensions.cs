using Krakenar.Core;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Cms.Core.Attributes;
using SkillCraft.Cms.Core.Castes;
using SkillCraft.Cms.Core.Customizations;
using SkillCraft.Cms.Core.Educations;
using SkillCraft.Cms.Core.Languages;
using SkillCraft.Cms.Core.Lineages;
using SkillCraft.Cms.Core.Scripts;
using SkillCraft.Cms.Core.Skills;
using SkillCraft.Cms.Core.Spells;
using SkillCraft.Cms.Core.Statistics;
using SkillCraft.Cms.Core.Talents;

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
    LanguageService.Register(services);
    LineageService.Register(services);
    SkillService.Register(services);
    ScriptService.Register(services);
    SpellService.Register(services);
    StatisticService.Register(services);
    TalentService.Register(services);
    return services;
  }
}
