using Krakenar.Contracts.Contents;
using Krakenar.Contracts.Fields;
using Krakenar.Contracts.Search;
using Krakenar.Core.Contents;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Progress.Models;

namespace SkillCraft.Cms.Core.Progress.Queries;

internal record ReadProgressQuery : IQuery<ProgressModel>;

internal class ReadProgressQueryHandler : IQueryHandler<ReadProgressQuery, ProgressModel>
{
  private static readonly Dictionary<Guid, Action<ProgressModel, FieldValue>> _handlers = new()
  {
    [ProgressDefinition.Characters] = (progress, field) => progress.Characters = Parse(field.Value),
    [ProgressDefinition.Attributes] = (progress, field) => progress.Attributes = Parse(field.Value),
    [ProgressDefinition.Statistics] = (progress, field) => progress.Statistics = Parse(field.Value),
    [ProgressDefinition.Skills] = (progress, field) => progress.Skills = Parse(field.Value),
    [ProgressDefinition.Lineages] = (progress, field) => progress.Lineages = Parse(field.Value),
    [ProgressDefinition.Customizations] = (progress, field) => progress.Customizations = Parse(field.Value),
    [ProgressDefinition.Castes] = (progress, field) => progress.Castes = Parse(field.Value),
    [ProgressDefinition.Educations] = (progress, field) => progress.Educations = Parse(field.Value),
    [ProgressDefinition.Talents] = (progress, field) => progress.Talents = Parse(field.Value),
    [ProgressDefinition.Specializations] = (progress, field) => progress.Specializations = Parse(field.Value),
    [ProgressDefinition.Languages] = (progress, field) => progress.Languages = Parse(field.Value),
    [ProgressDefinition.Equipment] = (progress, field) => progress.Equipment = Parse(field.Value),
    [ProgressDefinition.Adventure] = (progress, field) => progress.Adventure = Parse(field.Value),
    [ProgressDefinition.Combat] = (progress, field) => progress.Combat = Parse(field.Value),
    [ProgressDefinition.Magic] = (progress, field) => progress.Magic = Parse(field.Value),
    [ProgressDefinition.Annexes] = (progress, field) => progress.Annexes = Parse(field.Value)
  };

  private readonly IPublishedContentQuerier _publishedContentQuerier;

  public ReadProgressQueryHandler(IPublishedContentQuerier publishedContentQuerier)
  {
    _publishedContentQuerier = publishedContentQuerier;
  }

  public async Task<ProgressModel> HandleAsync(ReadProgressQuery query, CancellationToken cancellationToken)
  {
    SearchPublishedContentsPayload payload = new();
    payload.ContentType.Uids.Add(ProgressDefinition.ContentTypeId);
    SearchResults<PublishedContentLocale> locales = await _publishedContentQuerier.SearchAsync(payload, cancellationToken);
    PublishedContentLocale locale = locales.Items.Single();

    ProgressModel progress = new();
    foreach (FieldValue fieldValue in locale.FieldValues)
    {
      if (_handlers.TryGetValue(fieldValue.Id, out Action<ProgressModel, FieldValue>? handler))
      {
        handler(progress, fieldValue);
      }
    }
    return progress;
  }

  private static double Parse(string value)
  {
    value = value.Split('=').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).Last();
    if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double progress) && progress > 0)
    {
      return progress > 1 ? 1 : progress;
    }
    return 0;
  }
}
