using Logitar.Data;
using SkillCraft.Cms.Infrastructure.Entities;

namespace SkillCraft.Cms.Infrastructure.EncyclopediaDb;

internal static class Markers
{
  public static readonly TableId Table = new(EncyclopediaContext.Schema, nameof(EncyclopediaContext.Markers), alias: null);

  public static readonly ColumnId CreatedBy = new(nameof(MarkerEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(MarkerEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(MarkerEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(MarkerEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(MarkerEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(MarkerEntity.Version), Table);

  public static readonly ColumnId HtmlContent = new(nameof(MarkerEntity.HtmlContent), Table);
  public static readonly ColumnId Id = new(nameof(MarkerEntity.Id), Table);
  public static readonly ColumnId IsPublished = new(nameof(MarkerEntity.IsPublished), Table);
  public static readonly ColumnId Key = new(nameof(MarkerEntity.Key), Table);
  public static readonly ColumnId KeyNormalized = new(nameof(MarkerEntity.KeyNormalized), Table);
  public static readonly ColumnId MapId = new(nameof(MarkerEntity.MapId), Table);
  public static readonly ColumnId MapUid = new(nameof(MarkerEntity.MapUid), Table);
  public static readonly ColumnId MarkerId = new(nameof(MarkerEntity.MarkerId), Table);
  public static readonly ColumnId Title = new(nameof(MarkerEntity.Title), Table);
  public static readonly ColumnId X = new(nameof(MarkerEntity.X), Table);
  public static readonly ColumnId Y = new(nameof(MarkerEntity.Y), Table);
}
