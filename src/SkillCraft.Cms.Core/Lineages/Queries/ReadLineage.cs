using Krakenar.Contracts;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Lineages.Models;

namespace SkillCraft.Cms.Core.Lineages.Queries;

internal record ReadLineageQuery(Guid? Id, string? Slug) : IQuery<LineageModel?>;

internal class ReadLineageQueryHandler : IQueryHandler<ReadLineageQuery, LineageModel?>
{
  private readonly ILineageQuerier _lineageQuerier;

  public ReadLineageQueryHandler(ILineageQuerier lineageQuerier)
  {
    _lineageQuerier = lineageQuerier;
  }

  public async Task<LineageModel?> HandleAsync(ReadLineageQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, LineageModel> lineages = new(capacity: 2);

    if (query.Id.HasValue)
    {
      LineageModel? lineage = await _lineageQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (lineage is not null)
      {
        lineages[lineage.Id] = lineage;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      LineageModel? lineage = await _lineageQuerier.ReadAsync(query.Slug, cancellationToken);
      if (lineage is not null)
      {
        lineages[lineage.Id] = lineage;
      }
    }

    if (lineages.Count > 1)
    {
      throw TooManyResultsException<LineageModel>.ExpectedSingle(lineages.Count);
    }

    return lineages.Values.SingleOrDefault();
  }
}
