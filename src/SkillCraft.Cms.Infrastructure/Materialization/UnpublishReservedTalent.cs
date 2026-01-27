using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishDoctrineCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishDoctrineCommandHandler : ICommandHandler<UnpublishDoctrineCommand, Unit>
{
  private readonly ILogger<UnpublishDoctrineCommandHandler> _logger;
  private readonly RulesContext _rules;

  public UnpublishDoctrineCommandHandler(ILogger<UnpublishDoctrineCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(UnpublishDoctrineCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    DoctrineEntity? doctrine = await _rules.Doctrines.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (doctrine is null)
    {
      _logger.LogWarning("The doctrine 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      doctrine.Unpublish(@event);

      await _rules.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The doctrine '{Doctrine}' has been unpublished.", doctrine);
    }

    return Unit.Value;
  }
}
