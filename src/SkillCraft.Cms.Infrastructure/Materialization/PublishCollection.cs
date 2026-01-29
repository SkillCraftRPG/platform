using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record PublishCollectionCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishCollectionCommandHandler : ICommandHandler<PublishCollectionCommand, Unit>
{
  private readonly EncyclopediaContext _encyclopedia;
  private readonly ILogger<PublishCollectionCommandHandler> _logger;

  public PublishCollectionCommandHandler(EncyclopediaContext encyclopedia, ILogger<PublishCollectionCommandHandler> logger)
  {
    _encyclopedia = encyclopedia;
    _logger = logger;
  }

  public async Task<Unit> HandleAsync(PublishCollectionCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    CollectionEntity? collection = await _encyclopedia.Collections.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (collection is null)
    {
      collection = new CollectionEntity(command.Event);
      _encyclopedia.Collections.Add(collection);
    }

    collection.Slug = locale.UniqueName.Value; // TODO(fpion): locale.GetString(CollectionDefinition.Slug);
    collection.Name = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    // TODO(fpion): collection.MetaDescription = locale.TryGetString(CollectionDefinition.MetaDescription);
    // TODO(fpion): collection.HtmlContent = locale.TryGetString(CollectionDefinition.HtmlContent);

    collection.Publish(@event);

    await _encyclopedia.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The collection '{Collection}' has been published.", collection);

    return Unit.Value;
  }
}
