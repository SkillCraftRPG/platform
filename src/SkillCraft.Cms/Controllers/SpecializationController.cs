using Krakenar.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Cms.Core.Specializations;
using SkillCraft.Cms.Core.Specializations.Models;
using SkillCraft.Cms.Models.Parameters;

namespace SkillCraft.Cms.Controllers;

[ApiController]
[Route("api/specializations")]
public class SpecializationController : ControllerBase
{
  private readonly ISpecializationService _specializationService;

  public SpecializationController(ISpecializationService specializationService)
  {
    _specializationService = specializationService;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<SpecializationModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    SpecializationModel? specialization = await _specializationService.ReadAsync(id, slug: null, cancellationToken);
    return specialization is null ? NotFound() : Ok(specialization);
  }

  [HttpGet("slug:{slug}")]
  public async Task<ActionResult<SpecializationModel>> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    SpecializationModel? specialization = await _specializationService.ReadAsync(id: null, slug, cancellationToken);
    return specialization is null ? NotFound() : Ok(specialization);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<SpecializationModel>>> SearchAsync([FromQuery] SearchSpecializationsParameters parameters, CancellationToken cancellationToken)
  {
    SearchSpecializationsPayload payload = parameters.ToPayload();
    SearchResults<SpecializationModel> specializations = await _specializationService.SearchAsync(payload, cancellationToken);
    return Ok(specializations);
  }
}
