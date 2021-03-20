namespace Steropes.Tiles.Matcher.Registry
{
  /// <summary>
  ///   Holds a list of all available tiles in the system. Tiles need an unique tag/name.
  ///   This registry is for an unconditional lookup. Use ITileRegistryEx for a conditional
  ///   lookup.
  /// </summary>
  /// <typeparam name="TRenderTile"></typeparam>
  public interface ITileRegistry<TRenderTile>
  {
    /// <summary>
    ///   Never return null. If a tile is not found, either throw an error, or return a no-op-tile.
    /// </summary>
    /// <param name="tag"></param>
    /// <returns>true if the tile has been found, false otherwise</returns>
    bool TryFind(string tag, out TRenderTile tile);
  }
}