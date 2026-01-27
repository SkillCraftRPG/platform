using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishCollectionCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishCollectionCommandHandler : ICommandHandler<UnpublishCollectionCommand, Unit>
{
  private readonly EncyclopediaContext _encyclopedia;
  private readonly ILogger<UnpublishCollectionCommandHandler> _logger;

  public UnpublishCollectionCommandHandler(EncyclopediaContext encyclopedia, ILogger<UnpublishCollectionCommandHandler> logger)
  {
    _encyclopedia = encyclopedia;
    _logger = logger;
  }

  public async Task<Unit> HandleAsync(UnpublishCollectionCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    CollectionEntity? collection = await _encyclopedia.Collections.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (collection is null)
    {
      _logger.LogWarning("The collection 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      collection.Unpublish(@event);

      await _encyclopedia.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The collection '{Collection}' has been unpublished.", collection);
    }

    return Unit.Value;
  }
}
