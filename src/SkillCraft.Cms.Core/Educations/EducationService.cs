using Krakenar.Contracts.Search;
using Logitar.CQRS;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Cms.Core.Educations.Models;
using SkillCraft.Cms.Core.Educations.Queries;

namespace SkillCraft.Cms.Core.Educations;

public interface IEducationService
{
  Task<EducationModel?> ReadAsync(Guid? id = null, string? slug = null, CancellationToken cancellationToken = default);
  Task<SearchResults<EducationModel>> SearchAsync(SearchEducationsPayload payload, CancellationToken cancellationToken = default);
}

internal class EducationService : IEducationService
{
  public static void Register(IServiceCollection services)
  {
    services.AddTransient<IEducationService, EducationService>();
    services.AddTransient<IQueryHandler<ReadEducationQuery, EducationModel?>, ReadEducationQueryHandler>();
    services.AddTransient<IQueryHandler<SearchEducationsQuery, SearchResults<EducationModel>>, SearchEducationsQueryHandler>();
  }

  private readonly IQueryBus _queryBus;

  public EducationService(IQueryBus queryBus)
  {
    _queryBus = queryBus;
  }

  public async Task<EducationModel?> ReadAsync(Guid? id, string? slug, CancellationToken cancellationToken)
  {
    ReadEducationQuery query = new(id, slug);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }

  public async Task<SearchResults<EducationModel>> SearchAsync(SearchEducationsPayload payload, CancellationToken cancellationToken)
  {
    SearchEducationsQuery query = new(payload);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }
}
