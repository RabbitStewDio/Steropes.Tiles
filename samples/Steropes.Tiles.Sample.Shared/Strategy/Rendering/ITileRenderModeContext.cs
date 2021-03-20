
namespace Steropes.Tiles.Demo.Core.GameData.Strategy.Rendering
{
  public interface ITileRenderModeContext
  {
    GameRenderingConfig RenderingConfig { get; }
    StrategyGameData GameData { get; }
    IStrategyGameTileSet TileSet { get; }
  }
}