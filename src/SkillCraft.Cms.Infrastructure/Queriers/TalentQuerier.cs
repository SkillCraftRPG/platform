using Krakenar.Contracts.Actors;
using Krakenar.Contracts.Search;
using Krakenar.Core.Actors;
using Krakenar.EntityFrameworkCore.Relational;
using Krakenar.EntityFrameworkCore.Relational.KrakenarDb;
using Logitar.Data;
using Logitar.EventSourcing;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Cms.Core.Talents;
using SkillCraft.Cms.Core.Talents.Models;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Queriers;

internal class TalentQuerier : ITalentQuerier
{
  private readonly IActorService _actorService;
  private readonly ISqlHelper _sqlHelper;
  private readonly DbSet<TalentEntity> _talents;

  public TalentQuerier(IActorService actorService, RulesContext rules, ISqlHelper sqlHelper)
  {
    _actorService = actorService;
    _sqlHelper = sqlHelper;
    _talents = rules.Talents;
  }

  public async Task<TalentTree?> GetTreeAsync(Guid id, CancellationToken cancellationToken)
  {
    var talents = await _talents
      .Select(x => new
      {
        x.Id,
        ParentId = x.RequiredTalentUid,
        x.Tier,
        x.Slug,
        x.Name,
        x.AllowMultiplePurchases,
        Skill = x.Skill!.Name,
        x.Summary
      })
      .ToArrayAsync(cancellationToken);

    Dictionary<Guid, InternalTalentNode> nodes = new(capacity: talents.Length);
    foreach (var talent in talents)
    {
      nodes[talent.Id] = new InternalTalentNode(talent.Id, talent.Tier, talent.Slug, talent.Name, talent.AllowMultiplePurchases, talent.Skill, talent.Summary);
    }

    InternalTalentNode? target = nodes.GetValueOrDefault(id);
    if (target is null)
    {
      return null;
    }

    foreach (var talent in talents)
    {
      if (talent.ParentId.HasValue)
      {
        InternalTalentNode child = nodes[talent.Id];
        InternalTalentNode parent = nodes[talent.ParentId.Value];

        child.Parent = parent;
        parent.Children.Add(child);
      }
    }

    InternalTalentNode root = target;
    while (root.Parent is not null)
    {
      root = root.Parent;
    }
    return Flatten(root);
  }

