using Krakenar.Contracts;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Talents.Models;

namespace SkillCraft.Cms.Core.Talents.Queries;

internal record ReadTalentQuery(Guid? Id, string? Slug) : IQuery<TalentModel?>;

internal class ReadTalentQueryHandler : IQueryHandler<ReadTalentQuery, TalentModel?>
{
  private readonly ITalentQuerier _talentQuerier;

  public ReadTalentQueryHandler(ITalentQuerier talentQuerier)
  {
    _talentQuerier = talentQuerier;
  }

  public async Task<TalentModel?> HandleAsync(ReadTalentQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, TalentModel> talents = new(capacity: 2);

    if (query.Id.HasValue)
    {
      TalentModel? talent = await _talentQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (talent is not null)
      {
        talents[talent.Id] = talent;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      TalentModel? talent = await _talentQuerier.ReadAsync(query.Slug, cancellationToken);
      if (talent is not null)
      {
        talents[talent.Id] = talent;
      }
    }

    if (talents.Count > 1)
    {
      throw TooManyResultsException<TalentModel>.ExpectedSingle(talents.Count);
    }

    return talents.Values.SingleOrDefault();
  }
}
