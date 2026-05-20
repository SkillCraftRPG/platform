using Krakenar.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Cms.Core.Maps;
using SkillCraft.Cms.Core.Maps.Models;
using SkillCraft.Cms.Models.Parameters;

namespace SkillCraft.Cms.Controllers;

[ApiController]
[Route("api/maps")]
public class MapController : ControllerBase
{
  private readonly IMapService _mapService;

  public MapController(IMapService mapService)
  {
    _mapService = mapService;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<MapModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    MapModel? map = await _mapService.ReadAsync(id, key: null, cancellationToken);
    return map is null ? NotFound() : Ok(map);
  }

  [HttpGet("key:{key}")]
  public async Task<ActionResult<MapModel>> ReadAsync(string key, CancellationToken cancellationToken)
  {
    MapModel? map = await _mapService.ReadAsync(id: null, key, cancellationToken);
    return map is null ? NotFound() : Ok(map);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<MapModel>>> SearchAsync([FromQuery] SearchMapsParameters parameters, CancellationToken cancellationToken)
  {
    SearchMapsPayload payload = parameters.ToPayload();
    SearchResults<MapModel> maps = await _mapService.SearchAsync(payload, cancellationToken);
    return Ok(maps);
  }
}
