using Microsoft.AspNetCore.Mvc;
using SkillCraft.Cms.Core.Quests;
using SkillCraft.Cms.Core.Quests.Models;

namespace SkillCraft.Cms.Controllers;

[ApiController]
[Route("api/quests/logs")]
public class QuestLogController : ControllerBase
{
  private readonly IQuestService _questService;

  public QuestLogController(IQuestService questService)
  {
    _questService = questService;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<QuestModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    QuestLogModel? questLog = await _questService.ReadLogAsync(id, slug: null, cancellationToken);
    return questLog is null ? NotFound() : Ok(questLog);
  }

  [HttpGet("slug:{slug}")]
  public async Task<ActionResult<QuestModel>> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    QuestLogModel? questLog = await _questService.ReadLogAsync(id: null, slug, cancellationToken);
    return questLog is null ? NotFound() : Ok(questLog);
  }
}
