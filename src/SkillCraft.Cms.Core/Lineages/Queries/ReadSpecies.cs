using Krakenar.Contracts;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Lineages.Models;

namespace SkillCraft.Cms.Core.Lineages.Queries;

internal record ReadSpeciesQuery(Guid? Id, string? Slug) : IQuery<SpeciesModel?>;

internal class ReadSpeciesQueryHandler : IQueryHandler<ReadSpeciesQuery, SpeciesModel?>
{
  private readonly ISpeciesQuerier _speciesQuerier;

  public ReadSpeciesQueryHandler(ISpeciesQuerier speciesQuerier)
  {
    _speciesQuerier = speciesQuerier;
  }

  public async Task<SpeciesModel?> HandleAsync(ReadSpeciesQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, SpeciesModel> speciesById = new(capacity: 2);

    if (query.Id.HasValue)
    {
      SpeciesModel? species = await _speciesQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (species is not null)
      {
        speciesById[species.Id] = species;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      IReadOnlyCollection<SpeciesModel> speciesList = await _speciesQuerier.ReadAsync(query.Slug, cancellationToken);
      if (speciesList.Count == 1)
      {
        SpeciesModel species = speciesList.Single();
        speciesById[species.Id] = species;
      }
    }

    if (speciesById.Count > 1)
    {
      throw TooManyResultsException<SpeciesModel>.ExpectedSingle(speciesById.Count);
    }

    return speciesById.Values.SingleOrDefault();
  }
}
