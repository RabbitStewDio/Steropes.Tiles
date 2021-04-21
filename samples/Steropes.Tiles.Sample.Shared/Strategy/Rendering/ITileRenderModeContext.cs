namespace Steropes.Tiles.Sample.Shared.Strategy.Rendering
{
    public interface ITileRenderModeContext
    {
        GameRenderingConfig RenderingConfig { get; }
        StrategyGameData GameData { get; }
        IStrategyGameTileSet TileSet { get; }
    }
}
