using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Contents;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record PublishMapCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishMapCommandHandler : ICommandHandler<PublishMapCommand, Unit>
{
  private readonly EncyclopediaContext _encyclopedia;
  private readonly ILogger<PublishMapCommandHandler> _logger;

  public PublishMapCommandHandler(EncyclopediaContext encyclopedia, ILogger<PublishMapCommandHandler> logger)
  {
    _encyclopedia = encyclopedia;
    _logger = logger;
  }

  public async Task<Unit> HandleAsync(PublishMapCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    MapEntity? map = await _encyclopedia.Maps.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (map is null)
    {
      map = new MapEntity(command.Event);
      _encyclopedia.Maps.Add(map);
    }

    map.Key = locale.UniqueName.Value;
    map.Title = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    map.Width = (int)invariant.GetNumber(MapDefinition.Width);
    map.Height = (int)invariant.GetNumber(MapDefinition.Height);
    map.Source = invariant.GetString(MapDefinition.Source);

    map.Publish(@event);

    await _encyclopedia.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The map '{Map}' has been published.", map);

    return Unit.Value;
  }
}
