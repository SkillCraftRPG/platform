using FluentValidation;
using FluentValidation.Results;
using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Core.Spells;
using SkillCraft.Cms.Infrastructure.Contents;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record PublishSpellEffectCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishSpellEffectCommandHandler : ICommandHandler<PublishSpellEffectCommand, Unit>
{
  private readonly ILogger<PublishSpellEffectCommandHandler> _logger;
  private readonly RulesContext _rules;

  public PublishSpellEffectCommandHandler(ILogger<PublishSpellEffectCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(PublishSpellEffectCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    SpellEffectEntity? spellEffect = await _rules.SpellEffects.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (spellEffect is null)
    {
      spellEffect = new SpellEffectEntity(command.Event);
      _rules.SpellEffects.Add(spellEffect);
    }

    List<ValidationFailure> failures = new(capacity: 3);

    spellEffect.Key = locale.UniqueName.Value;
    spellEffect.Name = locale.DisplayName?.Value;

    await SetSpellAsync(spellEffect, invariant, failures, cancellationToken);
    spellEffect.Level = (int)invariant.GetNumber(SpellEffectDefinition.Level);

    SetCastingTime(spellEffect, invariant, failures);
    spellEffect.IsRitual = invariant.GetBoolean(SpellEffectDefinition.IsRitual);

    double? duration = invariant.TryGetNumber(SpellEffectDefinition.Duration);
    spellEffect.Duration = duration.HasValue ? (int)duration.Value : null;
    SetDurationTime(spellEffect, invariant, failures);
    spellEffect.IsConcentration = invariant.GetBoolean(SpellEffectDefinition.IsConcentration);

    spellEffect.Range = (int)invariant.GetNumber(SpellEffectDefinition.Range);

    spellEffect.IsSomatic = invariant.GetBoolean(SpellEffectDefinition.IsSomatic);
    spellEffect.IsVerbal = invariant.GetBoolean(SpellEffectDefinition.IsVerbal);
    spellEffect.Focus = locale.TryGetString(SpellEffectDefinition.Focus);
    spellEffect.Material = locale.TryGetString(SpellEffectDefinition.Material);

    spellEffect.HtmlContent = locale.TryGetString(SpellEffectDefinition.HtmlContent);

    spellEffect.Publish(@event);

    if (failures.Count > 0)
    {
      _rules.ChangeTracker.Clear();
      throw new ValidationException(failures);
    }

    await _rules.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The spell effect '{SpellEffect}' has been published.", spellEffect);

    return Unit.Value;
  }

  private static void SetCastingTime(SpellEffectEntity spellEffect, ContentLocale invariant, List<ValidationFailure> failures)
  {
    IReadOnlyCollection<string> castingTimes = invariant.GetSelect(SpellEffectDefinition.CastingTime);
    if (castingTimes.Count == 1)
    {
      spellEffect.CastingTime = castingTimes.Single();
    }
    else
    {
      failures.Add(new ValidationFailure(nameof(SpellEffectDefinition.CastingTime), "'{PropertyName}' must contain exactly one element.", castingTimes)
      {
        ErrorCode = castingTimes.Count < 1 ? ErrorCodes.EmptyValue : ErrorCodes.TooManyValues
      });
    }
  }

  private static void SetDurationTime(SpellEffectEntity spellEffect, ContentLocale invariant, List<ValidationFailure> failures)
  {
    IReadOnlyCollection<string> durationUnits = invariant.GetSelect(SpellEffectDefinition.DurationUnit);
    if (durationUnits.Count < 1)
    {
      spellEffect.DurationUnit = null;
    }
    else if (durationUnits.Count > 1)
    {
      failures.Add(new ValidationFailure(nameof(SpellEffectDefinition.DurationUnit), "'{PropertyName}' must contain exactly one element.", durationUnits)
      {
        ErrorCode = durationUnits.Count < 1 ? ErrorCodes.EmptyValue : ErrorCodes.TooManyValues
      });
    }
    else
    {
      string durationUnitValue = durationUnits.Single();
      if (Enum.TryParse(durationUnitValue, out TimeUnit durationUnit) && Enum.IsDefined(durationUnit))
      {
        spellEffect.DurationUnit = durationUnit;
      }
      else
      {
        failures.Add(new ValidationFailure(nameof(SpellEffectDefinition.DurationUnit), $"'{{PropertyName}}' must be parseable as an {nameof(TimeUnit)}.", durationUnitValue)
        {
          ErrorCode = ErrorCodes.InvalidEnumValue
        });
      }
    }
  }

  private async Task SetSpellAsync(SpellEffectEntity spellEffect, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> spellIds = invariant.GetRelatedContent(SpellEffectDefinition.Spell);
    if (spellIds.Count == 1)
    {
      Guid spellId = spellIds.Single();
      SpellEntity? spell = await _rules.Spells.SingleOrDefaultAsync(x => x.Id == spellId, cancellationToken);
      if (spell is null)
      {
        failures.Add(new ValidationFailure(nameof(SpellEffectDefinition.Spell), "'{PropertyName}' did not reference an existing entity.", spellId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
      else
      {
        spellEffect.SetSpell(spell);
      }
    }
    else
    {
      failures.Add(new ValidationFailure(nameof(SpellEffectDefinition.Spell), "'{PropertyName}' must contain exactly one element.", spellIds)
      {
        ErrorCode = spellIds.Count < 1 ? ErrorCodes.EmptyValue : ErrorCodes.TooManyValues
      });
    }
  }
}
