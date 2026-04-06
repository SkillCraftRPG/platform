using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishSpeciesCategoryCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishSpeciesCategoryCommandHandler : ICommandHandler<UnpublishSpeciesCategoryCommand, Unit>
{
  private readonly ILogger<UnpublishSpeciesCategoryCommandHandler> _logger;
  private readonly RulesContext _rules;

  public UnpublishSpeciesCategoryCommandHandler(ILogger<UnpublishSpeciesCategoryCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(UnpublishSpeciesCategoryCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    SpeciesCategoryEntity? speciesCategory = await _rules.SpeciesCategories.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (speciesCategory is null)
    {
      _logger.LogWarning("The species category 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      speciesCategory.Unpublish(@event);

      await _rules.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The species category '{SpeciesCategory}' has been unpublished.", speciesCategory);
    }

    return Unit.Value;
  }
}
