using Krakenar.Contracts;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Skills.Models;

namespace SkillCraft.Cms.Core.Skills.Queries;

internal record ReadSkillQuery(Guid? Id, string? Slug) : IQuery<SkillModel?>;

internal class ReadSkillQueryHandler : IQueryHandler<ReadSkillQuery, SkillModel?>
{
  private readonly ISkillQuerier _skillQuerier;

  public ReadSkillQueryHandler(ISkillQuerier skillQuerier)
  {
    _skillQuerier = skillQuerier;
  }

  public async Task<SkillModel?> HandleAsync(ReadSkillQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, SkillModel> skills = new(capacity: 2);

    if (query.Id.HasValue)
    {
      SkillModel? skill = await _skillQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (skill is not null)
      {
        skills[skill.Id] = skill;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      SkillModel? skill = await _skillQuerier.ReadAsync(query.Slug, cancellationToken);
      if (skill is not null)
      {
        skills[skill.Id] = skill;
      }
    }

    if (skills.Count > 1)
    {
      throw TooManyResultsException<SkillModel>.ExpectedSingle(skills.Count);
    }

    return skills.Values.SingleOrDefault();
  }
}
