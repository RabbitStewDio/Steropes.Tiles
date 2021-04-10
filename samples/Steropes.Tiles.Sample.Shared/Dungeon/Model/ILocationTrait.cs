using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Navigation;
using System;

namespace Steropes.Tiles.Sample.Shared.Dungeon.Model
{
    /// <summary>
    ///   This trait manages the fine grained location of items. This allows items
    ///   to stick to the map raster, move freely within map tiles or to transition
    ///   from one map position to the other based on some animation.
    /// </summary>
    public interface ILocationTrait : IItemTrait
    {
        event EventHandler Moved;
        MapCoordinate OccupiedTile { get; set; }
        DoublePoint Position { get; set; }
    }
}
