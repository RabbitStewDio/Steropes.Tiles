using Steropes.Tiles.Plotter.Operations;

namespace Steropes.Tiles.Plotter
{
    /// <summary>
    ///  The Plotter is responsible for navigating both screen and map coordinate spaces
    ///  and for invoking the rendering operation for each elibible tile.
    /// </summary>
    public interface IPlotter
    {
        void Draw(IPlotOperation plotOperation);
    }
}
