using System;
using Steropes.Tiles.MonogameDemo.GameData.Dungeon.Model;

namespace Steropes.Tiles.MonogameDemo.GameData.Dungeon.Simple
{
  public class DecorationType : IDecorationType
  {
    public DecorationType(DecorationTypeId id, string name, float lightSource, float blockVisibility)
    {
      Id = id;
      Name = name ?? throw new ArgumentNullException(nameof(name));
      BlockVisibility = blockVisibility;
      LightSource = lightSource;
    }

    public DecorationTypeId Id { get; }

    /// <summary>
    ///   This is the graphic tag. This string connects the map data to the graphic pack tile.
    /// </summary>
    public string Name { get; }

    public float LightSource { get; }
    public float BlockVisibility { get; }
  }
}