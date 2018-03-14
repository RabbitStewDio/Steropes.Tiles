using System;
using System.Collections.Generic;
using System.Drawing;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout
{
  public class GridTilePainter : TilePainterBase
  {
    public GridTilePainter(GeneratorPreferences preferences, TextureGrid grid): base(preferences, grid)
    {
    }

    public override void Draw(Graphics g, TextureTile tile)
    {
      DrawCellFrame(g);
      
      var pen = new Pen(Grid.FormattingMetaData.BorderColor ?? Preferences.DefaultTileColor);

      var tileSize = Grid.EffectiveTileSize;
      var width2 = tileSize.Width / 2;
      var height2 = tileSize.Height / 2;
      
      // to be pixel perfect, the rectangle size must be reduced by one so that the
      // line is drawing within the tile area.
      g.DrawRectangle(pen, -width2, -height2, tileSize.Width - 1, tileSize.Height - 1);

      pen.Dispose();

      DrawSelectorHint(g, tile);
    }

    protected override void DrawIndexedDirection(Graphics g,NeighbourIndex idx)
    {
      var points = new List<Point>();

      var tileSize = Grid.EffectiveTileSize;
      var left = -tileSize.Width / 2 + 2;
      var top = -tileSize.Height / 2 + 2;
      var right = tileSize.Width / 2 - 3;
      var bottom = tileSize.Height / 2 - 3;

      switch (idx)
      {
        case NeighbourIndex.North:
          points.Add(new Point(left, top));
          points.Add(new Point(right, top));
          break;
        case NeighbourIndex.NorthEast:
          points.Add(new Point(0, top));
          points.Add(new Point(right, top));
          points.Add(new Point(right, 0));
          break;
        case NeighbourIndex.East:
          points.Add(new Point(right, top));
          points.Add(new Point(right, bottom));
          break;
        case NeighbourIndex.SouthEast:
          points.Add(new Point(right, 0));
          points.Add(new Point(right, bottom));
          points.Add(new Point(0, bottom));
          break;
        case NeighbourIndex.South:
          points.Add(new Point(left, bottom));
          points.Add(new Point(right, bottom));
          break;
        case NeighbourIndex.SouthWest:
          points.Add(new Point(0, bottom));
          points.Add(new Point(left, bottom));
          points.Add(new Point(left, 0));
          break;
        case NeighbourIndex.West:
          points.Add(new Point(left, top));
          points.Add(new Point(left, bottom));
          break;
        case NeighbourIndex.NorthWest:
          points.Add(new Point(left, 0));
          points.Add(new Point(left, top));
          points.Add(new Point(0, top));
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(idx), idx, null);
      }

      var pen = new Pen(Grid.FormattingMetaData.TextColor ?? Preferences.DefaultTileHighlightColor);

      for (int pidx = 1; pidx < points.Count; pidx += 1)
      {
        var p1 = points[pidx - 1];
        var p2 = points[pidx];
        g.DrawLine(pen, p1, p2);
      }

      pen.Dispose();
    }
  }
}