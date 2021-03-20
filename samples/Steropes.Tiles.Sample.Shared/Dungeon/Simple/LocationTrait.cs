using System;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Demo.Core.GameData.Dungeon.Model;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Demo.Core.GameData.Dungeon.Simple
{
    /// <summary>
    ///   Allows items to be positioned freely on tiles.
    /// </summary>
    public abstract class LocationTrait : ILocationTrait
    {
        DoublePoint position;
        MapCoordinate occupiedTile;
        public event EventHandler Moved;

        public MapCoordinate OccupiedTile
        {
            get
            {
                return occupiedTile;
            }
            set { Position = new DoublePoint(value.X, value.Y); }
        }

        public DoublePoint Position
        {
            get { return position; }
            set
            {
                var oldTile = occupiedTile;
                var old = position;

                position = value;
                occupiedTile = new MapCoordinate((int)Math.Round(position.X), (int)Math.Round(position.Y));

                if (OnLocationChange())
                {
                    Moved?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    position = old;
                }
            }
        }

        /// <summary>
        ///  Special handler that coordinates map changes. These operations may fail or be invalid,
        ///  and we need to know about them here.
        /// </summary>
        /// <returns></returns>
        protected abstract bool OnLocationChange();
    }
}
