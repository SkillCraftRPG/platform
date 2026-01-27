using FluentValidation;
using FluentValidation.Results;
using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Logitar;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Contents;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record PublishSpecializationCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishSpecializationCommandHandler : ICommandHandler<PublishSpecializationCommand, Unit>
{
  private readonly ILogger<PublishSpecializationCommandHandler> _logger;
  private readonly RulesContext _rules;

  public PublishSpecializationCommandHandler(ILogger<PublishSpecializationCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(PublishSpecializationCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    SpecializationEntity? specialization = await _rules.Specializations
      .Include(x => x.OptionalTalents)
      .SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (specialization is null)
    {
      specialization = new SpecializationEntity(command.Event);
      _rules.Specializations.Add(specialization);
    }

    List<ValidationFailure> failures = [];

    specialization.Slug = locale.GetString(SpecializationDefinition.Slug);
    specialization.Name = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    specialization.Tier = (int)invariant.GetNumber(SpecializationDefinition.Tier);
    await SetTalentsAsync(specialization, invariant, failures, cancellationToken);

    specialization.OtherRequirements = locale.TryGetString(SpecializationDefinition.OtherRequirements);
    specialization.OtherOptions = locale.TryGetString(SpecializationDefinition.OtherOptions);

    specialization.MetaDescription = locale.TryGetString(SpecializationDefinition.MetaDescription);
    specialization.Summary = locale.TryGetString(SpecializationDefinition.Summary);
    specialization.HtmlContent = locale.TryGetString(SpecializationDefinition.HtmlContent);

    specialization.Publish(@event);

    if (failures.Count > 0)
    {
      _rules.ChangeTracker.Clear();
      throw new ValidationException(failures);
    }

    await _rules.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The specialization '{Specialization}' has been published.", specialization);

    return Unit.Value;
  }

  private async Task SetTalentsAsync(SpecializationEntity specialization, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> mandatoryTalentIds = invariant.GetRelatedContent(SpecializationDefinition.MandatoryTalent);
    IReadOnlyCollection<Guid> optionalTalentIds = invariant.GetRelatedContent(SpecializationDefinition.OptionalTalents);

    HashSet<Guid> talentIds = new(capacity: optionalTalentIds.Count + 1);
    talentIds.AddRange(optionalTalentIds);

    Guid? mandatoryTalentId = null;
    if (mandatoryTalentIds.Count < 1)
    {
      specialization.SetMandatoryTalent(null);
    }
    else if (mandatoryTalentIds.Count > 1)
    {
      failures.Add(new ValidationFailure(nameof(SpecializationDefinition.MandatoryTalent), "'{PropertyName}' must contain at most one element.", mandatoryTalentIds)
      {
        ErrorCode = ErrorCodes.TooManyValues
      });
    }
    else
    {
      mandatoryTalentId = mandatoryTalentIds.Single();
      talentIds.Add(mandatoryTalentId.Value);
    }

    Dictionary<Guid, TalentEntity> talents = talentIds.Count < 1
      ? []
      : await _rules.Talents.Where(x => talentIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, x => x, cancellationToken);

    if (mandatoryTalentId.HasValue)
    {
      if (talents.TryGetValue(mandatoryTalentId.Value, out TalentEntity? mandatoryTalent))
      {
        specialization.SetMandatoryTalent(mandatoryTalent);
      }
      else
      {
        failures.Add(new ValidationFailure(nameof(SpecializationDefinition.MandatoryTalent), "'{PropertyName}' must reference an existing entity.", mandatoryTalentId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
    }

    foreach (SpecializationOptionalTalentEntity optionalTalent in specialization.OptionalTalents)
    {
      if (!talents.ContainsKey(optionalTalent.TalentUid))
      {
        _rules.SpecializationOptionalTalents.Remove(optionalTalent);
      }
    }

    HashSet<Guid> existingIds = specialization.OptionalTalents.Select(x => x.TalentUid).ToHashSet();
    foreach (Guid talentId in optionalTalentIds)
    {
      if (talents.TryGetValue(talentId, out TalentEntity? talent))
      {
        if (!existingIds.Contains(talentId))
        {
          specialization.AddOptionalTalent(talent);
        }
      }
      else
      {
        failures.Add(new ValidationFailure(nameof(SpecializationDefinition.OptionalTalents), "'{PropertyName}' must reference existing entities.", talentId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
    }
  }
}
