using SkillCraft.Cms.Core.Articles.Models;

namespace SkillCraft.Cms.Core.Articles;

public interface IArticleQuerier
{
  Task<ArticleModel?> ReadAsync(string collection, string path, CancellationToken cancellationToken = default);
}
