﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout.MatchTypes
{
  public class CardinalIndexMatchTypeStrategy : IMatchTypeStrategy
  {
    public Size RequiredTiles => new Size(2, 2);

    public Size GetTileArea(TextureGrid g)
    {
      return RequiredTiles;
    }

    public List<TextureTile> Generate(TextureGrid grid)
    {
      var ni = new Dictionary<CardinalIndex, Point>
      {
        {CardinalIndex.North, new Point(0, 0)},
        {CardinalIndex.East, new Point(1, 0)},
        {CardinalIndex.South, new Point(0, 1)},
        {CardinalIndex.West, new Point(1, 1)}
      };

      var reg = new CardinalIndexTileRegistry<string>(new ReflectorRegistry(), 
                                                      TileTagEntries.CreateCardinalIndexTagEntries(), 
                                                      grid.Pattern);
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