namespace Steropes.Tiles.Matcher
{
    public interface ITileMatcher<out TRenderTile, out TContext>
    {
        /// <summary>
        ///  Reads the input map at position (x,y) and produces a matching render tile based
        ///  on the map data found and the context of that map entry.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="onMatchFound">This callback receives all matched tiles.</param>
        /// <returns></returns>
        bool Match(int x, int y, TileResultCollector<TRenderTile, TContext> onMatchFound);
    }
}
