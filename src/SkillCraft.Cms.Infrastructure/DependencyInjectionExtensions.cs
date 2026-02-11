using Krakenar.EntityFrameworkCore.Relational;
using Krakenar.Infrastructure;
using Krakenar.Infrastructure.Commands;
using Logitar.CQRS;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Cms.Core.Articles;
using SkillCraft.Cms.Core.Attributes;
using SkillCraft.Cms.Core.Castes;
using SkillCraft.Cms.Core.Collections;
using SkillCraft.Cms.Core.Customizations;
using SkillCraft.Cms.Core.Educations;
using SkillCraft.Cms.Core.Languages;
using SkillCraft.Cms.Core.Lineages;
using SkillCraft.Cms.Core.Scripts;
using SkillCraft.Cms.Core.Skills;
using SkillCraft.Cms.Core.Specializations;
using SkillCraft.Cms.Core.Spells;
using SkillCraft.Cms.Core.Statistics;
using SkillCraft.Cms.Core.Talents;
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
      .AddTransient<IArticleQuerier, ArticleQuerier>()
      .AddTransient<IAttributeQuerier, AttributeQuerier>()
      .AddTransient<ICasteQuerier, CasteQuerier>()
      .AddTransient<ICollectionQuerier, CollectionQuerier>()
      .AddTransient<ICustomizationQuerier, CustomizationQuerier>()
      .AddTransient<IEducationQuerier, EducationQuerier>()
      .AddTransient<IEthnicityQuerier, EthnicityQuerier>()
      .AddTransient<ILanguageQuerier, LanguageQuerier>()
      .AddTransient<ILineageQuerier, LineageQuerier>()
      .AddTransient<IScriptQuerier, ScriptQuerier>()
      .AddTransient<ISkillQuerier, SkillQuerier>()
      .AddTransient<ISpecializationQuerier, SpecializationQuerier>()
      .AddTransient<ISpeciesQuerier, SpeciesQuerier>()
      .AddTransient<ISpellQuerier, SpellQuerier>()
      .AddTransient<IStatisticQuerier, StatisticQuerier>()
      .AddTransient<ITalentQuerier, TalentQuerier>();
  }
}
