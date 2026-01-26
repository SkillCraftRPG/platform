using FluentValidation;
using FluentValidation.Results;
using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Contents;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record PublishTalentCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishTalentCommandHandler : ICommandHandler<PublishTalentCommand, Unit>
{
  private readonly ILogger<PublishTalentCommandHandler> _logger;
  private readonly RulesContext _rules;

  public PublishTalentCommandHandler(ILogger<PublishTalentCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(PublishTalentCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    TalentEntity? talent = await _rules.Talents.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (talent is null)
    {
      talent = new TalentEntity(command.Event);
      _rules.Talents.Add(talent);
    }

    List<ValidationFailure> failures = new(capacity: 2);

    talent.Slug = locale.GetString(TalentDefinition.Slug);
    talent.Name = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    talent.Tier = (int)invariant.GetNumber(TalentDefinition.Tier);
    talent.AllowMultiplePurchases = invariant.GetBoolean(TalentDefinition.AllowMultiplePurchases);
    await SetSkillAsync(talent, invariant, failures, cancellationToken);
    await SetRequiredTalentAsync(talent, invariant, failures, cancellationToken);

    talent.MetaDescription = locale.TryGetString(TalentDefinition.MetaDescription);
    talent.Summary = locale.TryGetString(TalentDefinition.Summary);
    talent.HtmlContent = locale.TryGetString(TalentDefinition.HtmlContent);

    talent.Publish(@event);

    if (failures.Count > 0)
    {
      _rules.ChangeTracker.Clear();
      throw new ValidationException(failures);
    }

    await _rules.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The talent '{Talent}' has been published.", talent);

    return Unit.Value;
  }

  private async Task SetRequiredTalentAsync(TalentEntity talent, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> requiredTalentIds = invariant.GetRelatedContent(TalentDefinition.RequiredTalent);
    if (requiredTalentIds.Count < 1)
    {
      talent.SetRequiredTalent(null);
    }
    else if (requiredTalentIds.Count > 1)
    {
      failures.Add(new ValidationFailure(nameof(TalentDefinition.RequiredTalent), "'{PropertyName}' must contain at most one element.", requiredTalentIds)
      {
        ErrorCode = ErrorCodes.TooManyValues
      });
    }
    else
    {
      Guid requiredTalentId = requiredTalentIds.Single();
      TalentEntity? requiredTalent = await _rules.Talents.SingleOrDefaultAsync(x => x.Id == requiredTalentId, cancellationToken);
      if (requiredTalent is null)
      {
        failures.Add(new ValidationFailure(nameof(TalentDefinition.RequiredTalent), "'{PropertyName}' did not reference an existing entity.", requiredTalentId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
      else
      {
        talent.SetRequiredTalent(requiredTalent);
      }
    }
  }

  private async Task SetSkillAsync(TalentEntity talent, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> skillIds = invariant.GetRelatedContent(TalentDefinition.Skill);
    if (skillIds.Count < 1)
    {
      talent.SetSkill(null);
    }
    else if (skillIds.Count > 1)
    {
      failures.Add(new ValidationFailure(nameof(TalentDefinition.Skill), "'{PropertyName}' must contain at most one element.", skillIds)
      {
        ErrorCode = ErrorCodes.TooManyValues
      });
    }
    else
    {
      Guid skillId = skillIds.Single();
      SkillEntity? skill = await _rules.Skills.SingleOrDefaultAsync(x => x.Id == skillId, cancellationToken);
      if (skill is null)
      {
        failures.Add(new ValidationFailure(nameof(TalentDefinition.Skill), "'{PropertyName}' did not reference an existing entity.", skillId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
      else
      {
        talent.SetSkill(skill);
      }
    }
  }
}
