using Krakenar.Contracts.Search;
using Logitar.CQRS;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Cms.Core.Customizations.Models;
using SkillCraft.Cms.Core.Customizations.Queries;

namespace SkillCraft.Cms.Core.Customizations;

public interface ICustomizationService
{
  Task<CustomizationModel?> ReadAsync(Guid? id = null, string? slug = null, CancellationToken cancellationToken = default);
  Task<SearchResults<CustomizationModel>> SearchAsync(SearchCustomizationsPayload payload, CancellationToken cancellationToken = default);
}

internal class CustomizationService : ICustomizationService
{
  public static void Register(IServiceCollection services)
  {
    services.AddTransient<ICustomizationService, CustomizationService>();
    services.AddTransient<IQueryHandler<ReadCustomizationQuery, CustomizationModel?>, ReadCustomizationQueryHandler>();
    services.AddTransient<IQueryHandler<SearchCustomizationsQuery, SearchResults<CustomizationModel>>, SearchCustomizationsQueryHandler>();
  }

  private readonly IQueryBus _queryBus;

  public CustomizationService(IQueryBus queryBus)
  {
    _queryBus = queryBus;
  }

  public async Task<CustomizationModel?> ReadAsync(Guid? id, string? slug, CancellationToken cancellationToken)
  {
    ReadCustomizationQuery query = new(id, slug);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }

  public async Task<SearchResults<CustomizationModel>> SearchAsync(SearchCustomizationsPayload payload, CancellationToken cancellationToken)
  {
    SearchCustomizationsQuery query = new(payload);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }
}
