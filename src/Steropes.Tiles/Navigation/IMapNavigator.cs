namespace Steropes.Tiles.Navigation
{
    /// <summary>
    ///  A map navigator knows how to progress from one tile to the next given a abstract
    ///  direction. 
    /// </summary>
    public interface IMapNavigator<in TDirection>
        where TDirection : struct
    {
        bool NavigateTo(TDirection direction, in MapCoordinate source, out MapCoordinate result, int steps = 1);
    }
}
