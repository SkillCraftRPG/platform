using Krakenar.Contracts;
using Logitar.CQRS;
using SkillCraft.Cms.Core.Educations.Models;

namespace SkillCraft.Cms.Core.Educations.Queries;

internal record ReadEducationQuery(Guid? Id, string? Slug) : IQuery<EducationModel?>;

internal class ReadEducationQueryHandler : IQueryHandler<ReadEducationQuery, EducationModel?>
{
  private readonly IEducationQuerier _educationQuerier;

  public ReadEducationQueryHandler(IEducationQuerier educationQuerier)
  {
    _educationQuerier = educationQuerier;
  }

  public async Task<EducationModel?> HandleAsync(ReadEducationQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, EducationModel> educations = new(capacity: 2);

    if (query.Id.HasValue)
    {
      EducationModel? education = await _educationQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (education is not null)
      {
        educations[education.Id] = education;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      EducationModel? education = await _educationQuerier.ReadAsync(query.Slug, cancellationToken);
      if (education is not null)
      {
        educations[education.Id] = education;
      }
    }

    if (educations.Count > 1)
    {
      throw TooManyResultsException<EducationModel>.ExpectedSingle(educations.Count);
    }

    return educations.Values.SingleOrDefault();
  }
}
