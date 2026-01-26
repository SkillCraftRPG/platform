using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Contents;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record PublishFeatureCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishFeatureCommandHandler : ICommandHandler<PublishFeatureCommand, Unit>
{
  private readonly ILogger<PublishFeatureCommandHandler> _logger;
  private readonly RulesContext _rules;

  public PublishFeatureCommandHandler(ILogger<PublishFeatureCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(PublishFeatureCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    FeatureEntity? feature = await _rules.Features.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (feature is null)
    {
      feature = new FeatureEntity(command.Event);
      _rules.Features.Add(feature);
    }

    feature.Key = locale.UniqueName.Value;
    feature.Name = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    feature.HtmlContent = locale.TryGetString(FeatureDefinition.HtmlContent);

    feature.Publish(@event);

    await _rules.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The feature '{Feature}' has been published.", feature);

    return Unit.Value;
  }
}
