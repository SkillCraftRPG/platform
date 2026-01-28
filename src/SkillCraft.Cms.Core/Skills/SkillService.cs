using Krakenar.Contracts.Search;
using Logitar.CQRS;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Cms.Core.Skills.Models;
using SkillCraft.Cms.Core.Skills.Queries;

namespace SkillCraft.Cms.Core.Skills;

public interface ISkillService
{
  Task<SkillModel?> ReadAsync(Guid? id = null, string? slug = null, CancellationToken cancellationToken = default);
  Task<SearchResults<SkillModel>> SearchAsync(SearchSkillsPayload payload, CancellationToken cancellationToken = default);
}

internal class SkillService : ISkillService
{
  public static void Register(IServiceCollection services)
  {
    services.AddTransient<ISkillService, SkillService>();
    services.AddTransient<IQueryHandler<ReadSkillQuery, SkillModel?>, ReadSkillQueryHandler>();
    services.AddTransient<IQueryHandler<SearchSkillsQuery, SearchResults<SkillModel>>, SearchSkillsQueryHandler>();
  }

  private readonly IQueryBus _queryBus;

  public SkillService(IQueryBus queryBus)
  {
    _queryBus = queryBus;
  }

  public async Task<SkillModel?> ReadAsync(Guid? id, string? slug, CancellationToken cancellationToken)
  {
    ReadSkillQuery query = new(id, slug);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }

  public async Task<SearchResults<SkillModel>> SearchAsync(SearchSkillsPayload payload, CancellationToken cancellationToken)
  {
    SearchSkillsQuery query = new(payload);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }
}
