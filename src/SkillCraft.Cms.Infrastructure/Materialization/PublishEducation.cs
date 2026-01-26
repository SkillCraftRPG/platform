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

internal record PublishEducationCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishEducationCommandHandler : ICommandHandler<PublishEducationCommand, Unit>
{
  private readonly ILogger<PublishEducationCommandHandler> _logger;
  private readonly RulesContext _rules;

  public PublishEducationCommandHandler(ILogger<PublishEducationCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(PublishEducationCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    EducationEntity? education = await _rules.Educations.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (education is null)
    {
      education = new EducationEntity(command.Event);
      _rules.Educations.Add(education);
    }

    List<ValidationFailure> failures = new(capacity: 2);

    education.Slug = locale.GetString(EducationDefinition.Slug);
    education.Name = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    double? wealthMultiplier = invariant.TryGetNumber(EducationDefinition.WealthMultiplier);
    education.WealthMultiplier = wealthMultiplier.HasValue ? (int)wealthMultiplier.Value : default;

    await SetSkillAsync(education, invariant, failures, cancellationToken);
    await SetFeatureAsync(education, invariant, failures, cancellationToken);

    education.MetaDescription = locale.TryGetString(EducationDefinition.MetaDescription);
    education.Summary = locale.TryGetString(EducationDefinition.Summary);
    education.HtmlContent = locale.TryGetString(EducationDefinition.HtmlContent);

    education.Publish(@event);

    if (failures.Count > 0)
    {
      _rules.ChangeTracker.Clear();
      throw new ValidationException(failures);
    }

    await _rules.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The education '{Education}' has been published.", education);

    return Unit.Value;
  }

  private async Task SetFeatureAsync(EducationEntity education, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> featureIds = invariant.GetRelatedContent(EducationDefinition.Feature);
    if (featureIds.Count < 1)
    {
      education.SetFeature(null);
    }
    else if (featureIds.Count > 1)
    {
      failures.Add(new ValidationFailure(nameof(EducationDefinition.Feature), "'{PropertyName}' must contain at most one element.", featureIds)
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
        failures.Add(new ValidationFailure(nameof(EducationDefinition.Feature), "'{PropertyName}' must reference an existing entity.", featureId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
      else
      {
        education.SetFeature(feature);
      }
    }
  }

  private async Task SetSkillAsync(EducationEntity education, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> skillIds = invariant.GetRelatedContent(EducationDefinition.Skill);
    if (skillIds.Count < 1)
    {
      education.SetSkill(null);
    }
    else if (skillIds.Count > 1)
    {
      failures.Add(new ValidationFailure(nameof(EducationDefinition.Skill), "'{PropertyName}' must contain at most one element.", skillIds)
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
        failures.Add(new ValidationFailure(nameof(EducationDefinition.Skill), "'{PropertyName}' must reference an existing entity.", skillId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
      else
      {
        education.SetSkill(skill);
      }
    }
  }
}
