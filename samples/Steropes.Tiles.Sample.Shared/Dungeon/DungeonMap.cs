using Steropes.Tiles.Sample.Shared.Dungeon.Model;
using Steropes.Tiles.Sample.Shared.Util;

namespace Steropes.Tiles.Sample.Shared.Dungeon
{
    /// <summary>
    ///   <para>
    ///     A map with multiple layers.
    ///     In this demo, a dungeon here has multiple layers and is rendered in multiple passes.
    ///     Pass 1. a floor layer (water, stone or dirt). This layer is rendered in its own pass.
    ///     Pass 2. wall, item and character layer. An combined item (chests, book cases) and wall-layer (stone walls, doors)
    ///     Each cell in that pass is rendered completely (and possibly multiple times) before moving on to the next cell.
    ///     Pass 3. finally a effects layer. Explosions, Gas and other sparkling effects.
    ///     Pass 4. Fog of war layer.
    ///   </para>
    ///   <para>
    ///     The cursor for selections must be rendered after the floor-layer, so that selections
    ///     appear below the sprites being rendered.
    ///     Moving sprites must be rendered at the same time the item layer is rendered or
    ///     we would not be able to see a wall overlap an player's sprite.
    ///   </para>
    ///   <para>Finally we render label markers on top of the map</para>
    /// </summary>
    public class DungeonMap
    {
        public int Width { get; }
        public int Height { get; }

        public DungeonMap(int width,
                          int height,
                          IDungeonGameRules rules)
        {
            Width = width;
            Height = height;
            FloorLayer = new Map2D<IFloorType>(width, height, (_, _) => rules.FloorTypes.DefaultValue);
            WallLayer = new Map2D<IWallType>(width, height, (_, _) => rules.WallTypes.DefaultValue);
            DecorationLayer = new Map2D<IDecorationType>(width, height, (_, _) => rules.DecorationTypes.DefaultValue);
        }

        /// <summary>
        ///   There can be only one floor type per coordinate, and there are no decorations or other extras to consider.
        ///   Therefore an simple 2D-Array is enough for this case.
        /// </summary>
        public Map2D<IFloorType> FloorLayer { get; }

        /// <summary>
        ///   The wall layer is also a simple enumerable set with no decorations. Decorations, like torches or picture
        ///   frames, are added in a different layer.
        /// </summary>
        public Map2D<IWallType> WallLayer { get; }

        /// <summary>
        ///   The decoration layer contains zero or one decorations per tile. Decorations are elements like torches and
        ///   picture frames for walls. Floors can have decorations too. Decorations are rendered after the wall tiles.
        /// </summary>
        public Map2D<IDecorationType> DecorationLayer { get; }
    }
}
