namespace Steropes.Tiles.Test.ScreenMapping
{
    public class ScreenMapperCase<TMap, TScreen>
    {
        public ScreenMapperCase(TScreen viewportViewCoordinates)
        {
            ViewCoordinates = viewportViewCoordinates;
        }

        public TMap MapCoordinate { get; private set; }
        public TScreen ViewCoordinates { get; }


        public ScreenMapperCase<TMap, TScreen> IsSameAs(TMap c)
        {
            MapCoordinate = c;
            return this;
        }
    }
}
