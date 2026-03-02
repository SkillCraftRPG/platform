using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record UnpublishSpellCategoryCommand(ContentLocaleUnpublished Event) : ICommand;

internal class UnpublishSpellCategoryCommandHandler : ICommandHandler<UnpublishSpellCategoryCommand, Unit>
{
  private readonly ILogger<UnpublishSpellCategoryCommandHandler> _logger;
  private readonly RulesContext _rules;

  public UnpublishSpellCategoryCommandHandler(ILogger<UnpublishSpellCategoryCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(UnpublishSpellCategoryCommand command, CancellationToken cancellationToken)
  {
    ContentLocaleUnpublished @event = command.Event;
    string streamId = @event.StreamId.Value;
    SpellCategoryEntity? spellCategory = await _rules.SpellCategories.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (spellCategory is null)
    {
      _logger.LogWarning("The spell category 'StreamId={StreamId}' was not found.", streamId);
    }
    else
    {
      spellCategory.Unpublish(@event);

      await _rules.SaveChangesAsync(cancellationToken);
      _logger.LogInformation("The spell category '{SpellCategory}' has been unpublished.", spellCategory);
    }

    return Unit.Value;
  }
}
