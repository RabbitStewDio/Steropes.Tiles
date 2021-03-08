using System;

namespace Steropes.Tiles.MonogameDemo.GameData.Strategy.Model
{
  /// <summary>
  ///  A reference to another terrain definition. Type safe string wrapper so that
  /// we can let the compiler help us with the correct use of this thing.
  /// </summary>
  public struct TerrainReference
  {
    public string Target { get; }

    public TerrainReference(string target)
    {
      Target = target ?? throw new ArgumentNullException(nameof(target));
    }
  }
}