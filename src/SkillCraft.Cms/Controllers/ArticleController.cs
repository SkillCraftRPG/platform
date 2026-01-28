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

  [HttpGet("/api/collections/key:{collection}/articles/path:{path}")]
  public async Task<ActionResult<ArticleModel>> ReadAsync(string collection, string path, CancellationToken cancellationToken)
  {
    path = Uri.UnescapeDataString(path); // TODO(fpion): path is not URI-decoded by default because it is not a query param.
    ArticleModel? article = await _articleService.ReadAsync(collection, path, cancellationToken);
    return article is null ? NotFound() : Ok(article);
  }
}
