using Microsoft.AspNetCore.Mvc;
using SkillCraft.Cms.Core.Articles;
using SkillCraft.Cms.Core.Articles.Models;

namespace SkillCraft.Cms.Controllers;

[ApiController]
[Route("api/articles")]
public class ArticleController : ControllerBase
{
  private readonly IArticleService _articleService;

  public ArticleController(IArticleService articleService)
  {
    _articleService = articleService;
  }

  [HttpGet("/api/collections/{collection}/articles/{**path}")]
  public async Task<ActionResult<ArticleModel>> ReadAsync(string collection, string path, CancellationToken cancellationToken)
  {
    ArticleModel? article = await _articleService.ReadAsync(collection, path, cancellationToken);
    return article is null ? NotFound() : Ok(article);
  }
}
