using FluentValidation;
using FluentValidation.Results;
using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Contents;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record PublishMarkerCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishMarkerCommandHandler : ICommandHandler<PublishMarkerCommand, Unit>
{
  private readonly EncyclopediaContext _encyclopedia;
  private readonly ILogger<PublishMarkerCommandHandler> _logger;

  public PublishMarkerCommandHandler(EncyclopediaContext encyclopedia, ILogger<PublishMarkerCommandHandler> logger)
  {
    _encyclopedia = encyclopedia;
    _logger = logger;
  }

  public async Task<Unit> HandleAsync(PublishMarkerCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    MarkerEntity? marker = await _encyclopedia.Markers.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (marker is null)
    {
      marker = new MarkerEntity(command.Event);
      _encyclopedia.Markers.Add(marker);
    }

    List<ValidationFailure> failures = new(capacity: 1);

    marker.Key = locale.UniqueName.Value;
    marker.Title = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    await SetMapAsync(marker, invariant, failures, cancellationToken);
    marker.X = (int)invariant.GetNumber(MarkerDefinition.X);
    marker.Y = (int)invariant.GetNumber(MarkerDefinition.Y);

    marker.HtmlContent = locale.TryGetString(MarkerDefinition.HtmlContent);

    marker.Publish(@event);

    if (failures.Count > 0)
    {
      _encyclopedia.ChangeTracker.Clear();
      throw new ValidationException(failures);
    }

    await _encyclopedia.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The marker '{Marker}' has been published.", marker);

    return Unit.Value;
  }

  private async Task SetMapAsync(MarkerEntity marker, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> mapIds = invariant.GetRelatedContent(MarkerDefinition.Map);
    if (mapIds.Count == 1)
    {
      Guid mapId = mapIds.Single();
      MapEntity? map = await _encyclopedia.Maps.SingleOrDefaultAsync(x => x.Id == mapId, cancellationToken);
      if (map is null)
      {
        failures.Add(new ValidationFailure(nameof(MarkerDefinition.Map), "'{PropertyName}' must reference an existing entity.", mapId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
      else
      {
        marker.SetMap(map);
      }
    }
    else
    {
      failures.Add(new ValidationFailure(nameof(MarkerDefinition.Map), "'{PropertyName}' must contain exactly one element.", mapIds)
      {
        ErrorCode = mapIds.Count < 1 ? ErrorCodes.EmptyValue : ErrorCodes.TooManyValues
      });
    }
  }
}
