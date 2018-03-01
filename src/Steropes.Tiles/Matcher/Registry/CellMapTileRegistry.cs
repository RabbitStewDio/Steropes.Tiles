using Steropes.Tiles.Matcher.Sprites;

namespace Steropes.Tiles.Matcher.Registry
{
  public class CellMapTileRegistry<TRenderTile> : ITileRegistryEx<CellMapTileSelectorKey, TRenderTile>
  {
    readonly ITileRegistry<TRenderTile> baseRegistry;
    readonly string format;

    public CellMapTileRegistry(ITileRegistry<TRenderTile> baseRegistry, string format = null)
    {
      this.baseRegistry = baseRegistry;
      this.format = format;
    }

    public TRenderTile Find(string tag, CellMapTileSelectorKey tileSelector)
    {
      return baseRegistry.Find(tileSelector.Format(tag, format));
    }

    public bool TryFind(string tag, CellMapTileSelectorKey selector, out TRenderTile tile)
    {
      return baseRegistry.TryFind(selector.Format(tag, format), out tile);
    }
  }
}