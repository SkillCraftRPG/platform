using Krakenar.Contracts.Search;
using Logitar.CQRS;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Cms.Core.Spells.Models;
using SkillCraft.Cms.Core.Spells.Queries;

namespace SkillCraft.Cms.Core.Spells;

public interface ISpellService
{
  Task<SpellModel?> ReadAsync(Guid? id = null, string? slug = null, CancellationToken cancellationToken = default);
  Task<SearchResults<SpellModel>> SearchAsync(SearchSpellsPayload payload, CancellationToken cancellationToken = default);
}

internal class SpellService : ISpellService
{
  public static void Register(IServiceCollection services)
  {
    services.AddTransient<ISpellService, SpellService>();
    services.AddTransient<IQueryHandler<ReadSpellQuery, SpellModel?>, ReadSpellQueryHandler>();
    services.AddTransient<IQueryHandler<SearchSpellsQuery, SearchResults<SpellModel>>, SearchSpellsQueryHandler>();
  }

  private readonly IQueryBus _queryBus;

  public SpellService(IQueryBus queryBus)
  {
    _queryBus = queryBus;
  }

  public async Task<SpellModel?> ReadAsync(Guid? id, string? slug, CancellationToken cancellationToken)
  {
    ReadSpellQuery query = new(id, slug);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }

  public async Task<SearchResults<SpellModel>> SearchAsync(SearchSpellsPayload payload, CancellationToken cancellationToken)
  {
    SearchSpellsQuery query = new(payload);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }
}
