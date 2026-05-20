using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishMarkerCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishMarkerCommandHandler : ICommandHandler<UnpublishMarkerCommand, Unit>
{
  private readonly EncyclopediaContext _encyclopedia;
  private readonly ILogger<UnpublishMarkerCommandHandler> _logger;

  public UnpublishMarkerCommandHandler(EncyclopediaContext encyclopedia, ILogger<UnpublishMarkerCommandHandler> logger)
  {
    _encyclopedia = encyclopedia;
    _logger = logger;
  }

  public async Task<Unit> HandleAsync(UnpublishMarkerCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    MarkerEntity? marker = await _encyclopedia.Markers.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (marker is null)
    {
      _logger.LogWarning("The marker 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      marker.Unpublish(@event);

      await _encyclopedia.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The marker '{Marker}' has been unpublished.", marker);
    }

    return Unit.Value;
  }
}
