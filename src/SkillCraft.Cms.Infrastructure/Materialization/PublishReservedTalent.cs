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

internal record PublishExclusiveTalentCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishExclusiveTalentCommandHandler : ICommandHandler<PublishExclusiveTalentCommand, Unit>
{
  private readonly ILogger<PublishExclusiveTalentCommandHandler> _logger;
  private readonly RulesContext _rules;

  public PublishExclusiveTalentCommandHandler(ILogger<PublishExclusiveTalentCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(PublishExclusiveTalentCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    ExclusiveTalentEntity? exclusiveTalent = await _rules.ExclusivedTalents
      .Include(x => x.DiscountedTalents)
      .Include(x => x.Features)
      .SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (exclusiveTalent is null)
    {
      exclusiveTalent = new ExclusiveTalentEntity(command.Event);
      _rules.ExclusivedTalents.Add(exclusiveTalent);
    }

    List<ValidationFailure> failures = [];

    exclusiveTalent.Key = locale.UniqueName.Value;
    exclusiveTalent.Name = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    await SetSpecializationAsync(exclusiveTalent, invariant, failures, cancellationToken);
    await SetDiscountedTalentsAsync(exclusiveTalent, invariant, failures, cancellationToken);
    await SetFeaturesAsync(exclusiveTalent, invariant, failures, cancellationToken);

    exclusiveTalent.HtmlContent = locale.TryGetString(ExclusiveTalentDefinition.HtmlContent);

    exclusiveTalent.Publish(@event);

    if (failures.Count > 0)
    {
      _rules.ChangeTracker.Clear();
      throw new ValidationException(failures);
    }

    await _rules.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The exclusive talent '{ExclusiveTalent}' has been published.", exclusiveTalent);

    return Unit.Value;
  }

  private async Task SetDiscountedTalentsAsync(ExclusiveTalentEntity exclusiveTalent, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> talentIds = invariant.GetRelatedContent(ExclusiveTalentDefinition.DiscountedTalents);
    Dictionary<Guid, TalentEntity> talents = talentIds.Count < 1
      ? []
      : await _rules.Talents.Where(x => talentIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, x => x, cancellationToken);

    foreach (ExclusiveTalentDiscountedTalentEntity discountedTalent in exclusiveTalent.DiscountedTalents)
    {
      if (!talents.ContainsKey(discountedTalent.TalentUid))
      {
        _rules.ExclusiveTalentDiscountedTalents.Remove(discountedTalent);
      }
    }

    HashSet<Guid> existingIds = exclusiveTalent.DiscountedTalents.Select(x => x.TalentUid).ToHashSet();
    foreach (Guid talentId in talentIds)
    {
      if (talents.TryGetValue(talentId, out TalentEntity? talent))
      {
        if (!existingIds.Contains(talentId))
        {
          exclusiveTalent.AddDiscountedTalent(talent);
        }
      }
      else
      {
        failures.Add(new ValidationFailure(nameof(ExclusiveTalentDefinition.DiscountedTalents), "'{PropertyName}' must reference existing entities.", talentId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
    }
  }

  private async Task SetFeaturesAsync(ExclusiveTalentEntity exclusiveTalent, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> featureIds = invariant.GetRelatedContent(ExclusiveTalentDefinition.Features);
    Dictionary<Guid, FeatureEntity> features = featureIds.Count < 1
      ? []
      : await _rules.Features.Where(x => featureIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, x => x, cancellationToken);

    foreach (ExclusiveTalentFeatureEntity feature in exclusiveTalent.Features)
    {
      if (!features.ContainsKey(feature.FeatureUid))
      {
        _rules.ExclusiveTalentFeatures.Remove(feature);
      }
    }

    HashSet<Guid> existingIds = exclusiveTalent.Features.Select(x => x.FeatureUid).ToHashSet();
    foreach (Guid featureId in featureIds)
    {
      if (features.TryGetValue(featureId, out FeatureEntity? feature))
      {
        if (!existingIds.Contains(featureId))
        {
          exclusiveTalent.AddFeature(feature);
        }
      }
      else
      {
        failures.Add(new ValidationFailure(nameof(ExclusiveTalentDefinition.Features), "'{PropertyName}' must reference existing entities.", featureId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
    }
  }

  private async Task SetSpecializationAsync(ExclusiveTalentEntity exclusiveTalent, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> specializationIds = invariant.GetRelatedContent(ExclusiveTalentDefinition.Specialization);
    if (specializationIds.Count == 1)
    {
      Guid specializationId = specializationIds.Single();
      SpecializationEntity? specialization = await _rules.Specializations.SingleOrDefaultAsync(x => x.Id == specializationId, cancellationToken);
      if (specialization is null)
      {
        failures.Add(new ValidationFailure(nameof(ExclusiveTalentDefinition.Specialization), "'{PropertyName}' must reference an existing entity.", specializationId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
      else
      {
        exclusiveTalent.SetSpecialization(specialization);
      }
    }
    else
    {
      failures.Add(new ValidationFailure(nameof(ExclusiveTalentDefinition.Specialization), "'{PropertyName}' must contain exactly one element.", specializationIds)
      {
        ErrorCode = specializationIds.Count < 1 ? ErrorCodes.EmptyValue : ErrorCodes.TooManyValues
      });
    }
  }
}

// TODO(fpion): Specialization Configurations (5)
// TODO(fpion): Migration
// TODO(fpion): Exclusive Talent → Doctrine?
