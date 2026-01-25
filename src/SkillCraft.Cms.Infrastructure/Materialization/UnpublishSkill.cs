using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishSkillCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishSkillCommandHandler : ICommandHandler<UnpublishSkillCommand, Unit>
{
  private readonly ILogger<UnpublishSkillCommandHandler> _logger;
  private readonly RulesContext _rules;

  public UnpublishSkillCommandHandler(ILogger<UnpublishSkillCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(UnpublishSkillCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    SkillEntity? skill = await _rules.Skills.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (skill is null)
    {
      _logger.LogWarning("The skill 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      skill.Unpublish(@event);

      await _rules.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The skill '{Skill}' has been unpublished.", skill);
    }

    return Unit.Value;
  }
}
