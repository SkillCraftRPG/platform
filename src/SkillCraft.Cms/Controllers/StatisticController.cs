using Krakenar.Contracts.Search;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Cms.Core.Statistics;
using SkillCraft.Cms.Core.Statistics.Models;
using SkillCraft.Cms.Models.Parameters;

namespace SkillCraft.Cms.Controllers;

[ApiController]
[Route("api/statistics")]
public class StatisticController : ControllerBase
{
  private readonly IStatisticService _statisticService;

  public StatisticController(IStatisticService statisticService)
  {
    _statisticService = statisticService;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<StatisticModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    StatisticModel? statistic = await _statisticService.ReadAsync(id, slug: null, cancellationToken);
    return statistic is null ? NotFound() : Ok(statistic);
  }

  [HttpGet("slug:{slug}")]
  public async Task<ActionResult<StatisticModel>> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    StatisticModel? statistic = await _statisticService.ReadAsync(id: null, slug, cancellationToken);
    return statistic is null ? NotFound() : Ok(statistic);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<StatisticModel>>> SearchAsync([FromQuery] SearchStatisticsParameters parameters, CancellationToken cancellationToken)
  {
    SearchStatisticsPayload payload = parameters.ToPayload();
    SearchResults<StatisticModel> statistics = await _statisticService.SearchAsync(payload, cancellationToken);
    return Ok(statistics);
  }
}
