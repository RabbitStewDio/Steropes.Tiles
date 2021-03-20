using System;

namespace Steropes.Tiles.Demo.Game
{
    public struct TerrainEntry
    {
        public TerrainType Terrain;
        public bool River;
        public bool Road;

        public TerrainEntry(TerrainType terrain) : this()
        {
            Terrain = terrain ?? throw new ArgumentNullException(nameof(terrain));
            River = false;
            Road = false;
        }

        public TerrainEntry(TerrainType terrain, bool river, bool road)
        {
            Terrain = terrain ?? throw new ArgumentNullException(nameof(terrain));
            River = river;
            Road = road;
        }
    }

    public class TerrainMap
    {
        readonly TerrainEntry[,] map;

        public TerrainMap(int width, int height)
        {
            map = new TerrainEntry[width, height];
            Width = width;
            Height = height;
        }

        public int Width { get; }
        public int Height { get; }

        public TerrainEntry this[int x, int y]
        {
            get { return map[x, y]; }
            set { map[x, y] = value; }
        }

        public static TerrainMap CreateMap(TerrainTypes t)
        {
            TerrainMap map = new TerrainMap(10, 10);
            for (int x = 0; x < map.Width; x += 1)
            for (int y = 0; y < map.Height; y += 1)
            {
                map[x, y] = new TerrainEntry(t.Desert);
            }

            map[0, 0] = new TerrainEntry(t.Hills, true, true);
            map[9, 9] = new TerrainEntry(t.Hills, true, true);
            map[0, 9] = new TerrainEntry(t.Hills, true, true);
            map[9, 0] = new TerrainEntry(t.Hills, true, true);

            map[2, 2] = new TerrainEntry(t.Hills, true, true);
            map[2, 3] = new TerrainEntry(t.Plains, false, true);
            map[2, 4] = new TerrainEntry(t.Plains, false, false);
            map[3, 2] = new TerrainEntry(t.Plains, false, false);
            map[3, 3] = new TerrainEntry(t.Grasland, true, false);
            map[3, 4] = new TerrainEntry(t.Grasland, false, true);
            map[3, 5] = new TerrainEntry(t.Plains, false, false);
            map[4, 2] = new TerrainEntry(t.Grasland, false, false);
            map[4, 3] = new TerrainEntry(t.Grasland, true, true);
            map[4, 4] = new TerrainEntry(t.Grasland, false, true);
            map[4, 5] = new TerrainEntry(t.Plains, false, false);
            map[5, 2] = new TerrainEntry(t.Plains, false, true);
            map[5, 3] = new TerrainEntry(t.Plains, false, false);
            map[5, 4] = new TerrainEntry(t.Plains, true, true);
            map[5, 5] = new TerrainEntry(t.Plains, false, false);
            return map;
        }
    }
}
