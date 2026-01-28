using Krakenar.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Cms.Core.Educations;
using SkillCraft.Cms.Core.Educations.Models;
using SkillCraft.Cms.Models.Parameters;

namespace SkillCraft.Cms.Controllers;

[ApiController]
[Route("api/educations")]
public class EducationController : ControllerBase
{
  private readonly IEducationService _educationService;

  public EducationController(IEducationService educationService)
  {
    _educationService = educationService;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<EducationModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    EducationModel? education = await _educationService.ReadAsync(id, slug: null, cancellationToken);
    return education is null ? NotFound() : Ok(education);
  }

  [HttpGet("slug:{slug}")]
  public async Task<ActionResult<EducationModel>> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    EducationModel? education = await _educationService.ReadAsync(id: null, slug, cancellationToken);
    return education is null ? NotFound() : Ok(education);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<EducationModel>>> SearchAsync([FromQuery] SearchEducationsParameters parameters, CancellationToken cancellationToken)
  {
    SearchEducationsPayload payload = parameters.ToPayload();
    SearchResults<EducationModel> educations = await _educationService.SearchAsync(payload, cancellationToken);
    return Ok(educations);
  }
}
