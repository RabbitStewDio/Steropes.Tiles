using System;
using Steropes.Tiles.Demo.Core.GameData.Dungeon.Model;

namespace Steropes.Tiles.Demo.Core.GameData.Dungeon.Simple
{
    public class WallType : IWallType, IEquatable<WallType>
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

        public bool Equals(WallType other)
        {
            return Equals((IWallType)other);
        }

        public bool Equals(IWallType other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((IWallType)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
