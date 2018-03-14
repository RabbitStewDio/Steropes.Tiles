using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout.MatchTypes
{
  public interface IMatchTypeStrategy
  {
    Size GetTileArea(TextureGrid g);

    List<TextureTile> Generate(TextureGrid grid);
  }

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

      return ni.Select(name =>
      {
        var tileName = reg.Find(grid.Name, name.Key);
        return new TextureTile(true, name.Value.X, name.Value.Y, tileName)
        {
          SelectorHint = name.Key.ToString()
        };
      }).ToList();
    }
  }
}