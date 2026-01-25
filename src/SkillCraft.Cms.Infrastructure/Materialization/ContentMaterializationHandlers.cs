using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Krakenar.Core.Localization;
using Krakenar.Core.Logging;
using Krakenar.EntityFrameworkCore.Relational;
using Logitar.CQRS;
using Logitar.EventSourcing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EventId = Logitar.EventSourcing.EventId;
using Stream = Logitar.EventSourcing.Stream;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal class ContentMaterializationHandlers : IEventHandler<ContentLocalePublished>, IEventHandler<ContentLocaleUnpublished>
{
  public static void Register(IServiceCollection services)
  {
    services.AddTransient<IEventHandler<ContentLocalePublished>, ContentMaterializationHandlers>();
    services.AddTransient<IEventHandler<ContentLocaleUnpublished>, ContentMaterializationHandlers>();

    services.AddTransient<ICommandHandler<PublishAttributeCommand, Unit>, PublishAttributeCommandHandler>();
    services.AddTransient<ICommandHandler<PublishSkillCommand, Unit>, PublishSkillCommandHandler>();
    services.AddTransient<ICommandHandler<PublishStatisticCommand, Unit>, PublishStatisticCommandHandler>();

    services.AddTransient<ICommandHandler<UnpublishAttributeCommand, Unit>, UnpublishAttributeCommandHandler>();
    services.AddTransient<ICommandHandler<UnpublishSkillCommand, Unit>, UnpublishSkillCommandHandler>();
    services.AddTransient<ICommandHandler<UnpublishStatisticCommand, Unit>, UnpublishStatisticCommandHandler>();
  }

  private readonly ICommandBus _commandBus;
  private readonly IEventStore _events;
  private readonly KrakenarContext _krakenar;
  private readonly ILogger<ContentMaterializationHandlers> _logger;
  private readonly ILoggingService _loggingService;

  public ContentMaterializationHandlers(
    ICommandBus commandBus,
    IEventStore events,
    KrakenarContext krakenar,
    ILogger<ContentMaterializationHandlers> logger,
    ILoggingService loggingService)
  {
    _commandBus = commandBus;
    _events = events;
    _krakenar = krakenar;
    _logger = logger;
    _loggingService = loggingService;
  }

  public async Task HandleAsync(ContentLocalePublished @event, CancellationToken cancellationToken)
  {
    try
    {
      ContentId contentId = new(@event.StreamId);
      if (contentId.RealmId.HasValue)
      {
        _logger.LogWarning("Event 'Id={EventId}' is being ignored because it has a realm ID.", @event.Id);
        return;
      }

      LanguageId? defaultLanguageId = await GetDefaultLanguageIdAsync(@event.Id, @event.LanguageId, cancellationToken);
      if (!defaultLanguageId.HasValue)
      {
        return;
      }

      EntityKind? kind = await GetEntityKindAsync(@event.StreamId, @event.Id, cancellationToken);
      if (!kind.HasValue)
      {
        return;
      }

      FetchOptions options = new()
      {
        ToVersion = @event.Version,
        IsDeleted = false
      };
      Stream? stream = await _events.FetchAsync(@event.StreamId, options, cancellationToken);
      if (stream is null)
      {
        _logger.LogWarning("Event 'Id={EventId}' is being ignored because its stream 'Id={StreamId}' was not found.", @event.Id, @event.StreamId);
        return;
      }

      PublishedContentLocales? published = GetPublishedContentLocales(stream.Events, defaultLanguageId.Value, @event.Id);
      if (published is null)
      {
        return;
      }

      switch (kind)
      {
        case EntityKind.Attribute:
          await _commandBus.ExecuteAsync(new PublishAttributeCommand(@event, published.Invariant, published.Locale), cancellationToken);
          break;
        case EntityKind.Skill:
          await _commandBus.ExecuteAsync(new PublishSkillCommand(@event, published.Invariant, published.Locale), cancellationToken);
          break;
        case EntityKind.Statistic:
          await _commandBus.ExecuteAsync(new PublishStatisticCommand(@event, published.Invariant, published.Locale), cancellationToken);
          break;
        default:
          _logger.LogWarning("Event 'Id={EventId}' is being ignored because the entity kind '{Kind}' is not supported.", @event.Id, kind);
          return;
      }

      _logger.LogInformation("Event 'Id={EventId}' handled successfully.", @event.Id);
    }
    catch (Exception exception)
    {
      _loggingService.Report(exception);
      _logger.LogError(exception, "Event 'Id={EventId}' was not handled successfully.", @event.Id);
    }
    finally
    {
      // TODO(fpion): reporting
      // Pour une paire EntityKind, EntityId
      // Pour un événement: ID, Version, ActorId, OccurredOn
      // Status: in-sync, out-of-sync
      // Error
    }
  }

  public async Task HandleAsync(ContentLocaleUnpublished @event, CancellationToken cancellationToken)
  {
    try
    {
      ContentId contentId = new(@event.StreamId);
      if (contentId.RealmId.HasValue)
      {
        _logger.LogWarning("Event 'Id={EventId}' is being ignored because it has a realm ID.", @event.Id);
        return;
      }

      LanguageId? defaultLanguageId = await GetDefaultLanguageIdAsync(@event.Id, @event.LanguageId, cancellationToken);
      if (!defaultLanguageId.HasValue)
      {
        return;
      }

      EntityKind? kind = await GetEntityKindAsync(@event.StreamId, @event.Id, cancellationToken);
      if (!kind.HasValue)
      {
        return;
      }

      switch (kind)
      {
        case EntityKind.Attribute:
          await _commandBus.ExecuteAsync(new UnpublishAttributeCommand(@event), cancellationToken);
          break;
        case EntityKind.Skill:
          await _commandBus.ExecuteAsync(new UnpublishSkillCommand(@event), cancellationToken);
          break;
        case EntityKind.Statistic:
          await _commandBus.ExecuteAsync(new UnpublishStatisticCommand(@event), cancellationToken);
          break;
        default:
          _logger.LogWarning("Event 'Id={EventId}' is being ignored because the entity kind '{Kind}' is not supported.", @event.Id, kind);
          return;
      }

      _logger.LogInformation("Event 'Id={EventId}' handled successfully.", @event.Id);
    }
    catch (Exception exception)
    {
      _loggingService.Report(exception);
      _logger.LogError(exception, "Event 'Id={EventId}' was not handled successfully.", @event.Id);
    }
    finally
    {
      // TODO(fpion): reporting
    }
  }

  private async Task<LanguageId?> GetDefaultLanguageIdAsync(EventId eventId, LanguageId? eventLanguageId, CancellationToken cancellationToken)
  {
    string? defaultLanguageStreamId = await _krakenar.Languages.AsNoTracking()
      .Where(x => x.RealmId == null && x.IsDefault)
      .Select(x => x.StreamId)
      .SingleOrDefaultAsync(cancellationToken);
    if (string.IsNullOrWhiteSpace(defaultLanguageStreamId))
    {
      _logger.LogWarning("Event 'Id={EventId}' is being ignored because the defaut language was not found.", @eventId);
      return null;
    }

    LanguageId defaultLanguageId = new(defaultLanguageStreamId);
    if (eventLanguageId.HasValue && eventLanguageId.Value != defaultLanguageId)
    {
      _logger.LogWarning("Event 'Id={EventId}' is being ignored because the language 'Id={LanguageId}' is not tracked.", eventId, eventLanguageId.Value);
      return null;
    }
    return defaultLanguageId;
  }

  private async Task<EntityKind?> GetEntityKindAsync(StreamId streamId, EventId eventId, CancellationToken cancellationToken)
  {
    string? contentType = await _krakenar.Contents.AsNoTracking()
      .Include(x => x.ContentType)
      .Where(x => x.StreamId == streamId.Value)
      .Select(x => x.ContentType!.UniqueName)
      .SingleOrDefaultAsync(cancellationToken);

    if (string.IsNullOrWhiteSpace(contentType) || !Enum.TryParse(contentType.Trim(), ignoreCase: true, out EntityKind kind) || !Enum.IsDefined(kind))
    {
      _logger.LogWarning("Event 'Id={EventId}' is being ignored because its content type '{ContentType}' is not tracked.", eventId, contentType);
      return null;
    }

    return kind;
  }

  private record PublishedContentLocales(ContentLocale Invariant, ContentLocale Locale);
  private PublishedContentLocales? GetPublishedContentLocales(IEnumerable<Event> events, LanguageId defaultLanguageId, EventId eventId)
  {
    ContentLocale? latestInvariant = null;
    ContentLocale? latestLocale = null;
    ContentLocale? publishedInvariant = null;
    ContentLocale? publishedLocale = null;
    foreach (Event change in events)
    {
      if (change.Data is ContentCreated created)
      {
        latestInvariant = created.Invariant;
      }
      else if (change.Data is ContentLocaleChanged changed)
      {
        if (changed.LanguageId is null)
        {
          latestInvariant = changed.Locale;
        }
        else if (changed.LanguageId == defaultLanguageId)
        {
          latestLocale = changed.Locale;
        }
      }
      else if (change.Data is ContentLocalePublished published)
      {
        if (published.LanguageId is null)
        {
          publishedInvariant = latestInvariant;
        }
        else if (published.LanguageId == defaultLanguageId)
        {
          publishedLocale = latestLocale;
        }
      }
      else if (change.Data is ContentLocaleRemoved removed)
      {
        if (removed.LanguageId == defaultLanguageId)
        {
          latestLocale = null;
          publishedLocale = null;
        }
      }
      else if (change.Data is ContentLocaleUnpublished unpublished)
      {
        if (unpublished.LanguageId is null)
        {
          publishedInvariant = null;
        }
        else if (unpublished.LanguageId == defaultLanguageId)
        {
          publishedLocale = null;
        }
      }
    }

    if (publishedInvariant is null)
    {
      _logger.LogWarning("Event 'Id={EventId}' is being ignored because the invariant is not published.", eventId);
      return null;
    }
    else if (publishedLocale is null)
    {
      _logger.LogWarning("Event 'Id={EventId}' is being ignored because the locale is not published.", eventId);
      return null;
    }

    return new PublishedContentLocales(publishedInvariant, publishedLocale);
  }
}
