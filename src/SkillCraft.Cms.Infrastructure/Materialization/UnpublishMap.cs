using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishMapCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishMapCommandHandler : ICommandHandler<UnpublishMapCommand, Unit>
{
  private readonly EncyclopediaContext _encyclopedia;
  private readonly ILogger<UnpublishMapCommandHandler> _logger;

  public UnpublishMapCommandHandler(EncyclopediaContext encyclopedia, ILogger<UnpublishMapCommandHandler> logger)
  {
    _encyclopedia = encyclopedia;
    _logger = logger;
  }

  public async Task<Unit> HandleAsync(UnpublishMapCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    MapEntity? map = await _encyclopedia.Maps.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (map is null)
    {
      _logger.LogWarning("The map 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      map.Unpublish(@event);

      await _encyclopedia.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The map '{Map}' has been unpublished.", map);
    }

    return Unit.Value;
  }
}
