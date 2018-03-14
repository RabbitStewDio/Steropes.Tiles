using System.Collections.Generic;
using System.Drawing;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout.MatchTypes
{
  public class DiagonalFlagMatchTypeStrategy : IMatchTypeStrategy
  {
    public DiagonalFlagMatchTypeStrategy()
    {
    }

    public Size GetTileArea(TextureGrid g)
    {
      return RequiredTiles;
    }
    public Size RequiredTiles => new Size(4, 4);

    public List<TextureTile> Generate(TextureGrid grid)
    {
      var keys = DiagonalTileSelectionKey.Values;
      var reg = new DiagonalTileRegistry<string>(new ReflectorRegistry(), null, grid.Pattern);
      var retval = new List<TextureTile>();
      for (var index = 0; index < keys.Length; index++)
      {
        var key = keys[index];
        var tileName = reg.Find(grid.Name, key);
        var x = index % 4;
        var y = index / 4;
        retval.Add(new TextureTile(true, x, y, tileName)
        {
          SelectorHint = key.ToString()
        });
      }

      return retval;
    }
  }
}