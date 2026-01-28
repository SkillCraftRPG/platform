using Krakenar.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Cms.Core.Spells;
using SkillCraft.Cms.Core.Spells.Models;
using SkillCraft.Cms.Models.Parameters;

namespace SkillCraft.Cms.Controllers;

[ApiController]
[Route("api/spells")]
public class SpellController : ControllerBase
{
  private readonly ISpellService _spellService;

  public SpellController(ISpellService spellService)
  {
    _spellService = spellService;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<SpellModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    SpellModel? spell = await _spellService.ReadAsync(id, slug: null, cancellationToken);
    return spell is null ? NotFound() : Ok(spell);
  }

  [HttpGet("slug:{slug}")]
  public async Task<ActionResult<SpellModel>> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    SpellModel? spell = await _spellService.ReadAsync(id: null, slug, cancellationToken);
    return spell is null ? NotFound() : Ok(spell);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<SpellModel>>> SearchAsync([FromQuery] SearchSpellsParameters parameters, CancellationToken cancellationToken)
  {
    SearchSpellsPayload payload = parameters.ToPayload();
    SearchResults<SpellModel> spells = await _spellService.SearchAsync(payload, cancellationToken);
    return Ok(spells);
  }
}
