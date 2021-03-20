using System;
using System.Text;
using Steropes.Tiles.Demo.Core.GameData.Util;

namespace Steropes.Tiles.Demo.Core.GameData.Strategy.Model
{
    /// <summary>
    ///  Container structure to hold all definitions of terrain data.
    /// </summary>
    public struct TerrainData
    {
        const byte RoadMask = 0b0000_0111;
        const byte NotRoadMask = unchecked((byte)~RoadMask);

        /// <summary>
        ///  The index of the terrain in the terrain lookup table.
        /// </summary>
        public byte TerrainIdx;

        /// <summary>
        ///  Player who controls the land. (6bit)
        ///  How control is retained: Farming, Purchased (2 bit)
        /// </summary>
        public byte Owner;

        /// <summary>
        ///  Nearest city that controls the land. 
        /// </summary>
        public byte City;

        /// <summary>
        ///  Resource for this tile, or 0 for none. Resources
        ///  are enumerated, only one resource can exist on a 
        ///  tile.
        /// </summary>
        public byte Resources;

        /// <summary>
        ///  The type of improvement built on the terrain. 
        ///  Multiple improvements can exist on the terrain at
        ///  the same time. 
        /// 
        ///  Rivers (1bit), 
        ///  Encodes Roads/Railways (2bit), 
        ///  Buildings like Irrigation, Mines, Fortress, ... 
        ///  (remaining bits) 
        /// </summary>
        public ushort Improvement;

        public byte Roads
        {
            get { return (byte)(Improvement & RoadMask); }
            set
            {
                Improvement &= NotRoadMask;
                Improvement |= (byte)(value & RoadMask);
            }
        }

        public override string ToString()
        {
            return $"{nameof(TerrainIdx)}: {TerrainIdx}, {nameof(Improvement)}: {Improvement}, {nameof(Owner)}: {Owner}, {nameof(City)}: {City}, {nameof(Roads)}: {Roads}";
        }
    }

    public class TerrainMap : IMap2D<TerrainData>
    {
        readonly TerrainData[,] data;

        public TerrainMap(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.data = new TerrainData[width, height];
        }

        public int Width { get; }
        public int Height { get; }

        public TerrainData this[int x, int y]
        {
            get { return data[x, y]; }
            set { data[x, y] = value; }
        }

        public string Print(Func<TerrainData, char> printer)
        {
            StringBuilder b = new StringBuilder();
            for (int y = 0; y < Height; y += 1)
            {
                for (int x = 0; x < Width; x += 1)
                {
                    var c = printer(this[x, y]);
                    b.Append(c);
                }

                b.Append("\n");
            }

            return b.ToString();
        }
    }
}
