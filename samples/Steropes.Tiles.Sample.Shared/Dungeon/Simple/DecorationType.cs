using System;
using Steropes.Tiles.Demo.Core.GameData.Dungeon.Model;

namespace Steropes.Tiles.Demo.Core.GameData.Dungeon.Simple
{
    public class DecorationType : IDecorationType
    {
        public DecorationType(DecorationTypeId id, string name, float lightSource, float blockVisibility)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            BlockVisibility = blockVisibility;
            LightSource = lightSource;
        }

        public DecorationTypeId Id { get; }

        /// <summary>
        ///   This is the graphic tag. This string connects the map data to the graphic pack tile.
        /// </summary>
        public string Name { get; }

        public float LightSource { get; }
        public float BlockVisibility { get; }

        public bool Equals(IDecorationType other)
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

            return Equals((DecorationType)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ LightSource.GetHashCode();
                hashCode = (hashCode * 397) ^ BlockVisibility.GetHashCode();
                return hashCode;
            }
        }
    }
}
