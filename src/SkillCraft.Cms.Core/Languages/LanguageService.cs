using Krakenar.Contracts.Search;
using Logitar.CQRS;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Cms.Core.Languages.Models;
using SkillCraft.Cms.Core.Languages.Queries;

namespace SkillCraft.Cms.Core.Languages;

public interface ILanguageService
{
  Task<LanguageModel?> ReadAsync(Guid? id = null, string? slug = null, CancellationToken cancellationToken = default);
  Task<SearchResults<LanguageModel>> SearchAsync(SearchLanguagesPayload payload, CancellationToken cancellationToken = default);
}

internal class LanguageService : ILanguageService
{
  public static void Register(IServiceCollection services)
  {
    services.AddTransient<ILanguageService, LanguageService>();
    services.AddTransient<IQueryHandler<ReadLanguageQuery, LanguageModel?>, ReadLanguageQueryHandler>();
    services.AddTransient<IQueryHandler<SearchLanguagesQuery, SearchResults<LanguageModel>>, SearchLanguagesQueryHandler>();
  }

  private readonly IQueryBus _queryBus;

  public LanguageService(IQueryBus queryBus)
  {
    _queryBus = queryBus;
  }

  public async Task<LanguageModel?> ReadAsync(Guid? id, string? slug, CancellationToken cancellationToken)
  {
    ReadLanguageQuery query = new(id, slug);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }

  public async Task<SearchResults<LanguageModel>> SearchAsync(SearchLanguagesPayload payload, CancellationToken cancellationToken)
  {
    SearchLanguagesQuery query = new(payload);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }
}
