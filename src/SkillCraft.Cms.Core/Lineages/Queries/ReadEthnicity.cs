using Krakenar.Contracts;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Lineages.Models;

namespace SkillCraft.Cms.Core.Lineages.Queries;

internal record ReadEthnicityQuery(Guid? Id, string? Slug) : IQuery<EthnicityModel?>;

internal class ReadEthnicityQueryHandler : IQueryHandler<ReadEthnicityQuery, EthnicityModel?>
{
  private readonly IEthnicityQuerier _ethnicityQuerier;

  public ReadEthnicityQueryHandler(IEthnicityQuerier ethnicityQuerier)
  {
    _ethnicityQuerier = ethnicityQuerier;
  }

  public async Task<EthnicityModel?> HandleAsync(ReadEthnicityQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, EthnicityModel> ethnicies = new(capacity: 2);

    if (query.Id.HasValue)
    {
      EthnicityModel? ethnicity = await _ethnicityQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (ethnicity is not null)
      {
        ethnicies[ethnicity.Id] = ethnicity;
      }
    }

    //if (!string.IsNullOrWhiteSpace(query.Slug))
    //{
    //  IReadOnlyCollection<EthnicityModel> found = await _ethnicityQuerier.ReadAsync(query.Slug, cancellationToken);
    //  if (found.Count == 1)
    //  {
    //    EthnicityModel ethnicity = found.Single();
    //    ethnicies[ethnicity.Id] = ethnicity;
    //  }
    //} // TODO(fpion): implement

    if (ethnicies.Count > 1)
    {
      throw TooManyResultsException<EthnicityModel>.ExpectedSingle(ethnicies.Count);
    }

    return ethnicies.Values.SingleOrDefault();
  }
}
