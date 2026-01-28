using Krakenar.Contracts.Search;
using Logitar.CQRS;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Cms.Core.Talents.Models;
using SkillCraft.Cms.Core.Talents.Queries;

namespace SkillCraft.Cms.Core.Talents;

public interface ITalentService
{
  Task<TalentModel?> ReadAsync(Guid? id = null, string? slug = null, CancellationToken cancellationToken = default);
  Task<SearchResults<TalentModel>> SearchAsync(SearchTalentsPayload payload, CancellationToken cancellationToken = default);
}

internal class TalentService : ITalentService
{
  public static void Register(IServiceCollection services)
  {
    services.AddTransient<ITalentService, TalentService>();
    services.AddTransient<IQueryHandler<ReadTalentQuery, TalentModel?>, ReadTalentQueryHandler>();
    services.AddTransient<IQueryHandler<SearchTalentsQuery, SearchResults<TalentModel>>, SearchTalentsQueryHandler>();
  }

  private readonly IQueryBus _queryBus;

  public TalentService(IQueryBus queryBus)
  {
    _queryBus = queryBus;
  }

  public async Task<TalentModel?> ReadAsync(Guid? id, string? slug, CancellationToken cancellationToken)
  {
    ReadTalentQuery query = new(id, slug);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }

  public async Task<SearchResults<TalentModel>> SearchAsync(SearchTalentsPayload payload, CancellationToken cancellationToken)
  {
    SearchTalentsQuery query = new(payload);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }
}
