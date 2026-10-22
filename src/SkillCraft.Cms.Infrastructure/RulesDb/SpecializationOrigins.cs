using Logitar.Data;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.RulesDb;

internal static class SpecializationOrigins
{
  public static readonly TableId Table = new(RulesContext.Schema, nameof(RulesContext.SpecializationOrigins), alias: null);

  public static readonly ColumnId SpecializationId = new(nameof(SpecializationOriginEntity.SpecializationId), Table);
  public static readonly ColumnId SpecializationUid = new(nameof(SpecializationOriginEntity.SpecializationUid), Table);
  public static readonly ColumnId OriginId = new(nameof(SpecializationOriginEntity.OriginId), Table);
  public static readonly ColumnId OriginUid = new(nameof(SpecializationOriginEntity.OriginUid), Table);
}
