using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishLanguageCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishLanguageCommandHandler : ICommandHandler<UnpublishLanguageCommand, Unit>
{
  private readonly ILogger<UnpublishLanguageCommandHandler> _logger;
  private readonly RulesContext _rules;

  public UnpublishLanguageCommandHandler(ILogger<UnpublishLanguageCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(UnpublishLanguageCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    LanguageEntity? language = await _rules.Languages.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (language is null)
    {
      _logger.LogWarning("The language 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      language.Unpublish(@event);

      await _rules.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The language '{Language}' has been unpublished.", language);
    }

    return Unit.Value;
  }
}
