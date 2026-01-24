using Krakenar.EntityFrameworkCore.PostgreSQL;
using Logitar;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Cms.Infrastructure;

namespace SkillCraft.Cms.PostgreSQL;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddSkillCraftCmsPostgreSQL(this IServiceCollection services, IConfiguration configuration)
  {
    string? connectionString = EnvironmentHelper.TryGetString("POSTGRESQLCONNSTR_Krakenar") ?? configuration.GetConnectionString("PostgreSQL");
    if (string.IsNullOrWhiteSpace(connectionString))
    {
      throw new ArgumentException("The connection string for the database provider 'PostgreSQL' could not be found.", nameof(configuration));
    }

    return services
      .AddKrakenarEntityFrameworkCorePostgreSQL(connectionString)
      .AddDbContext<RulesContext>(options => options.UseNpgsql(connectionString, options => options.MigrationsAssembly("SkillCraft.Cms.PostgreSQL")));
  }
}