  public async Task<TalentModel?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    TalentEntity? talent = await _talents.AsNoTracking()
      .Where(x => x.Id == id && x.IsPublished)
      .Include(x => x.Skill).ThenInclude(x => x!.Attribute)
      .Include(x => x.RequiredTalent).ThenInclude(x => x!.Skill).ThenInclude(x => x!.Attribute)
      .SingleOrDefaultAsync(cancellationToken);
    return talent is null ? null : await MapAsync(talent, cancellationToken);
  }
  public async Task<TalentModel?> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    string slugNormalized = Helper.Normalize(slug);
    TalentEntity? talent = await _talents.AsNoTracking()
      .Where(x => x.SlugNormalized == slugNormalized && x.IsPublished)
      .Include(x => x.Skill).ThenInclude(x => x!.Attribute)
      .Include(x => x.RequiredTalent).ThenInclude(x => x!.Skill).ThenInclude(x => x!.Attribute)
      .SingleOrDefaultAsync(cancellationToken);
    return talent is null ? null : await MapAsync(talent, cancellationToken);
  }

  public async Task<SearchResults<TalentModel>> SearchAsync(SearchTalentsPayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = _sqlHelper.Query(RulesDb.Talents.Table).SelectAll(RulesDb.Talents.Table)
      .ApplyIdFilter(RulesDb.Talents.Id, payload.Ids)
      .Where(RulesDb.Talents.IsPublished, Operators.IsEqualTo(true));
    _sqlHelper.ApplyTextSearch(builder, payload.Search, RulesDb.Talents.Slug, RulesDb.Talents.Name, RulesDb.Talents.Summary);

    if (payload.Slugs.Count > 0)
    {
      string[] normalizedSlugs = payload.Slugs.Where(slug => !string.IsNullOrWhiteSpace(slug))
        .Select(Helper.Normalize)
        .Distinct()
        .ToArray();
      builder.Where(RulesDb.Talents.SlugNormalized, Operators.IsIn(normalizedSlugs));
    }
    if (payload.Tiers.Count > 0)
    {
      object[] tiers = payload.Tiers.Distinct().Select(tier => (object)tier).ToArray();
      builder.Where(RulesDb.Talents.Tier, Operators.IsIn(tiers));
    }
    if (!string.IsNullOrWhiteSpace(payload.Skill))
    {
      string trimmed = payload.Skill.Trim();
      if (Guid.TryParse(trimmed, out Guid skillUid))
      {
        builder.Where(RulesDb.Talents.SkillUid, Operators.IsEqualTo(skillUid));
      }
      else if (trimmed.Equals("any", StringComparison.InvariantCultureIgnoreCase))
      {
        builder.Where(RulesDb.Talents.SkillId, Operators.IsNotNull());
      }
      else if (trimmed.Equals("none", StringComparison.InvariantCultureIgnoreCase))
      {
        builder.Where(RulesDb.Talents.SkillId, Operators.IsNull());
      }
    }
    if (payload.AllowMultiplePurchases.HasValue)
    {
      builder.Where(RulesDb.Talents.AllowMultiplePurchases, Operators.IsEqualTo(payload.AllowMultiplePurchases.Value));
    }
    if (payload.RequiredTalentId.HasValue)
    {
      builder.Where(RulesDb.Talents.RequiredTalentUid, Operators.IsEqualTo(payload.RequiredTalentId.Value));
    }

    IQueryable<TalentEntity> query = _talents.FromQuery(builder).AsNoTracking()
      .Include(x => x.Skill).ThenInclude(x => x!.Attribute)
      .Include(x => x.RequiredTalent).ThenInclude(x => x!.Skill).ThenInclude(x => x!.Attribute);

    long total = await query.LongCountAsync(cancellationToken);

    IOrderedQueryable<TalentEntity>? ordered = null;
    foreach (TalentSortOption sort in payload.Sort)
    {
      switch (sort.Field)
      {
        case TalentSort.CreatedOn:
          ordered = (ordered is null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.CreatedOn) : query.OrderBy(x => x.CreatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.CreatedOn) : ordered.ThenBy(x => x.CreatedOn));
          break;
        case TalentSort.Name:
          ordered = (ordered is null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.Name ?? x.Slug) : query.OrderBy(x => x.Name ?? x.Slug))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.Name ?? x.Slug) : ordered.ThenBy(x => x.Name ?? x.Slug));
          break;
        case TalentSort.Slug:
          ordered = (ordered is null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.Slug) : query.OrderBy(x => x.Slug))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.Slug) : ordered.ThenBy(x => x.Slug));
          break;
        case TalentSort.UpdatedOn:
          ordered = (ordered is null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UpdatedOn) : query.OrderBy(x => x.UpdatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UpdatedOn) : ordered.ThenBy(x => x.UpdatedOn));
          break;
      }
    }
    query = ordered ?? query;

    query = query.ApplyPaging(payload);

    TalentEntity[] entities = await query.ToArrayAsync(cancellationToken);
    IReadOnlyCollection<TalentModel> talents = await MapAsync(entities, cancellationToken);

    return new SearchResults<TalentModel>(talents, total);
  }

  private async Task<TalentModel> MapAsync(TalentEntity talent, CancellationToken cancellationToken)
  {
    return (await MapAsync([talent], cancellationToken)).Single();
  }
  private async Task<IReadOnlyCollection<TalentModel>> MapAsync(IEnumerable<TalentEntity> talents, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = talents.SelectMany(talent => talent.GetActorIds());
    IReadOnlyDictionary<ActorId, Actor> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    RulesMapper mapper = new(actors);

    return talents.Select(mapper.ToTalent).ToList().AsReadOnly();
  }

  private class InternalTalentNode
  {
    public Guid Id { get; }
    public InternalTalentNode? Parent { get; set; }
    public List<InternalTalentNode> Children { get; } = [];

    public int Tier { get; }
    public string Slug { get; }
    public string Name { get; }

    public bool AllowMultiplePurchases { get; }
    public string? Skill { get; }
    public string? Summary { get; }

    public InternalTalentNode(Guid id, int tier, string slug, string name, bool allowMultiplePurchases, string? skill, string? summary)
    {
      Id = id;

      Tier = tier;
      Slug = slug;
      Name = name;

      AllowMultiplePurchases = allowMultiplePurchases;
      Skill = skill;
      Summary = summary;
    }

    public override bool Equals(object? obj) => obj is InternalTalentNode talent && talent.Id == Id;
    public override int GetHashCode() => Id.GetHashCode();
    public override string ToString() => $"{Name} (Id={Id})";
  }

  private static TalentTree Flatten(InternalTalentNode root) => new(MapRecursive(root).OrderBy(talent => talent.Name).ThenBy(talent => talent.Slug));
  private static IReadOnlyCollection<TalentNode> MapRecursive(InternalTalentNode node)
  {
    List<TalentNode> nodes = [];
    nodes.Add(Map(node));
    foreach (InternalTalentNode child in node.Children)
    {
      nodes.AddRange(MapRecursive(child));
    }
    return nodes.ToList().AsReadOnly();
  }
  private static TalentNode Map(InternalTalentNode talent) => new()
  {
    Id = talent.Id,
    Parent = talent.Parent?.Id,
    Children = talent.Children.Select(child => child.Id).Distinct().OrderBy(id => id).ToList(),
    Tier = talent.Tier,
    Slug = talent.Slug,
    Name = talent.Name,
    AllowMultiplePurchases = talent.AllowMultiplePurchases,
    Skill = talent.Skill,
    Summary = talent.Summary
  };
}
