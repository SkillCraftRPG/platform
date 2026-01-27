using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishArticleCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishArticleCommandHandler : ICommandHandler<UnpublishArticleCommand, Unit>
{
  private readonly EncyclopediaContext _encyclopedia;
  private readonly ILogger<UnpublishArticleCommandHandler> _logger;

  public UnpublishArticleCommandHandler(EncyclopediaContext encyclopedia, ILogger<UnpublishArticleCommandHandler> logger)
  {
    _encyclopedia = encyclopedia;
    _logger = logger;
  }

  public async Task<Unit> HandleAsync(UnpublishArticleCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    ArticleEntity? article = await _encyclopedia.Articles.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (article is null)
    {
      _logger.LogWarning("The article 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      article.Unpublish(@event);

      await _encyclopedia.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The article '{Article}' has been unpublished.", article);
    }

    return Unit.Value;
  }
}
