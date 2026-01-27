using Krakenar.Core;
using Logitar.CQRS;
using SkillCraft.Cms.Core;
using SkillCraft.Cms.Export.Tasks;
using SkillCraft.Cms.Infrastructure;
using SkillCraft.Cms.PostgreSQL;

namespace SkillCraft.Cms.Export;

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

    services.AddHostedService<ExportWorker>();
    services.AddSingleton<IApplicationContext, ExportApplicationContext>();

    services.AddTransient<ICommandHandler<ExportContentsTask, Unit>, ExportContentsTaskHandler>();
  }
}
