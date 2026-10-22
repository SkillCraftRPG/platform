using Logitar.CQRS;
using SkillCraft.Cms.Core.Specializations.Models;

namespace SkillCraft.Cms.Core.Specializations.Queries;

internal record GetSpecializationTreeQuery(Guid Id) : IQuery<SpecializationTree?>;

internal class GetSpecializationTreeQueryHandler : IQueryHandler<GetSpecializationTreeQuery, SpecializationTree?>
{
  private readonly ISpecializationQuerier _specializationQuerier;

  public GetSpecializationTreeQueryHandler(ISpecializationQuerier specializationQuerier)
  {
    _specializationQuerier = specializationQuerier;
  }

  public async Task<SpecializationTree?> HandleAsync(GetSpecializationTreeQuery query, CancellationToken cancellationToken)
  {
    return await _specializationQuerier.GetTreeAsync(query.Id, cancellationToken);
  }
}
