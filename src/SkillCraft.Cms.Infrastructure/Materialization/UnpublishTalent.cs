using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishTalentCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishTalentCommandHandler : ICommandHandler<UnpublishTalentCommand, Unit>
{
  private readonly ILogger<UnpublishTalentCommandHandler> _logger;
  private readonly RulesContext _rules;

  public UnpublishTalentCommandHandler(ILogger<UnpublishTalentCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(UnpublishTalentCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    TalentEntity? talent = await _rules.Talents.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (talent is null)
    {
      _logger.LogWarning("The talent 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      talent.Unpublish(@event);

      await _rules.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The talent '{Talent}' has been unpublished.", talent);
    }

    return Unit.Value;
  }
}
