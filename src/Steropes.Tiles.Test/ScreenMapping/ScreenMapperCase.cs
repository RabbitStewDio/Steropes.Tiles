namespace Steropes.Tiles.Test.ScreenMapping
{
  public class ScreenMapperCase<TMap, TScreen>
  {
    public TMap MapCoordinate { get; private set; }
    public TScreen ViewCoordinates { get; private set; }

    public ScreenMapperCase(TScreen viewportViewCoordinates)
    {
      this.ViewCoordinates = viewportViewCoordinates;
    }


    public ScreenMapperCase<TMap,TScreen> IsSameAs(TMap c)
    {
      MapCoordinate = c;
      return this;
    }
  }
}