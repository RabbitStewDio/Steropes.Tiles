using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Monogame.Tiles;

namespace Steropes.Tiles.MonogameDemo.GameData.Strategy.Rendering
{
  public interface ITileRenderModeContext
  {
    GameRenderingConfig RenderingConfig { get; }
    StrategyGameData GameData { get; }
    ITileRegistry<ITexturedTile> TileRegistry { get; }
    IStrategyGameTileSet TileSet { get; }
  }
}