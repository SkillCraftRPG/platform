using Krakenar.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Cms.Core.Skills;
using SkillCraft.Cms.Core.Skills.Models;
using SkillCraft.Cms.Models.Parameters;

namespace SkillCraft.Cms.Controllers;

[ApiController]
[Route("api/skills")]
public class SkillController : ControllerBase
{
  private readonly ISkillService _skillService;

  public SkillController(ISkillService skillService)
  {
    _skillService = skillService;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<SkillModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    SkillModel? skill = await _skillService.ReadAsync(id, slug: null, cancellationToken);
    return skill is null ? NotFound() : Ok(skill);
  }

  [HttpGet("slug:{slug}")]
  public async Task<ActionResult<SkillModel>> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    SkillModel? skill = await _skillService.ReadAsync(id: null, slug, cancellationToken);
    return skill is null ? NotFound() : Ok(skill);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<SkillModel>>> SearchAsync([FromQuery] SearchSkillsParameters parameters, CancellationToken cancellationToken)
  {
    SearchSkillsPayload payload = parameters.ToPayload();
    SearchResults<SkillModel> skills = await _skillService.SearchAsync(payload, cancellationToken);
    return Ok(skills);
  }
}
