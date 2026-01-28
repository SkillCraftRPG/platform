using Krakenar.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Cms.Core.Castes;
using SkillCraft.Cms.Core.Castes.Models;
using SkillCraft.Cms.Models.Parameters;

namespace SkillCraft.Cms.Controllers;

[ApiController]
[Route("api/castes")]
public class CasteController : ControllerBase
{
  private readonly ICasteService _casteService;

  public CasteController(ICasteService casteService)
  {
    _casteService = casteService;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<CasteModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    CasteModel? caste = await _casteService.ReadAsync(id, slug: null, cancellationToken);
    return caste is null ? NotFound() : Ok(caste);
  }

  [HttpGet("slug:{slug}")]
  public async Task<ActionResult<CasteModel>> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    CasteModel? caste = await _casteService.ReadAsync(id: null, slug, cancellationToken);
    return caste is null ? NotFound() : Ok(caste);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<CasteModel>>> SearchAsync([FromQuery] SearchCastesParameters parameters, CancellationToken cancellationToken)
  {
    SearchCastesPayload payload = parameters.ToPayload();
    SearchResults<CasteModel> castes = await _casteService.SearchAsync(payload, cancellationToken);
    return Ok(castes);
  }
}
