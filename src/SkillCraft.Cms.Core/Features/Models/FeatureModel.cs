namespace SkillCraft.Cms.Core.Features.Models;

public record FeatureModel
{
  public string Name { get; set; }
  public string? HtmlContent { get; set; }

  public FeatureModel() : this(string.Empty)
  {
  }

  public FeatureModel(string name, string? htmlContent = null)
  {
    Name = name;
    HtmlContent = htmlContent;
  }
} // TODO(fpion): other fields?
