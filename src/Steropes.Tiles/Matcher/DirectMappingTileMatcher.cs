using Steropes.Tiles.Matcher.Sprites;

namespace Steropes.Tiles.Matcher
{
  public delegate bool Mapper<in TSourceTile, TRenderTile, TContext>(TSourceTile key, out TRenderTile result, out TContext context);

  /// <summary>
  ///   A special case tile matcher for when there is a one-to-one relationship between the source information in
  ///   the map and the renderable tiles used by the graphical output.
  /// </summary>
  /// <typeparam name="TRenderTile"></typeparam>
  /// <typeparam name="TSourceTile"></typeparam>
  /// <typeparam name="TContext"></typeparam>
  public class DirectMappingTileMatcher<TSourceTile, TRenderTile, TContext> : ITileMatcher<TRenderTile, TContext>
  {
    readonly Mapper<TSourceTile, TRenderTile, TContext> mapper;
    readonly MapQuery<TSourceTile> readMap;

    public DirectMappingTileMatcher(MapQuery<TSourceTile> readMap, Mapper<TSourceTile, TRenderTile, TContext> mapper)
    {
      this.readMap = readMap;
      this.mapper = mapper;
    }

    public bool Match(int x, int y, TileResultCollector<TRenderTile, TContext> onMatchFound)
    {
      var sourceTile = readMap(x, y);
      if (mapper(sourceTile, out TRenderTile mapResult, out TContext context))
      {
        onMatchFound(SpritePosition.Whole, mapResult, context);
        return true;
      }
      return false;
    }
  }
}