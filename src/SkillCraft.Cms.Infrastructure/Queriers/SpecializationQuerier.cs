using Krakenar.Contracts.Actors;
using Krakenar.Contracts.Search;
using Krakenar.Core.Actors;
using Krakenar.EntityFrameworkCore.Relational;
using Krakenar.EntityFrameworkCore.Relational.KrakenarDb;
using Logitar.Data;
using Logitar.EventSourcing;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Cms.Core.Specializations;
using SkillCraft.Cms.Core.Specializations.Models;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.Queriers;

internal class SpecializationQuerier : ISpecializationQuerier
{
  private readonly IActorService _actorService;
  private readonly DbSet<DoctrineDiscountedTalentEntity> _discountedTalents;
  private readonly DbSet<SpecializationOptionalTalentEntity> _optionalTalents;
  private readonly DbSet<SpecializationEntity> _specializations;
  private readonly ISqlHelper _sqlHelper;

  public SpecializationQuerier(IActorService actorService, RulesContext rules, ISqlHelper sqlHelper)
  {
    _actorService = actorService;
    _discountedTalents = rules.DoctrineDiscountedTalents;
    _optionalTalents = rules.SpecializationOptionalTalents;
    _specializations = rules.Specializations;
    _sqlHelper = sqlHelper;
  }

  public async Task<SpecializationTree?> GetTreeAsync(Guid id, CancellationToken cancellationToken)
  {
    var specializations = await _specializations
      .Select(x => new
      {
        x.Id,
        ParentIds = x.Origins.Select(origin => origin.OriginUid),
        x.Tier,
        x.Slug,
        x.Name,
        Doctrine = x.Doctrine!.Name,
        x.Summary
      })
      .ToArrayAsync(cancellationToken);

    Dictionary<Guid, InternalSpecializationNode> nodes = new(capacity: specializations.Length);
    foreach (var specialization in specializations)
    {
      nodes[specialization.Id] = new InternalSpecializationNode(specialization.Id, specialization.Tier, specialization.Slug, specialization.Name, specialization.Doctrine, specialization.Summary);
    }

    InternalSpecializationNode? target = nodes.GetValueOrDefault(id);
    if (target is null)
    {
      return null;
    }

    foreach (var specialization in specializations)
    {
      foreach (Guid parentId in specialization.ParentIds)
      {
        InternalSpecializationNode child = nodes[specialization.Id];
        InternalSpecializationNode parent = nodes[parentId];

        child.Parents.Add(parent);
        parent.Children.Add(child);
      }
    }
    return Flatten(target);
  }

