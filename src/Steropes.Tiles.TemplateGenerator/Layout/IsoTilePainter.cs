using System;
using System.Collections.Generic;
using System.Drawing;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout
{
  public class IsoTilePainter : TilePainterBase
  {

    public IsoTilePainter(GeneratorPreferences prefs, TextureGrid grid): base(prefs, grid)
    {
    }

    public override void Draw(Graphics g, TextureTile tile)
    {
      DrawCellFrame(g);

      var pen = new Pen(Grid.FormattingMetaData.BorderColor ?? Preferences.DefaultTileColor);

      var tileSize = Grid.EffectiveTileSize;
      var width2 = tileSize.Width / 2;
      var height2 = tileSize.Height / 2;

      var left = new Point(-width2, 0);
      var right = new Point(width2, 0);
      var top = new Point(0, -height2);
      var bottom = new Point(0, height2);

      g.DrawLine(pen, left, top);
      g.DrawLine(pen, left, bottom);
      g.DrawLine(pen, right, top);
      g.DrawLine(pen, right, bottom);

      // Draw offset secondary tile and side connectors 
      var ap = Grid.ComputeEffectiveAnchorPoint(tile);
      var extraSpace = height2 - ap.Y;
      if (extraSpace != 0)
      {
        var left2 = new Point(-width2, extraSpace);
        var right2 = new Point(width2, extraSpace);
        var top2 = new Point(0, extraSpace - height2);
        var bottom2 = new Point(0, extraSpace + height2);

        g.DrawLine(pen, left2, top2);
        g.DrawLine(pen, left2, bottom2);
        g.DrawLine(pen, right2, top2);
        g.DrawLine(pen, right2, bottom2);

        g.DrawLine(pen, left, left2);
        g.DrawLine(pen, bottom, bottom2);
        g.DrawLine(pen, right, right2);
        g.DrawLine(pen, top, top2);
      }

      pen.Dispose();

      DrawSelectorHint(g, tile);
    }

    
    protected override void DrawIndexedDirection(Graphics g, NeighbourIndex idx)
    {
      var points = new List<Point>();

      var tileSize = Grid.EffectiveTileSize;
      var width2 = tileSize.Width / 2 - 2;
      var height2 = tileSize.Height / 2 - 2;

      switch (idx)
      {
        case NeighbourIndex.North:
          points.Add(new Point(0, -height2));
          points.Add(new Point(width2, 0));
          break;
        case NeighbourIndex.NorthEast:
          points.Add(new Point(width2 / 2, -height2 / 2));
          points.Add(new Point(width2, 0));
          points.Add(new Point(width2 / 2, +height2 / 2));
          break;
        case NeighbourIndex.East:
          points.Add(new Point(width2, 0));
          points.Add(new Point(0, height2));
          break;
        case NeighbourIndex.SouthEast:
          points.Add(new Point(width2 / 2, +height2 / 2));
          points.Add(new Point(0, height2));
          points.Add(new Point(-width2 / 2, +height2 / 2));
          break;
        case NeighbourIndex.South:
          points.Add(new Point(0, height2));
          points.Add(new Point(-width2, 0));
          break;
        case NeighbourIndex.SouthWest:
          points.Add(new Point(-width2 / 2, +height2 / 2));
          points.Add(new Point(-width2, 0));
          points.Add(new Point(-width2 / 2, -height2 / 2));
          break;
        case NeighbourIndex.West:
          points.Add(new Point(-width2, 0));
          points.Add(new Point(0, -height2));
          break;
        case NeighbourIndex.NorthWest:
          points.Add(new Point(-width2 / 2, -height2 / 2));
          points.Add(new Point(0, -height2));
          points.Add(new Point(width2 / 2, -height2 / 2));
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