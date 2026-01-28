using Krakenar.Contracts;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Specializations.Models;

namespace SkillCraft.Cms.Core.Specializations.Queries;

internal record ReadSpecializationQuery(Guid? Id, string? Slug) : IQuery<SpecializationModel?>;

internal class ReadSpecializationQueryHandler : IQueryHandler<ReadSpecializationQuery, SpecializationModel?>
{
  private readonly ISpecializationQuerier _specializationQuerier;

  public ReadSpecializationQueryHandler(ISpecializationQuerier specializationQuerier)
  {
    _specializationQuerier = specializationQuerier;
  }

  public async Task<SpecializationModel?> HandleAsync(ReadSpecializationQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, SpecializationModel> specializations = new(capacity: 2);

    if (query.Id.HasValue)
    {
      SpecializationModel? specialization = await _specializationQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (specialization is not null)
      {
        specializations[specialization.Id] = specialization;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      SpecializationModel? specialization = await _specializationQuerier.ReadAsync(query.Slug, cancellationToken);
      if (specialization is not null)
      {
        specializations[specialization.Id] = specialization;
      }
    }

    if (specializations.Count > 1)
    {
      throw TooManyResultsException<SpecializationModel>.ExpectedSingle(specializations.Count);
    }

    return specializations.Values.SingleOrDefault();
  }
}
