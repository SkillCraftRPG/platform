using Krakenar.Contracts.Search;
using Logitar.CQRS;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Cms.Core.Specializations.Models;
using SkillCraft.Cms.Core.Specializations.Queries;

namespace SkillCraft.Cms.Core.Specializations;

public interface ISpecializationService
{
  Task<SpecializationModel?> ReadAsync(Guid? id = null, string? slug = null, CancellationToken cancellationToken = default);
  Task<SearchResults<SpecializationModel>> SearchAsync(SearchSpecializationsPayload payload, CancellationToken cancellationToken = default);
}

internal class SpecializationService : ISpecializationService
{
  public static void Register(IServiceCollection services)
  {
    services.AddTransient<ISpecializationService, SpecializationService>();
    services.AddTransient<IQueryHandler<ReadSpecializationQuery, SpecializationModel?>, ReadSpecializationQueryHandler>();
    services.AddTransient<IQueryHandler<SearchSpecializationsQuery, SearchResults<SpecializationModel>>, SearchSpecializationsQueryHandler>();
  }

  private readonly IQueryBus _queryBus;

  public SpecializationService(IQueryBus queryBus)
  {
    _queryBus = queryBus;
  }

  public async Task<SpecializationModel?> ReadAsync(Guid? id, string? slug, CancellationToken cancellationToken)
  {
    ReadSpecializationQuery query = new(id, slug);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }

  public async Task<SearchResults<SpecializationModel>> SearchAsync(SearchSpecializationsPayload payload, CancellationToken cancellationToken)
  {
    SearchSpecializationsQuery query = new(payload);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }
}
