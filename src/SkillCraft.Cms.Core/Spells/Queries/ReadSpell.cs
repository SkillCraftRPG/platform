using Krakenar.Contracts;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Spells.Models;

namespace SkillCraft.Cms.Core.Spells.Queries;

internal record ReadSpellQuery(Guid? Id, string? Slug) : IQuery<SpellModel?>;

internal class ReadSpellQueryHandler : IQueryHandler<ReadSpellQuery, SpellModel?>
{
  private readonly ISpellQuerier _spellQuerier;

  public ReadSpellQueryHandler(ISpellQuerier spellQuerier)
  {
    _spellQuerier = spellQuerier;
  }

  public async Task<SpellModel?> HandleAsync(ReadSpellQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, SpellModel> spells = new(capacity: 2);

    if (query.Id.HasValue)
    {
      SpellModel? spell = await _spellQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (spell is not null)
      {
        spells[spell.Id] = spell;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      SpellModel? spell = await _spellQuerier.ReadAsync(query.Slug, cancellationToken);
      if (spell is not null)
      {
        spells[spell.Id] = spell;
      }
    }

    if (spells.Count > 1)
    {
      throw TooManyResultsException<SpellModel>.ExpectedSingle(spells.Count);
    }

    return spells.Values.SingleOrDefault();
  }
}
