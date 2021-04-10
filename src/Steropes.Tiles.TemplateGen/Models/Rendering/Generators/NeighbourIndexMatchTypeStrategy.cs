using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Registry;
using System.Collections.Generic;

namespace Steropes.Tiles.TemplateGen.Models.Rendering.Generators
{
    public class NeighbourIndexMatchTypeStrategy : IMatchTypeStrategy
    {
        public IntDimension RequiredTiles => new IntDimension(3, 3);

        public IntDimension GetTileArea(TextureGrid g)
        {
            return RequiredTiles;
        }

        public List<TextureTile> Generate(TextureGrid grid)
        {
            var ni = new Dictionary<NeighbourMatchPosition, IntPoint>();
            if (grid.Parent?.Parent?.TileType == TileType.Isometric)
            {
                ni.Add(NeighbourMatchPosition.NorthWest, new IntPoint(1, 0));
                ni.Add(NeighbourMatchPosition.North, new IntPoint(2, 0));
                ni.Add(NeighbourMatchPosition.NorthEast, new IntPoint(2, 1));
                ni.Add(NeighbourMatchPosition.East, new IntPoint(2, 2));
                ni.Add(NeighbourMatchPosition.SouthEast, new IntPoint(1, 2));
                ni.Add(NeighbourMatchPosition.South, new IntPoint(0, 2));
                ni.Add(NeighbourMatchPosition.SouthWest, new IntPoint(0, 1));
                ni.Add(NeighbourMatchPosition.West, new IntPoint(0, 0));
                ni.Add(NeighbourMatchPosition.Isolated, new IntPoint(1, 1));
            }
            else
            {
                ni.Add(NeighbourMatchPosition.NorthWest, new IntPoint(0, 0));
                ni.Add(NeighbourMatchPosition.North, new IntPoint(1, 0));
                ni.Add(NeighbourMatchPosition.NorthEast, new IntPoint(2, 0));
                ni.Add(NeighbourMatchPosition.West, new IntPoint(0, 1));
                ni.Add(NeighbourMatchPosition.Isolated, new IntPoint(1, 1));
                ni.Add(NeighbourMatchPosition.East, new IntPoint(2, 1));
                ni.Add(NeighbourMatchPosition.SouthWest, new IntPoint(0, 2));
                ni.Add(NeighbourMatchPosition.South, new IntPoint(1, 2));
                ni.Add(NeighbourMatchPosition.SouthEast, new IntPoint(2, 2));
            }

            var reg = new NeighbourIndexTileRegistry<string>(new ReflectorRegistry(), NeighbourIndexTileRegistry<string>.LongSelector(), grid.Pattern);

            var retval = new List<TextureTile>();
            foreach (var name in ni)
            {
                if (!reg.TryFind(grid.Name, name.Key, out var tileName))
                {
                    continue;
                }

                retval.Add(new TextureTile(true, name.Value.X, name.Value.Y, tileName)
                {
                    SelectorHint = name.Key.ToString()
                });
            }

            return retval;
        }
    }
}
