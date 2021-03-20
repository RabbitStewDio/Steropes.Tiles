using System;

namespace Steropes.Tiles.Demo.Game
{
    public class TerrainType
    {
        public string Name { get; }
        public int MoveCost { get; }
        public int Farm { get; }
        public int Trade { get; }
        public int Production { get; }

        public TerrainType(string name, int moveCost, int farm, int trade, int production)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            MoveCost = moveCost;
            Farm = farm;
            Trade = trade;
            Production = production;
        }
    }
}
