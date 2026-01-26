using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishEducationCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishEducationCommandHandler : ICommandHandler<UnpublishEducationCommand, Unit>
{
  private readonly ILogger<UnpublishEducationCommandHandler> _logger;
  private readonly RulesContext _rules;

  public UnpublishEducationCommandHandler(ILogger<UnpublishEducationCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(UnpublishEducationCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    EducationEntity? education = await _rules.Educations.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (education is null)
    {
      _logger.LogWarning("The education 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      education.Unpublish(@event);

      await _rules.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The education '{Education}' has been unpublished.", education);
    }

    return Unit.Value;
  }
}
