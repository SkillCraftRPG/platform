using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishLineageCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishLineageCommandHandler : ICommandHandler<UnpublishLineageCommand, Unit>
{
  private readonly ILogger<UnpublishLineageCommandHandler> _logger;
  private readonly RulesContext _rules;

  public UnpublishLineageCommandHandler(ILogger<UnpublishLineageCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(UnpublishLineageCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    LineageEntity? lineage = await _rules.Lineages.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (lineage is null)
    {
      _logger.LogWarning("The lineage 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      lineage.Unpublish(@event);

      await _rules.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The lineage '{Lineage}' has been unpublished.", lineage);
    }

    return Unit.Value;
  }
}
