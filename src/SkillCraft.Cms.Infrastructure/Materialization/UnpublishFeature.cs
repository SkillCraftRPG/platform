using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishFeatureCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishFeatureCommandHandler : ICommandHandler<UnpublishFeatureCommand, Unit>
{
  private readonly ILogger<UnpublishFeatureCommandHandler> _logger;
  private readonly RulesContext _rules;

  public UnpublishFeatureCommandHandler(ILogger<UnpublishFeatureCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(UnpublishFeatureCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    FeatureEntity? feature = await _rules.Features.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (feature is null)
    {
      _logger.LogWarning("The feature 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      feature.Unpublish(@event);

      await _rules.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The feature '{Feature}' has been unpublished.", feature);
    }

    return Unit.Value;
  }
}
