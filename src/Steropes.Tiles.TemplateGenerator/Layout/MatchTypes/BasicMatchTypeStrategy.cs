using System;
using System.Collections.Generic;
using System.Drawing;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout.MatchTypes
{
  public class BasicMatchTypeStrategy: IMatchTypeStrategy
  {
    public Size GetTileArea(TextureGrid grid)
    {
      var maxX = 0;
      var maxY = 0;

      foreach (var textureTile in grid.Tiles)
      {
        maxX = Math.Max(Math.Max(0, textureTile.X), maxX);
        maxY = Math.Max(Math.Max(0, textureTile.Y), maxY);
      }

      maxX += 1;
      maxY += 1;
      return new Size(maxX, maxY);

    }

    public List<TextureTile> Generate(TextureGrid grid)
    {
      return new List<TextureTile>();
    }
  }
}