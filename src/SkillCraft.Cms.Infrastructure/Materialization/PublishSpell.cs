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
    SpellEntity? spell = await _rules.Spells
      .Include(x => x.Categories)
      .SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (spell is null)
    {
      spell = new SpellEntity(command.Event);
      _rules.Spells.Add(spell);
    }

    List<ValidationFailure> failures = [];

    spell.Slug = locale.GetString(SpellDefinition.Slug);
    spell.Name = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    spell.Tier = (int)invariant.GetNumber(SpellDefinition.Tier);
    await SetCategoriesAsync(spell, invariant, failures, cancellationToken);

    spell.MetaDescription = locale.TryGetString(SpellDefinition.MetaDescription);
    spell.Summary = locale.TryGetString(SpellDefinition.Summary);
    spell.HtmlContent = locale.TryGetString(SpellDefinition.HtmlContent);

    spell.Publish(@event);

    if (failures.Count > 0)
    {
      _rules.ChangeTracker.Clear();
      throw new ValidationException(failures);
    }

    await _rules.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The spell '{Spell}' has been published.", spell);

    return Unit.Value;
  }

  private async Task SetCategoriesAsync(SpellEntity spell, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> categoryIds = invariant.GetRelatedContent(SpellDefinition.Categories);
    Dictionary<Guid, SpellCategoryEntity> categories = categoryIds.Count < 1
      ? []
      : await _rules.SpellCategories.Where(x => categoryIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, x => x, cancellationToken);

    foreach (SpellCategoryAssociationEntity category in spell.Categories)
    {
      if (!categories.ContainsKey(category.SpellCategoryUid))
      {
        _rules.SpellCategoryAssociations.Remove(category);
      }
    }

    HashSet<Guid> existingIds = spell.Categories.Select(x => x.SpellCategoryUid).ToHashSet();
    foreach (Guid categoryId in categoryIds)
    {
      if (categories.TryGetValue(categoryId, out SpellCategoryEntity? category))
      {
        if (!existingIds.Contains(categoryId))
        {
          spell.AddCategory(category);
        }
      }
      else
      {
        failures.Add(new ValidationFailure(nameof(SpellDefinition.Categories), "'{PropertyName}' must reference existing entities.", categoryId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
    }
  }
}
