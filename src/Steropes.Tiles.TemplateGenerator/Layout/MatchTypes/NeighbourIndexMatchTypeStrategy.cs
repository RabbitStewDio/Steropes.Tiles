using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.TemplateGenerator.Model;
using System.Collections.Generic;
using System.Drawing;

namespace Steropes.Tiles.TemplateGenerator.Layout.MatchTypes
{
    public class NeighbourIndexMatchTypeStrategy : IMatchTypeStrategy
    {
        public Size RequiredTiles => new Size(3, 3);

        public Size GetTileArea(TextureGrid g)
        {
            return RequiredTiles;
        }

        public List<TextureTile> Generate(TextureGrid grid)
        {
            var ni = new Dictionary<NeighbourMatchPosition, Point>();
            if (grid.Parent?.Parent?.TileType == TileType.Isometric)
            {
                ni.Add(NeighbourMatchPosition.NorthWest, new Point(1, 0));
                ni.Add(NeighbourMatchPosition.North, new Point(2, 0));
                ni.Add(NeighbourMatchPosition.NorthEast, new Point(2, 1));
                ni.Add(NeighbourMatchPosition.East, new Point(2, 2));
                ni.Add(NeighbourMatchPosition.SouthEast, new Point(1, 2));
                ni.Add(NeighbourMatchPosition.South, new Point(0, 2));
                ni.Add(NeighbourMatchPosition.SouthWest, new Point(0, 1));
                ni.Add(NeighbourMatchPosition.West, new Point(0, 0));
                ni.Add(NeighbourMatchPosition.Isolated, new Point(1, 1));
            }
            else
            {
                ni.Add(NeighbourMatchPosition.NorthWest, new Point(0, 0));
                ni.Add(NeighbourMatchPosition.North, new Point(1, 0));
                ni.Add(NeighbourMatchPosition.NorthEast, new Point(2, 0));
                ni.Add(NeighbourMatchPosition.West, new Point(0, 1));
                ni.Add(NeighbourMatchPosition.Isolated, new Point(1, 1));
                ni.Add(NeighbourMatchPosition.East, new Point(2, 1));
                ni.Add(NeighbourMatchPosition.SouthWest, new Point(0, 2));
                ni.Add(NeighbourMatchPosition.South, new Point(1, 2));
                ni.Add(NeighbourMatchPosition.SouthEast, new Point(2, 2));
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
