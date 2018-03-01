namespace Steropes.Tiles.Matcher
{
  public sealed class NoOpTileMatcher<TRenderTile, TContext>: ITileMatcher<TRenderTile, TContext>
  {
    public static ITileMatcher<TRenderTile, TContext> Instance { get; } = new NoOpTileMatcher<TRenderTile, TContext>();

    public bool Match(int x, int y, TileResultCollector<TRenderTile, TContext> onMatchFound)
    {
      return false;
    }
  }
}