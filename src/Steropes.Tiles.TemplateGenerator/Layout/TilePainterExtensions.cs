using System;
using System.Drawing;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout
{
  public static class TilePainterExtensions
  {
    public static ITilePainter CreateTilePainter(this TextureGrid grid)
    {
      var t = grid.Parent?.Parent?.TileType ?? TileType.Grid;
      switch (t)
      {
        case TileType.Grid:
          return new GridTilePainter(grid);
        case TileType.Isometric:
          return new IsoTilePainter(grid);
        case TileType.Hex:
          return new GridTilePainter(grid); // todo
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public static Size ComputeTileDimension(this TextureGrid grid)
    {
      var textureFile = grid.Parent?.Parent;
      if (textureFile == null)
      {
        throw new ArgumentException("TextureGrid is not associated with a texture file.");
      }

      var w = textureFile.Width;
      var h = textureFile.Height;
      if (grid.MatcherType == MatcherType.Corner)
      {
        return new Size(w / 2, h / 2);
      }

      return new Size(w, h);
    }
  }
}