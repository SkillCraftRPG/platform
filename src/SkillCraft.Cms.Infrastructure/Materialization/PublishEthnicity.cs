using FluentValidation;
using FluentValidation.Results;
using Krakenar.Core.Contents;
using Krakenar.Core.Contents.Events;
using Logitar;
using Logitar.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillCraft.Cms.Core.Lineages;
using SkillCraft.Cms.Infrastructure.Configurations;
using SkillCraft.Cms.Infrastructure.Contents;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Materialization;

internal record PublishEthnicityCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishEthnicityCommandHandler : ICommandHandler<PublishEthnicityCommand, Unit>
{
  private readonly ILogger<PublishEthnicityCommandHandler> _logger;
  private readonly RulesContext _rules;

  public PublishEthnicityCommandHandler(ILogger<PublishEthnicityCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(PublishEthnicityCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    LineageEntity? lineage = await _rules.Lineages
      .Include(x => x.Features)
      .Include(x => x.Languages)
      .SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (lineage is null)
    {
      lineage = new LineageEntity(command.Event);
      _rules.Lineages.Add(lineage);
    }

    List<ValidationFailure> failures = [];

    lineage.Slug = locale.GetString(EthnicityDefinition.Slug);
    lineage.Name = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    await SetSpeciesAsync(lineage, invariant, failures, cancellationToken);

    await SetFeaturesAsync(lineage, invariant, failures, cancellationToken);

    await SetLanguagesAsync(lineage, invariant, failures, cancellationToken);
    lineage.ExtraLanguages = (int)invariant.GetNumber(EthnicityDefinition.ExtraLanguages);
    lineage.LanguagesText = locale.TryGetString(EthnicityDefinition.LanguagesText);

    SetNamesSelection(lineage, locale, failures);
    lineage.NamesText = locale.TryGetString(EthnicityDefinition.NamesText);

    SetSpeed(lineage, invariant, failures);

    lineage.MetaDescription = locale.TryGetString(EthnicityDefinition.MetaDescription);
    lineage.Summary = locale.TryGetString(EthnicityDefinition.Summary);
    lineage.HtmlContent = locale.TryGetString(EthnicityDefinition.HtmlContent);

    lineage.Morphology = locale.TryGetString(EthnicityDefinition.Morphology);
    lineage.Psychology = locale.TryGetString(EthnicityDefinition.Psychology);
    lineage.Culture = locale.TryGetString(EthnicityDefinition.Culture);
    lineage.History = locale.TryGetString(EthnicityDefinition.History);
    lineage.Geography = locale.TryGetString(EthnicityDefinition.Geography);
    lineage.Politics = locale.TryGetString(EthnicityDefinition.Politics);
    lineage.Relations = locale.TryGetString(EthnicityDefinition.Relations);

    lineage.Publish(@event);

    if (failures.Count > 0)
    {
      _rules.ChangeTracker.Clear();
      throw new ValidationException(failures);
    }

    await _rules.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The lineage '{Lineage}' has been published.", lineage);

    return Unit.Value;
  }

  private async Task SetFeaturesAsync(LineageEntity lineage, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> featureIds = invariant.GetRelatedContent(EthnicityDefinition.Features);
    Dictionary<Guid, FeatureEntity> features = featureIds.Count < 1
      ? []
      : await _rules.Features.Where(x => featureIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, x => x, cancellationToken);

    foreach (LineageFeatureEntity feature in lineage.Features)
    {
      if (!features.ContainsKey(feature.FeatureUid))
      {
        _rules.LineageFeatures.Remove(feature);
      }
    }

    HashSet<Guid> existingIds = lineage.Features.Select(x => x.FeatureUid).ToHashSet();
    foreach (Guid featureId in featureIds)
    {
      if (features.TryGetValue(featureId, out FeatureEntity? feature))
      {
        if (!existingIds.Contains(featureId))
        {
          lineage.AddFeature(feature);
        }
      }
      else
      {
        failures.Add(new ValidationFailure(nameof(EthnicityDefinition.Features), "'{PropertyName}' must reference existing entities.", featureId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
    }
  }

  private async Task SetLanguagesAsync(LineageEntity lineage, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> languageIds = invariant.GetRelatedContent(EthnicityDefinition.Languages);
    Dictionary<Guid, LanguageEntity> languages = languageIds.Count < 1
      ? []
      : await _rules.Languages.Where(x => languageIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, x => x, cancellationToken);

    foreach (LineageLanguageEntity language in lineage.Languages)
    {
      if (!languages.ContainsKey(language.LanguageUid))
      {
        _rules.LineageLanguages.Remove(language);
      }
    }

    HashSet<Guid> existingIds = lineage.Languages.Select(x => x.LanguageUid).ToHashSet();
    foreach (Guid languageId in languageIds)
    {
      if (languages.TryGetValue(languageId, out LanguageEntity? language))
      {
        if (!existingIds.Contains(languageId))
        {
          lineage.AddLanguage(language);
        }
      }
      else
      {
        failures.Add(new ValidationFailure(nameof(EthnicityDefinition.Languages), "'{PropertyName}' must reference existing entities.", languageId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
    }
  }

  private static void SetNamesSelection(LineageEntity lineage, ContentLocale locale, List<ValidationFailure> failures)
  {
    lineage.FamilyNames = null;
    lineage.FemaleNames = null;
    lineage.MaleNames = null;
    lineage.UnisexNames = null;
    lineage.CustomNames = null;

    string? namesSelection = locale.TryGetString(EthnicityDefinition.NamesSelection);
    if (!string.IsNullOrWhiteSpace(namesSelection))
    {
      Dictionary<string, string[]> custom = [];
      string[] lines = namesSelection.Remove("\r").Split('\n');
      foreach (string line in lines)
      {
        if (!string.IsNullOrWhiteSpace(line))
        {
          string[] parts = line.Split(':');
          if (parts.Length != 2)
          {
            failures.Add(new ValidationFailure(nameof(EthnicityDefinition.NamesSelection), "'{PropertyName}' must contain a list of key-value pairs (colon ':' separator) on each line.", line)
            {
              ErrorCode = ErrorCodes.InvalidFormat
            });
          }
          else
          {
            string category = parts.First().Trim();
            string[] names = parts.Last().Split(Constants.Separator).Where(name => !string.IsNullOrWhiteSpace(name)).Select(name => name.Trim()).OrderBy(name => name).ToArray();
            if (string.IsNullOrEmpty(category))
            {
              failures.Add(new ValidationFailure(nameof(EthnicityDefinition.NamesSelection), "'{PropertyName}' cannot have an empty name category key.", line)
              {
                ErrorCode = ErrorCodes.EmptyValue
              });
            }
            else if (names.Length < 1)
            {
              failures.Add(new ValidationFailure(nameof(EthnicityDefinition.NamesSelection), "'{PropertyName}' cannot have an empty name category list.", line)
              {
                ErrorCode = ErrorCodes.EmptyValue
              });
            }
            else
            {
              switch (category.ToLowerInvariant())
              {
                case "family":
                  lineage.FamilyNames = string.Join(Constants.Separator, names);
                  break;
                case "female":
                  lineage.FemaleNames = string.Join(Constants.Separator, names);
                  break;
                case "male":
                  lineage.MaleNames = string.Join(Constants.Separator, names);
                  break;
                case "unisex":
                  lineage.UnisexNames = string.Join(Constants.Separator, names);
                  break;
                default:
                  custom[category] = names;
                  break;
              }
            }
          }
        }
      }
      lineage.CustomNames = custom.Count < 1 ? null : JsonSerializer.Serialize(custom);
    }
  }

  private async Task SetSpeciesAsync(LineageEntity lineage, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> speciesIds = invariant.GetRelatedContent(EthnicityDefinition.Species);
    if (speciesIds.Count < 1)
    {
      lineage.SetParent(null); // TODO(fpion): it should not be empty!
    }
    else if (speciesIds.Count > 1)
    {
      failures.Add(new ValidationFailure(nameof(EthnicityDefinition.Species), "'{PropertyName}' must contain at most one element.", speciesIds)
      {
        ErrorCode = ErrorCodes.TooManyValues
      });
    }
    else
    {
      Guid parentId = speciesIds.Single();
      LineageEntity? parent = await _rules.Lineages.SingleOrDefaultAsync(x => x.Id == parentId, cancellationToken);
      if (parent is null)
      {
        failures.Add(new ValidationFailure(nameof(EthnicityDefinition.Species), "'{PropertyName}' must reference an existing entity.", parentId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
      else
      {
        lineage.SetParent(parent);
      }
    }
  }

  private void SetSpeed(LineageEntity lineage, ContentLocale invariant, List<ValidationFailure> failures)
  {
    lineage.Walk = 0;
    lineage.Climb = 0;
    lineage.Swim = 0;
    lineage.Fly = 0;
    lineage.Hover = false;
    lineage.Burrow = 0;

    string value = invariant.GetString(EthnicityDefinition.Speeds);
    if (!string.IsNullOrWhiteSpace(value))
    {
      string[] values = value.Trim().Split(Constants.Separator);
      foreach (string raw in values)
      {
        if (!string.IsNullOrWhiteSpace(raw))
        {
          string[] parts = raw.Split(':');
          bool isHover = parts.First().Equals(nameof(LineageEntity.Hover), StringComparison.InvariantCultureIgnoreCase);
          if (parts.Length != 2)
          {
            failures.Add(new ValidationFailure(nameof(EthnicityDefinition.Speeds), "'{PropertyName}' must contain a list of key-value pairs (colon ':' separator) separated by a comma (',').", raw)
            {
              ErrorCode = ErrorCodes.InvalidFormat
            });
          }
          else if (!Enum.TryParse(parts.First(), ignoreCase: true, out SpeedKind kind) && !isHover)
          {
            failures.Add(new ValidationFailure(nameof(EthnicityDefinition.Speeds), $"'{{PropertyName}}' pair keys must be parseable as a {nameof(SpeedKind)}.", raw)
            {
              ErrorCode = ErrorCodes.InvalidFormat
            });
          }
          else if (!int.TryParse(parts.Last(), out int speed) || speed <= 0)
          {
            failures.Add(new ValidationFailure(nameof(EthnicityDefinition.Speeds), "'{PropertyName}' pair values must be a strictly positive integer.", raw)
            {
              ErrorCode = ErrorCodes.InvalidFormat
            });
          }
          else
          {
            if (isHover)
            {
              lineage.Fly = speed;
              lineage.Hover = true;
            }
            else
            {
              switch (kind)
              {
                case SpeedKind.Burrow:
                  lineage.Burrow = speed;
                  break;
                case SpeedKind.Climb:
                  lineage.Climb = speed;
                  break;
                case SpeedKind.Fly:
                  lineage.Fly = speed;
                  break;
                case SpeedKind.Swim:
                  lineage.Swim = speed;
                  break;
                case SpeedKind.Walk:
                  lineage.Walk = speed;
                  break;
                default:
                  _logger.LogWarning("The speed kind '{Value}' is not supported for ethnicity '{Ethnicity}'.", kind, lineage);
                  break;
              }
            }
          }
        }
      }
    }
  }
}
