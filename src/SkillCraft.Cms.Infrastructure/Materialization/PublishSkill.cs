using FluentValidation;
using FluentValidation.Results;
using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Core.Skills;
using SkillCraft.Cms.Infrastructure.Contents;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record PublishSkillCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishSkillCommandHandler : ICommandHandler<PublishSkillCommand, Unit>
{
  private readonly ILogger<PublishSkillCommandHandler> _logger;
  private readonly RulesContext _rules;

  public PublishSkillCommandHandler(ILogger<PublishSkillCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(PublishSkillCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    SkillEntity? skill = await _rules.Skills.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (skill is null)
    {
      skill = new SkillEntity(command.Event);
      _rules.Skills.Add(skill);
    }

    List<ValidationFailure> failures = new(capacity: 2);

    skill.Slug = locale.GetString(SkillDefinition.Slug);
    SetValue(skill, invariant, failures);
    skill.Name = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    await SetAttributeAsync(skill, invariant, failures, cancellationToken);

    skill.MetaDescription = locale.TryGetString(SkillDefinition.MetaDescription);
    skill.Summary = locale.TryGetString(SkillDefinition.Summary);
    skill.HtmlContent = locale.TryGetString(SkillDefinition.HtmlContent);

    skill.Publish(@event);

    if (failures.Count > 0)
    {
      _rules.ChangeTracker.Clear();
      throw new ValidationException(failures);
    }

    await _rules.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The skill '{Skill}' has been published.", skill);

    return Unit.Value;
  }

  private static void SetValue(SkillEntity skill, ContentLocale invariant, List<ValidationFailure> failures)
  {
    if (Enum.TryParse(invariant.UniqueName.Value, out GameSkill value) && Enum.IsDefined(value))
    {
      skill.Value = value;
    }
    else
    {
      failures.Add(new ValidationFailure(nameof(ContentLocale.UniqueName), $"'{{PropertyName}}' must be parseable as a {nameof(GameSkill)}.", invariant.UniqueName.Value)
      {
        ErrorCode = ErrorCodes.InvalidEnumValue
      });
    }
  }

  private async Task SetAttributeAsync(SkillEntity skill, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> attributeIds = invariant.GetRelatedContent(SkillDefinition.Attribute);
    if (attributeIds.Count < 1)
    {
      skill.SetAttribute(null);
    }
    else if (attributeIds.Count > 1)
    {
      failures.Add(new ValidationFailure(nameof(SkillDefinition.Attribute), "'{PropertyName}' must contain at most one element.", attributeIds)
      {
        ErrorCode = ErrorCodes.TooManyValues
      });
    }
    else
    {
      Guid attributeId = attributeIds.Single();
      AttributeEntity? attribute = await _rules.Attributes.SingleOrDefaultAsync(x => x.Id == attributeId, cancellationToken);
      if (attribute is null)
      {
        failures.Add(new ValidationFailure(nameof(SkillDefinition.Attribute), "'{PropertyName}' did not reference an existing entity.", attributeId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
      else
      {
        skill.SetAttribute(attribute);
      }
    }
  }
}
