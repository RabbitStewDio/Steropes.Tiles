using System.Collections;
using System.Collections.Generic;

namespace Steropes.Tiles.Demo.Game
{
    public class TerrainTypes: IEnumerable<TerrainType>
    {
        public TerrainType Desert { get; }
        public TerrainType Plains { get; }
        public TerrainType Hills { get; }
        public TerrainType Grasland { get; }

        public TerrainTypes()
        {
            Desert = new TerrainType("Desert", 1, 0, 1, 0);
            Plains = new TerrainType("Plains", 1, 1, 0, 1);
            Grasland = new TerrainType("Grasland", 1, 2, 1, 0);
            Hills = new TerrainType("Hills", 2, 0, 0, 2);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TerrainType> GetEnumerator()
        {
            return new List<TerrainType> {Desert, Plains, Hills, Grasland}.GetEnumerator();
        }
    }
}