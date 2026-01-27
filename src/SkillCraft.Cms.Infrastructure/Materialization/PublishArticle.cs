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

internal record PublishArticleCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishArticleCommandHandler : ICommandHandler<PublishArticleCommand, Unit>
{
  private readonly EncyclopediaContext _encyclopedia;
  private readonly ILogger<PublishArticleCommandHandler> _logger;

  public PublishArticleCommandHandler(EncyclopediaContext encyclopedia, ILogger<PublishArticleCommandHandler> logger)
  {
    _encyclopedia = encyclopedia;
    _logger = logger;
  }

  public async Task<Unit> HandleAsync(PublishArticleCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    ArticleEntity? article = await _encyclopedia.Articles.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (article is null)
    {
      article = new ArticleEntity(command.Event);
      _encyclopedia.Articles.Add(article);
    }

    List<ValidationFailure> failures = new(capacity: 2);

    article.Slug = locale.GetString(ArticleDefinition.Slug);
    article.Title = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    await SetCollectionAsync(article, invariant, failures, cancellationToken);
    await SetParentAsync(article, invariant, failures, cancellationToken);

    article.MetaDescription = locale.TryGetString(ArticleDefinition.MetaDescription);
    article.HtmlContent = locale.TryGetString(ArticleDefinition.HtmlContent);

    article.Publish(@event);

    if (failures.Count > 0)
    {
      _encyclopedia.ChangeTracker.Clear();
      throw new ValidationException(failures);
    }

    await _encyclopedia.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The article '{Article}' has been published.", article);

    return Unit.Value;
  }

  private async Task SetCollectionAsync(ArticleEntity article, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> collectionIds = invariant.GetRelatedContent(ArticleDefinition.Collection);
    if (collectionIds.Count == 1)
    {
      Guid collectionId = collectionIds.Single();
      CollectionEntity? collection = await _encyclopedia.Collections.SingleOrDefaultAsync(x => x.Id == collectionId, cancellationToken);
      if (collection is null)
      {
        failures.Add(new ValidationFailure(nameof(ArticleDefinition.Collection), "'{PropertyName}' must reference an existing entity.", collectionId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
      else
      {
        article.SetCollection(collection);
      }
    }
    else
    {
      failures.Add(new ValidationFailure(nameof(ArticleDefinition.Collection), "'{PropertyName}' must contain exactly one element.", collectionIds)
      {
        ErrorCode = collectionIds.Count < 1 ? ErrorCodes.EmptyValue : ErrorCodes.TooManyValues
      });
    }
  }

  private async Task SetParentAsync(ArticleEntity article, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> parentIds = invariant.GetRelatedContent(ArticleDefinition.Parent);
    if (parentIds.Count < 1)
    {
      article.SetParent(null);
    }
    else if (parentIds.Count > 1)
    {
      failures.Add(new ValidationFailure(nameof(ArticleDefinition.Parent), "'{PropertyName}' must contain at most one element.", parentIds)
      {
        ErrorCode = ErrorCodes.TooManyValues
      });
    }
    else
    {
      Guid parentId = parentIds.Single();
      ArticleEntity? parent = await _encyclopedia.Articles.SingleOrDefaultAsync(x => x.Id == parentId, cancellationToken);
      if (parent is null)
      {
        failures.Add(new ValidationFailure(nameof(ArticleDefinition.Parent), "'{PropertyName}' must reference an existing entity.", parentId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
      else
      {
        article.SetParent(parent);
      }
    }
  }
}
