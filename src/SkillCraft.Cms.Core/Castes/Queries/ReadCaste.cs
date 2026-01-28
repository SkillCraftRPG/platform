using Krakenar.Contracts;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Castes.Models;

namespace SkillCraft.Cms.Core.Castes.Queries;

internal record ReadCasteQuery(Guid? Id, string? Slug) : IQuery<CasteModel?>;

internal class ReadCasteQueryHandler : IQueryHandler<ReadCasteQuery, CasteModel?>
{
  private readonly ICasteQuerier _casteQuerier;

  public ReadCasteQueryHandler(ICasteQuerier casteQuerier)
  {
    _casteQuerier = casteQuerier;
  }

  public async Task<CasteModel?> HandleAsync(ReadCasteQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, CasteModel> castes = new(capacity: 2);

    if (query.Id.HasValue)
    {
      CasteModel? caste = await _casteQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (caste is not null)
      {
        castes[caste.Id] = caste;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      CasteModel? caste = await _casteQuerier.ReadAsync(query.Slug, cancellationToken);
      if (caste is not null)
      {
        castes[caste.Id] = caste;
      }
    }

    if (castes.Count > 1)
    {
      throw TooManyResultsException<CasteModel>.ExpectedSingle(castes.Count);
    }

    return castes.Values.SingleOrDefault();
  }
}
