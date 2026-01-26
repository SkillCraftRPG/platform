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

internal record PublishCasteCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishCasteCommandHandler : ICommandHandler<PublishCasteCommand, Unit>
{
  private readonly ILogger<PublishCasteCommandHandler> _logger;
  private readonly RulesContext _rules;

  public PublishCasteCommandHandler(ILogger<PublishCasteCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(PublishCasteCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    CasteEntity? caste = await _rules.Castes.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (caste is null)
    {
      caste = new CasteEntity(command.Event);
      _rules.Castes.Add(caste);
    }

    List<ValidationFailure> failures = new(capacity: 2);

    caste.Slug = locale.GetString(CasteDefinition.Slug);
    caste.Name = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    caste.WealthRoll = invariant.TryGetString(CasteDefinition.WealthRoll);
    await SetSkillAsync(caste, invariant, failures, cancellationToken);
    await SetFeatureAsync(caste, invariant, failures, cancellationToken);

    caste.MetaDescription = locale.TryGetString(CasteDefinition.MetaDescription);
    caste.Summary = locale.TryGetString(CasteDefinition.Summary);
    caste.HtmlContent = locale.TryGetString(CasteDefinition.HtmlContent);

    caste.Publish(@event);

    if (failures.Count > 0)
    {
      _rules.ChangeTracker.Clear();
      throw new ValidationException(failures);
    }

    await _rules.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The caste '{Caste}' has been published.", caste);

    return Unit.Value;
  }

  private async Task SetFeatureAsync(CasteEntity caste, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> featureIds = invariant.GetRelatedContent(CasteDefinition.Feature);
    if (featureIds.Count < 1)
    {
      caste.SetFeature(null);
    }
    else if (featureIds.Count > 1)
    {
      failures.Add(new ValidationFailure(nameof(CasteDefinition.Feature), "'{PropertyName}' must contain at most one element.", featureIds)
      {
        ErrorCode = ErrorCodes.TooManyValues
      });
    }
    else
    {
      Guid featureId = featureIds.Single();
      FeatureEntity? feature = await _rules.Features.SingleOrDefaultAsync(x => x.Id == featureId, cancellationToken);
      if (feature is null)
      {
        failures.Add(new ValidationFailure(nameof(CasteDefinition.Feature), "'{PropertyName}' must reference an existing entity.", featureId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
      else
      {
        caste.SetFeature(feature);
      }
    }
  }

  private async Task SetSkillAsync(CasteEntity caste, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> skillIds = invariant.GetRelatedContent(CasteDefinition.Skill);
    if (skillIds.Count < 1)
    {
      caste.SetSkill(null);
    }
    else if (skillIds.Count > 1)
    {
      failures.Add(new ValidationFailure(nameof(CasteDefinition.Skill), "'{PropertyName}' must contain at most one element.", skillIds)
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
        failures.Add(new ValidationFailure(nameof(CasteDefinition.Skill), "'{PropertyName}' must reference an existing entity.", skillId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
      else
      {
        caste.SetSkill(skill);
      }
    }
  }
}
