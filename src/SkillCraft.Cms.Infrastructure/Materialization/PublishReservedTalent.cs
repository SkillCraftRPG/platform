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

internal record PublishDoctrineTalentCommand(ContentLocalePublished Event, ContentLocale Invariant, ContentLocale Locale) : ICommand;

internal class PublishDoctrineTalentCommandHandler : ICommandHandler<PublishDoctrineTalentCommand, Unit>
{
  private readonly ILogger<PublishDoctrineTalentCommandHandler> _logger;
  private readonly RulesContext _rules;

  public PublishDoctrineTalentCommandHandler(ILogger<PublishDoctrineTalentCommandHandler> logger, RulesContext rules)
  {
    _logger = logger;
    _rules = rules;
  }

  public async Task<Unit> HandleAsync(PublishDoctrineTalentCommand command, CancellationToken cancellationToken)
  {
    ContentLocalePublished @event = command.Event;
    ContentLocale invariant = command.Invariant;
    ContentLocale locale = command.Locale;

    string streamId = @event.StreamId.Value;
    DoctrineEntity? doctrine = await _rules.Doctrines
      .Include(x => x.DiscountedTalents)
      .Include(x => x.Features)
      .SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);
    if (doctrine is null)
    {
      doctrine = new DoctrineEntity(command.Event);
      _rules.Doctrines.Add(doctrine);
    }

    List<ValidationFailure> failures = [];

    doctrine.Key = locale.UniqueName.Value;
    doctrine.Name = locale.DisplayName?.Value ?? locale.UniqueName.Value;

    await SetSpecializationAsync(doctrine, invariant, failures, cancellationToken);
    await SetDiscountedTalentsAsync(doctrine, invariant, failures, cancellationToken);
    await SetFeaturesAsync(doctrine, invariant, failures, cancellationToken);

    doctrine.HtmlContent = locale.TryGetString(DoctrineDefinition.HtmlContent);

    doctrine.Publish(@event);

    if (failures.Count > 0)
    {
      _rules.ChangeTracker.Clear();
      throw new ValidationException(failures);
    }

    await _rules.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("The doctrine '{Doctrine}' has been published.", doctrine);

    return Unit.Value;
  }

  private async Task SetDiscountedTalentsAsync(DoctrineEntity doctrine, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> talentIds = invariant.GetRelatedContent(DoctrineDefinition.DiscountedTalents);
    Dictionary<Guid, TalentEntity> talents = talentIds.Count < 1
      ? []
      : await _rules.Talents.Where(x => talentIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, x => x, cancellationToken);

    foreach (DoctrineDiscountedTalentEntity discountedTalent in doctrine.DiscountedTalents)
    {
      if (!talents.ContainsKey(discountedTalent.TalentUid))
      {
        _rules.DoctrineDiscountedTalents.Remove(discountedTalent);
      }
    }

    HashSet<Guid> existingIds = doctrine.DiscountedTalents.Select(x => x.TalentUid).ToHashSet();
    foreach (Guid talentId in talentIds)
    {
      if (talents.TryGetValue(talentId, out TalentEntity? talent))
      {
        if (!existingIds.Contains(talentId))
        {
          doctrine.AddDiscountedTalent(talent);
        }
      }
      else
      {
        failures.Add(new ValidationFailure(nameof(DoctrineDefinition.DiscountedTalents), "'{PropertyName}' must reference existing entities.", talentId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
    }
  }

  private async Task SetFeaturesAsync(DoctrineEntity doctrine, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> featureIds = invariant.GetRelatedContent(DoctrineDefinition.Features);
    Dictionary<Guid, FeatureEntity> features = featureIds.Count < 1
      ? []
      : await _rules.Features.Where(x => featureIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, x => x, cancellationToken);

    foreach (DoctrineFeatureEntity feature in doctrine.Features)
    {
      if (!features.ContainsKey(feature.FeatureUid))
      {
        _rules.DoctrineFeatures.Remove(feature);
      }
    }

    HashSet<Guid> existingIds = doctrine.Features.Select(x => x.FeatureUid).ToHashSet();
    foreach (Guid featureId in featureIds)
    {
      if (features.TryGetValue(featureId, out FeatureEntity? feature))
      {
        if (!existingIds.Contains(featureId))
        {
          doctrine.AddFeature(feature);
        }
      }
      else
      {
        failures.Add(new ValidationFailure(nameof(DoctrineDefinition.Features), "'{PropertyName}' must reference existing entities.", featureId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
    }
  }

  private async Task SetSpecializationAsync(DoctrineEntity doctrine, ContentLocale invariant, List<ValidationFailure> failures, CancellationToken cancellationToken)
  {
    IReadOnlyCollection<Guid> specializationIds = invariant.GetRelatedContent(DoctrineDefinition.Specialization);
    if (specializationIds.Count == 1)
    {
      Guid specializationId = specializationIds.Single();
      SpecializationEntity? specialization = await _rules.Specializations.SingleOrDefaultAsync(x => x.Id == specializationId, cancellationToken);
      if (specialization is null)
      {
        failures.Add(new ValidationFailure(nameof(DoctrineDefinition.Specialization), "'{PropertyName}' must reference an existing entity.", specializationId)
        {
          ErrorCode = ErrorCodes.EntityNotFound
        });
      }
      else
      {
        doctrine.SetSpecialization(specialization);
      }
    }
    else
    {
      failures.Add(new ValidationFailure(nameof(DoctrineDefinition.Specialization), "'{PropertyName}' must contain exactly one element.", specializationIds)
      {
        ErrorCode = specializationIds.Count < 1 ? ErrorCodes.EmptyValue : ErrorCodes.TooManyValues
      });
    }
  }
}

// TODO(fpion): Migration
