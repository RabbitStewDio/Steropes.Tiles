using Steropes.Tiles.Sample.Shared.Dungeon.Model;
using Steropes.Tiles.Sample.Shared.Util;

namespace Steropes.Tiles.Sample.Shared.Dungeon
{
    /// <summary>
    ///   This structure encapsulates all game rules for the dungeon game. Well, as this is a tile
    ///   demo, it actually only holds some basic metadata/information about the entities that can
    ///   exist.
    /// </summary>
    public interface IDungeonGameRules
    {
        ITypeRegistry<IFloorType> FloorTypes { get; }
        ITypeRegistry<IWallType> WallTypes { get; }
        ITypeRegistry<IDecorationType> DecorationTypes { get; }
        ITypeRegistry<IItemType> ItemTypes { get; }
    }
}
