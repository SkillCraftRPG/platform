using FluentValidation;
using FluentValidation.Results;
using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Core.Statistics;
using SkillCraft.Cms.Infrastructure.Contents;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record PublishStatisticCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishStatisticCommandHandler : ICommandHandler<PublishStatisticCommand, Unit>
{
  private readonly ILogger<PublishStatisticCommandHandler> _logger;
  private readonly RulesContext _rules;

  public PublishStatisticCommandHandler(ILogger<PublishStatisticCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(PublishStatisticCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    StatisticEntity? statistic = await _rules.Statistics.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (statistic is null)
    {
      statistic = new StatisticEntity(command.Event);
      _rules.Statistics.Add(statistic);
    }

    List<ValidationFailure> failures = new(capacity: 2);

    statistic.Slug = locale.GetString(StatisticDefinition.Slug);
    statistic.Value = GetValue(invariant, failures);
    statistic.Name = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    await SetAttributeAsync(statistic, invariant, failures, cancellationToken);

    statistic.MetaDescription = locale.TryGetString(StatisticDefinition.MetaDescription);
    statistic.Summary = locale.TryGetString(StatisticDefinition.Summary);
    statistic.HtmlContent = locale.TryGetString(StatisticDefinition.HtmlContent);

    statistic.Publish(@event);

    if (failures.Count > 0)
    {
      _rules.ChangeTracker.Clear();
      throw new ValidationException(failures);
    }

    await _rules.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The statistic '{Statistic}' has been published.", statistic);

    return Unit.Value;
  }

  private static GameStatistic GetValue(ContentLocale invariant, List<ValidationFailure> failures)
  {
    if (Enum.TryParse(invariant.UniqueName.Value, out GameStatistic value) && Enum.IsDefined(value))
    {
      return value;
    }

    failures.Add(new ValidationFailure(nameof(ContentLocale.UniqueName), $"'{{PropertyName}}' must be parseable as a {nameof(GameStatistic)}.", invariant.UniqueName.Value)
    {
      ErrorCode = ErrorCodes.InvalidEnumValue
    });

    return default;
  }

  private async Task SetAttributeAsync(StatisticEntity statistic, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> attributeIds = invariant.GetRelatedContent(StatisticDefinition.Attribute);
    if (attributeIds.Count < 1)
    {
      failures.Add(new ValidationFailure(nameof(StatisticDefinition.Attribute), "'{PropertyName}' must contain exactly one element.", attributeIds)
      {
        ErrorCode = ErrorCodes.EmptyValue
      });
    }
    else if (attributeIds.Count > 1)
    {
      failures.Add(new ValidationFailure(nameof(StatisticDefinition.Attribute), "'{PropertyName}' must contain exactly one element.", attributeIds)
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
        failures.Add(new ValidationFailure(nameof(StatisticDefinition.Attribute), "'{PropertyName}' did not reference an existing entity.", attributeId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
      else
      {
        statistic.SetAttribute(attribute);
      }
    }
  }
}
