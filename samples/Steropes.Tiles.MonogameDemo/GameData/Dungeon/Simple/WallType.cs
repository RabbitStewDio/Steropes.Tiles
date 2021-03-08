using System;
using Steropes.Tiles.MonogameDemo.GameData.Dungeon.Model;

namespace Steropes.Tiles.MonogameDemo.GameData.Dungeon.Simple
{
    public class WallType: IWallType
    {
        public WallType(WallTypeId id, string name, bool obstructWalking, bool obstructSight)
        {
            Id = id;
            ObstructWalking = obstructWalking;
            ObstructSight = obstructSight;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public WallTypeId Id { get; }
        public string Name { get; }
        public bool ObstructWalking { get; }
        public bool ObstructSight { get; }
    }
}