  public async Task<SpecializationModel?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    SpecializationEntity? specialization = await _specializations.AsNoTracking()
      .Where(x => x.Id == id && x.IsPublished)
      .Include(x => x.Doctrine).ThenInclude(x => x!.Features).ThenInclude(x => x.Feature)
      .Include(x => x.MandatoryTalent).ThenInclude(x => x!.RequiredTalent).ThenInclude(x => x!.Skill).ThenInclude(x => x!.Attribute)
      .Include(x => x.MandatoryTalent).ThenInclude(x => x!.Skill).ThenInclude(x => x!.Attribute)
      .SingleOrDefaultAsync(cancellationToken);
    if (specialization is null)
    {
      return null;
    }
    await FillAsync(specialization, cancellationToken);
    return await MapAsync(specialization, cancellationToken);
  }
  public async Task<SpecializationModel?> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    string slugNormalized = Helper.Normalize(slug);
    SpecializationEntity? specialization = await _specializations.AsNoTracking()
      .Where(x => x.SlugNormalized == slugNormalized && x.IsPublished)
      .Include(x => x.Doctrine).ThenInclude(x => x!.Features).ThenInclude(x => x.Feature)
      .Include(x => x.MandatoryTalent).ThenInclude(x => x!.RequiredTalent).ThenInclude(x => x!.Skill).ThenInclude(x => x!.Attribute)
      .Include(x => x.MandatoryTalent).ThenInclude(x => x!.Skill).ThenInclude(x => x!.Attribute)
      .SingleOrDefaultAsync(cancellationToken);
    if (specialization is null)
    {
      return null;
    }
    await FillAsync(specialization, cancellationToken);
    return await MapAsync(specialization, cancellationToken);
  }
  private async Task FillAsync(SpecializationEntity specialization, CancellationToken cancellationToken)
  {
    SpecializationOptionalTalentEntity[] optionalTalents = await _optionalTalents.AsNoTracking()
      .Where(x => x.SpecializationId == specialization.SpecializationId)
      .Include(x => x.Talent).ThenInclude(x => x!.RequiredTalent).ThenInclude(x => x!.Skill).ThenInclude(x => x!.Attribute)
      .Include(x => x.Talent).ThenInclude(x => x!.Skill).ThenInclude(x => x!.Attribute)
      .ToArrayAsync(cancellationToken);
    specialization.OptionalTalents.AddRange(optionalTalents);

    if (specialization.Doctrine is not null)
    {
      DoctrineDiscountedTalentEntity[] discountedTalents = await _discountedTalents.AsNoTracking()
        .Where(x => x.DoctrineId == specialization.Doctrine.DoctrineId)
        .Include(x => x.Talent).ThenInclude(x => x!.RequiredTalent).ThenInclude(x => x!.Skill).ThenInclude(x => x!.Attribute)
        .Include(x => x.Talent).ThenInclude(x => x!.Skill).ThenInclude(x => x!.Attribute)
        .ToArrayAsync(cancellationToken);
      specialization.Doctrine.DiscountedTalents.AddRange(discountedTalents);
    }
  }

  public async Task<SearchResults<SpecializationModel>> SearchAsync(SearchSpecializationsPayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = _sqlHelper.Query(RulesDb.Specializations.Table).SelectAll(RulesDb.Specializations.Table)
      .LeftJoin(RulesDb.Doctrines.SpecializationId, RulesDb.Specializations.SpecializationId)
      .ApplyIdFilter(RulesDb.Specializations.Id, payload.Ids)
      .Where(RulesDb.Specializations.IsPublished, Operators.IsEqualTo(true));
    _sqlHelper.ApplyTextSearch(builder, payload.Search, RulesDb.Specializations.Slug, RulesDb.Specializations.Name, RulesDb.Specializations.Summary, RulesDb.Doctrines.Name);

    IQueryable<SpecializationEntity> query = _specializations.FromQuery(builder).AsNoTracking()
      .Include(x => x.Doctrine)
      .Include(x => x.MandatoryTalent).ThenInclude(x => x!.RequiredTalent).ThenInclude(x => x!.Skill).ThenInclude(x => x!.Attribute)
      .Include(x => x.MandatoryTalent).ThenInclude(x => x!.Skill).ThenInclude(x => x!.Attribute);

    long total = await query.LongCountAsync(cancellationToken);

    IOrderedQueryable<SpecializationEntity>? ordered = null;
    foreach (SpecializationSortOption sort in payload.Sort)
    {
      switch (sort.Field)
      {
        case SpecializationSort.CreatedOn:
          ordered = (ordered is null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.CreatedOn) : query.OrderBy(x => x.CreatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.CreatedOn) : ordered.ThenBy(x => x.CreatedOn));
          break;
        case SpecializationSort.Name:
          ordered = (ordered is null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.Name ?? x.Slug) : query.OrderBy(x => x.Name ?? x.Slug))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.Name ?? x.Slug) : ordered.ThenBy(x => x.Name ?? x.Slug));
          break;
        case SpecializationSort.Slug:
          ordered = (ordered is null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.Slug) : query.OrderBy(x => x.Slug))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.Slug) : ordered.ThenBy(x => x.Slug));
          break;
        case SpecializationSort.UpdatedOn:
          ordered = (ordered is null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UpdatedOn) : query.OrderBy(x => x.UpdatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UpdatedOn) : ordered.ThenBy(x => x.UpdatedOn));
          break;
      }
    }
    query = ordered ?? query;

    query = query.ApplyPaging(payload);

    SpecializationEntity[] entities = await query.ToArrayAsync(cancellationToken);
    IReadOnlyCollection<SpecializationModel> specializations = await MapAsync(entities, cancellationToken);

    return new SearchResults<SpecializationModel>(specializations, total);
  }

  private async Task<SpecializationModel> MapAsync(SpecializationEntity specialization, CancellationToken cancellationToken)
  {
    return (await MapAsync([specialization], cancellationToken)).Single();
  }
  private async Task<IReadOnlyCollection<SpecializationModel>> MapAsync(IEnumerable<SpecializationEntity> specializations, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = specializations.SelectMany(specialization => specialization.GetActorIds());
    IReadOnlyDictionary<ActorId, Actor> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    RulesMapper mapper = new(actors);

    return specializations.Select(mapper.ToSpecialization).ToList().AsReadOnly();
  }

  private class InternalSpecializationNode
  {
    public Guid Id { get; }
    public List<InternalSpecializationNode> Parents { get; } = [];
    public List<InternalSpecializationNode> Children { get; } = [];

    public int Tier { get; }
    public string Slug { get; }
    public string Name { get; }

    public string? Doctrine { get; }
    public string? Summary { get; }

    public InternalSpecializationNode(Guid id, int tier, string slug, string name, string? doctrine, string? summary)
    {
      Id = id;

      Tier = tier;
      Slug = slug;
      Name = name;

      Doctrine = doctrine;
      Summary = summary;
    }

    public override bool Equals(object? obj) => obj is InternalSpecializationNode specialization && specialization.Id == Id;
    public override int GetHashCode() => Id.GetHashCode();
    public override string ToString() => $"{Name} (Id={Id})";
  }
  private static SpecializationTree Flatten(InternalSpecializationNode node)
  {
    Dictionary<Guid, SpecializationNode> nodes = [];
    nodes[node.Id] = Map(node);
    foreach (InternalSpecializationNode parent in node.Parents)
    {
      MapParentRecursive(parent, nodes);
    }
    foreach (InternalSpecializationNode child in node.Children)
    {
      MapChildrenRecursive(child, nodes);
    }
    return new SpecializationTree(nodes.Values.OrderBy(specialization => specialization.Tier)
      .ThenBy(specialization => specialization.Name)
      .ThenBy(specialization => specialization.Slug));
  }
  private static void MapParentRecursive(InternalSpecializationNode node, IDictionary<Guid, SpecializationNode> nodes)
  {
    if (!nodes.ContainsKey(node.Id))
    {
      nodes[node.Id] = Map(node);
      foreach (InternalSpecializationNode parent in node.Parents)
      {
        MapParentRecursive(parent, nodes);
      }
    }
  }
  private static void MapChildrenRecursive(InternalSpecializationNode node, IDictionary<Guid, SpecializationNode> nodes)
  {
    if (!nodes.ContainsKey(node.Id))
    {
      nodes[node.Id] = Map(node);
      foreach (InternalSpecializationNode child in node.Children)
      {
        MapChildrenRecursive(child, nodes);
      }
    }
  }
  private static SpecializationNode Map(InternalSpecializationNode specialization) => new()
  {
    Id = specialization.Id,
    Parents = specialization.Parents.Select(parent => parent.Id).Distinct().OrderBy(id => id).ToList(),
    Children = specialization.Children.Select(child => child.Id).Distinct().OrderBy(id => id).ToList(),
    Tier = specialization.Tier,
    Slug = specialization.Slug,
    Name = specialization.Name,
    Doctrine = specialization.Doctrine,
    Summary = specialization.Summary
  };
}
