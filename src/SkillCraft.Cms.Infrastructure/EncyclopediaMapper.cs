using Krakenar.Contracts;
using Krakenar.Contracts.Actors;
using Logitar;
using Logitar.EventSourcing;
using SkillCraft.Cms.Core.Articles.Models;
using SkillCraft.Cms.Core.Collections.Models;
using SkillCraft.Cms.Infrastructure.Entities;
using AggregateEntity = Krakenar.EntityFrameworkCore.Relational.Entities.Aggregate;

namespace SkillCraft.Cms.Infrastructure;

internal class EncyclopediaMapper
{
  private readonly Dictionary<ActorId, Actor> _actors = [];
  private readonly Actor _system = new();

  public EncyclopediaMapper()
  {
  }

  public EncyclopediaMapper(IReadOnlyDictionary<ActorId, Actor> actors)
  {
    foreach (KeyValuePair<ActorId, Actor> actor in actors)
    {
      _actors[actor.Key] = actor.Value;
    }
  }

  public ArticleModel ToArticle(ArticleEntity source)
  {
    if (source.Collection is null)
    {
      throw new ArgumentException("The collection is required.", nameof(source));
    }

    ArticleModel destination = new()
    {
      Id = source.Id,
      Slug = source.Slug,
      Title = source.Title,
      MetaDescription = source.MetaDescription,
      HtmlContent = source.HtmlContent,
      Collection = ToCollection(source.Collection)
    };

    if (source.Parent is not null)
    {
      destination.Parent = ToArticle(source.Parent);
    }

    MapAggregate(source, destination);

    return destination;
  }

  public CollectionModel ToCollection(CollectionEntity source)
  {
    CollectionModel destination = new()
    {
      Id = source.Id,
      Key = source.Key,
      Name = source.Name
    };

    MapAggregate(source, destination);

    return destination;
  }

  private void MapAggregate(AggregateEntity source, Aggregate destination)
  {
    destination.Version = source.Version;

    destination.CreatedBy = FindActor(source.CreatedBy);
    destination.CreatedOn = source.CreatedOn.AsUniversalTime();

    destination.UpdatedBy = FindActor(source.UpdatedBy);
    destination.UpdatedOn = source.UpdatedOn.AsUniversalTime();
  }

  private Actor FindActor(string? id) => TryFindActor(id) ?? _system;
  private Actor FindActor(ActorId? id) => TryFindActor(id) ?? _system;

  private Actor? TryFindActor(string? id) => TryFindActor(string.IsNullOrWhiteSpace(id) ? null : new ActorId(id));
  private Actor? TryFindActor(ActorId? id) => id.HasValue && _actors.TryGetValue(id.Value, out Actor? actor) ? actor : null;
}
