using Logitar.Data;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.EncyclopediaDb;

internal static class Maps
{
  public static readonly TableId Table = new(EncyclopediaContext.Schema, nameof(EncyclopediaContext.Maps), alias: null);

  public static readonly ColumnId CreatedBy = new(nameof(MapEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(MapEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(MapEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(MapEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(MapEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(MapEntity.Version), Table);

  public static readonly ColumnId Height = new(nameof(MapEntity.Height), Table);
  public static readonly ColumnId Id = new(nameof(MapEntity.Id), Table);
  public static readonly ColumnId IsPublished = new(nameof(MapEntity.IsPublished), Table);
  public static readonly ColumnId Key = new(nameof(MapEntity.Key), Table);
  public static readonly ColumnId KeyNormalized = new(nameof(MapEntity.KeyNormalized), Table);
  public static readonly ColumnId MapId = new(nameof(MapEntity.MapId), Table);
  public static readonly ColumnId Source = new(nameof(MapEntity.Source), Table);
  public static readonly ColumnId Title = new(nameof(MapEntity.Title), Table);
  public static readonly ColumnId Width = new(nameof(MapEntity.Width), Table);
}
