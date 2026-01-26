using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Infrastructure.Contents;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record PublishSpellCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishSpellCommandHandler : ICommandHandler<PublishSpellCommand, Unit>
{
  private readonly ILogger<PublishSpellCommandHandler> _logger;
  private readonly RulesContext _rules;

  public PublishSpellCommandHandler(ILogger<PublishSpellCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(PublishSpellCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    SpellEntity? spell = await _rules.Spells.SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (spell is null)
    {
      spell = new SpellEntity(command.Event);
      _rules.Spells.Add(spell);
    }

    spell.Slug = locale.GetString(SpellDefinition.Slug);
    spell.Name = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    spell.Tier = (int)invariant.GetNumber(SpellDefinition.Tier);

    spell.MetaDescription = locale.TryGetString(SpellDefinition.MetaDescription);
    spell.Summary = locale.TryGetString(SpellDefinition.Summary);
    spell.HtmlContent = locale.TryGetString(SpellDefinition.HtmlContent);

    spell.Publish(@event);

    await _rules.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The spell '{Spell}' has been published.", spell);

    return Unit.Value;
  }
}
