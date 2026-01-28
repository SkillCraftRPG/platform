using Krakenar.Contracts;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Customizations.Models;

namespace SkillCraft.Cms.Core.Customizations.Queries;

internal record ReadCustomizationQuery(Guid? Id, string? Slug) : IQuery<CustomizationModel?>;

internal class ReadCustomizationQueryHandler : IQueryHandler<ReadCustomizationQuery, CustomizationModel?>
{
  private readonly ICustomizationQuerier _customizationQuerier;

  public ReadCustomizationQueryHandler(ICustomizationQuerier customizationQuerier)
  {
    _customizationQuerier = customizationQuerier;
  }

  public async Task<CustomizationModel?> HandleAsync(ReadCustomizationQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, CustomizationModel> customizations = new(capacity: 2);

    if (query.Id.HasValue)
    {
      CustomizationModel? customization = await _customizationQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (customization is not null)
      {
        customizations[customization.Id] = customization;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      CustomizationModel? customization = await _customizationQuerier.ReadAsync(query.Slug, cancellationToken);
      if (customization is not null)
      {
        customizations[customization.Id] = customization;
      }
    }

    if (customizations.Count > 1)
    {
      throw TooManyResultsException<CustomizationModel>.ExpectedSingle(customizations.Count);
    }

    return customizations.Values.SingleOrDefault();
  }
}
