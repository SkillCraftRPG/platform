using Krakenar.Contracts.Search;
using Logitar.CQRS;
using Microsoft.Extensions.DependencyInjection;
using SkillCraft.Cms.Core.Attributes.Models;
using SkillCraft.Cms.Core.Attributes.Queries;

namespace SkillCraft.Cms.Core.Attributes;

public interface IAttributeService
{
  Task<AttributeModel?> ReadAsync(Guid? id = null, string? slug = null, CancellationToken cancellationToken = default);
  Task<SearchResults<AttributeModel>> SearchAsync(SearchAttributesPayload payload, CancellationToken cancellationToken = default);
}

internal class AttributeService : IAttributeService
{
  public static void Register(IServiceCollection services)
  {
    services.AddTransient<IAttributeService, AttributeService>();
    services.AddTransient<IQueryHandler<ReadAttributeQuery, AttributeModel?>, ReadAttributeQueryHandler>();
    services.AddTransient<IQueryHandler<SearchAttributesQuery, SearchResults<AttributeModel>>, SearchAttributesQueryHandler>();
  }

  private readonly IQueryBus _queryBus;

  public AttributeService(IQueryBus queryBus)
  {
    _queryBus = queryBus;
  }

  public async Task<AttributeModel?> ReadAsync(Guid? id, string? slug, CancellationToken cancellationToken)
  {
    ReadAttributeQuery query = new(id, slug);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }

  public async Task<SearchResults<AttributeModel>> SearchAsync(SearchAttributesPayload payload, CancellationToken cancellationToken)
  {
    SearchAttributesQuery query = new(payload);
    return await _queryBus.ExecuteAsync(query, cancellationToken);
  }
}
