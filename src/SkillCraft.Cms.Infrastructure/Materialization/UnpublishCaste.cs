using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishCasteCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishCasteCommandHandler : ICommandHandler<UnpublishCasteCommand, Unit>
{
  private readonly ILogger<UnpublishCasteCommandHandler> _logger;
  private readonly RulesContext _rules;

  public UnpublishCasteCommandHandler(ILogger<UnpublishCasteCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(UnpublishCasteCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    CasteEntity? caste = await _rules.Castes.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (caste is null)
    {
      _logger.LogWarning("The caste 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      caste.Unpublish(@event);

      await _rules.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The caste '{Caste}' has been unpublished.", caste);
    }

    return Unit.Value;
  }
}
