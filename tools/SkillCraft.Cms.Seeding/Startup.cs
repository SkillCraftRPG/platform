using Krakenar.Core;
using Logitar.CQRS;
using SkillCraft.Cms.Core;
using SkillCraft.Cms.Infrastructure;
using SkillCraft.Cms.PostgreSQL;
using SkillCraft.Cms.Seeding.Krakenar.Tasks;
using SkillCraft.Cms.Seeding.Settings;

namespace SkillCraft.Cms.Seeding;

internal class Startup
{
  private readonly IConfiguration _configuration;

  public Startup(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public void ConfigureServices(IServiceCollection services)
  {
    services.AddSkillCraftCmsCore();
    services.AddSkillCraftCmsInfrastructure();
    services.AddSkillCraftCmsPostgreSQL(_configuration);

    services.AddHostedService<SeedingWorker>();
    services.AddSingleton(serviceProvider => DefaultSettings.Initialize(serviceProvider.GetRequiredService<IConfiguration>()));
    services.AddSingleton<IApplicationContext, SeedingApplicationContext>();

    services.AddTransient<ICommandHandler<InitializeConfigurationTask, Unit>, InitializeConfigurationTaskHandler>();
    services.AddTransient<ICommandHandler<MigrateDatabaseTask, Unit>, MigrateDatabaseTaskHandler>();
    services.AddTransient<ICommandHandler<SeedContentsTask, Unit>, SeedContentsTaskHandler>();
    services.AddTransient<ICommandHandler<SeedContentTypesTask, Unit>, SeedContentTypesTaskHandler>();
    services.AddTransient<ICommandHandler<SeedDictionariesTask, Unit>, SeedDictionariesTaskHandler>();
    services.AddTransient<ICommandHandler<SeedFieldTypesTask, Unit>, SeedFieldTypesTaskHandler>();
    services.AddTransient<ICommandHandler<SeedLanguagesTask, Unit>, SeedLanguagesTaskHandler>();
    services.AddTransient<ICommandHandler<SeedRealmTask, Unit>, SeedRealmTaskHandler>();
    services.AddTransient<ICommandHandler<SeedSendersTask, Unit>, SeedSendersTaskHandler>();
    services.AddTransient<ICommandHandler<SeedTemplatesTask, Unit>, SeedTemplatesTaskHandler>();
    services.AddTransient<ICommandHandler<SeedUsersTask, Unit>, SeedUsersTaskHandler>();
  }
}
