using Krakenar.Contracts.Search;
using Logitar.CQRS;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Cms.Core.Scripts.Models;
using SkillCraft.Cms.Core.Scripts.Queries;

namespace SkillCraft.Cms.Core.Scripts;

public interface IScriptService
{
  Task<ScriptModel?> ReadAsync(Guid? id = null, string? slug = null, CancellationToken cancellationToken = default);
  Task<SearchResults<ScriptModel>> SearchAsync(SearchScriptsPayload payload, CancellationToken cancellationToken = default);
}

internal class ScriptService : IScriptService
{
  public static void Register(IServiceCollection services)
  {
    services.AddTransient<IScriptService, ScriptService>();
    services.AddTransient<IQueryHandler<ReadScriptQuery, ScriptModel?>, ReadScriptQueryHandler>();
    services.AddTransient<IQueryHandler<SearchScriptsQuery, SearchResults<ScriptModel>>, SearchScriptsQueryHandler>();
  }

  private readonly IQueryBus _queryBus;

  public ScriptService(IQueryBus queryBus)
  {
    _queryBus = queryBus;
  }

  public async Task<ScriptModel?> ReadAsync(Guid? id, string? slug, CancellationToken cancellationToken)
  {
    ReadScriptQuery query = new(id, slug);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }

  public async Task<SearchResults<ScriptModel>> SearchAsync(SearchScriptsPayload payload, CancellationToken cancellationToken)
  {
    SearchScriptsQuery query = new(payload);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }
}
