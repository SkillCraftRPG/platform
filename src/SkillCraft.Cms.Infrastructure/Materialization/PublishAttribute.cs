using FluentValidation;
using FluentValidation.Results;
using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Core.Attributes;
using SkillCraft.Cms.Infrastructure.Contents;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record PublishAttributeCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishAttributeCommandHandler : ICommandHandler<PublishAttributeCommand, Unit>
{
  private readonly ILogger<PublishAttributeCommandHandler> _logger;
  private readonly RulesContext _rules;

  public PublishAttributeCommandHandler(ILogger<PublishAttributeCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(PublishAttributeCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    AttributeEntity? attribute = await _rules.Attributes.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (attribute is null)
    {
      attribute = new AttributeEntity(command.Event);
      _rules.Attributes.Add(attribute);
    }

    List<ValidationFailure> failures = new(capacity: 2);

    attribute.Slug = locale.GetString(AttributeDefinition.Slug);
    SetValue(attribute, invariant, failures);
    attribute.Name = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    SetCategory(attribute, invariant, failures);

    attribute.MetaDescription = locale.TryGetString(AttributeDefinition.MetaDescription);
    attribute.Summary = locale.TryGetString(AttributeDefinition.Summary);
    attribute.HtmlContent = locale.TryGetString(AttributeDefinition.HtmlContent);

    attribute.Publish(@event);

    if (failures.Count > 0)
    {
      _rules.ChangeTracker.Clear();
      throw new ValidationException(failures);
    }

    await _rules.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The attribute '{Attribute}' has been published.", attribute);

    return Unit.Value;
  }

  private static void SetCategory(AttributeEntity attribute, ContentLocale invariant, List<ValidationFailure> failures)
  {
    IReadOnlyCollection<string> categories = invariant.GetSelect(AttributeDefinition.Category);
    if (categories.Count < 1)
    {
      attribute.Category = null;
    }
    else if (categories.Count > 1)
    {
      failures.Add(new ValidationFailure(nameof(AttributeDefinition.Category), "'{PropertyName}' must contain at most one element.", categories)
      {
        ErrorCode = ErrorCodes.TooManyValues
      });
    }
    else
    {
      string categoryValue = categories.Single();
      if (Enum.TryParse(categoryValue, out AttributeCategory category) && Enum.IsDefined(category))
      {
        attribute.Category = category;
      }
      else
      {
        failures.Add(new ValidationFailure(nameof(AttributeDefinition.Category), $"'{{PropertyName}}' must be parseable as an {nameof(AttributeCategory)}.", categoryValue)
        {
          ErrorCode = ErrorCodes.InvalidEnumValue
        });
      }
    }
  }

  private static void SetValue(AttributeEntity attribute, ContentLocale invariant, List<ValidationFailure> failures)
  {
    if (Enum.TryParse(invariant.UniqueName.Value, out GameAttribute value) && Enum.IsDefined(value))
    {
      attribute.Value = value;
    }
    else
    {
      failures.Add(new ValidationFailure(nameof(ContentLocale.UniqueName), $"'{{PropertyName}}' must be parseable as a {nameof(GameAttribute)}.", invariant.UniqueName.Value)
      {
        ErrorCode = ErrorCodes.InvalidEnumValue
      });
    }
  }
}
