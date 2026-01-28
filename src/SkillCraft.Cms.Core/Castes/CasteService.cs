using Krakenar.Contracts.Search;
using Logitar.CQRS;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Cms.Core.Castes.Models;
using SkillCraft.Cms.Core.Castes.Queries;

namespace SkillCraft.Cms.Core.Castes;

public interface ICasteService
{
  Task<CasteModel?> ReadAsync(Guid? id = null, string? slug = null, CancellationToken cancellationToken = default);
  Task<SearchResults<CasteModel>> SearchAsync(SearchCastesPayload payload, CancellationToken cancellationToken = default);
}

internal class CasteService : ICasteService
{
  public static void Register(IServiceCollection services)
  {
    services.AddTransient<ICasteService, CasteService>();
    services.AddTransient<IQueryHandler<ReadCasteQuery, CasteModel?>, ReadCasteQueryHandler>();
    services.AddTransient<IQueryHandler<SearchCastesQuery, SearchResults<CasteModel>>, SearchCastesQueryHandler>();
  }

  private readonly IQueryBus _queryBus;

  public CasteService(IQueryBus queryBus)
  {
    _queryBus = queryBus;
  }

  public async Task<CasteModel?> ReadAsync(Guid? id, string? slug, CancellationToken cancellationToken)
  {
    ReadCasteQuery query = new(id, slug);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }

  public async Task<SearchResults<CasteModel>> SearchAsync(SearchCastesPayload payload, CancellationToken cancellationToken)
  {
    SearchCastesQuery query = new(payload);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }
}
