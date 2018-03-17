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
      DrawCellFrame(g, tile);
      
      var pen = new Pen(Grid.TextureTileFormattingMetaData.TileOutlineColor ?? Preferences.DefaultTileColor);
      
      // to be pixel perfect, the rectangle size must be reduced by one so that the
      // line is drawing within the tile area.
      g.DrawRectangle(pen, GetTileArea(tile));

      pen.Dispose();

      DrawSelectorHint(g, tile);
    }

    public Rectangle GetTileHighlightArea(TextureTile tile)
    {
      var rect = GetTileArea(tile);
      rect.X += 2;
      rect.Y += 2;
      rect.Width -= 4;
      rect.Height -= 4;
      return rect;
    }

    protected override void DrawIndexedDirection(Graphics g, TextureTile tile, NeighbourIndex idx)
    {
      var points = new List<Point>();

      var rect = GetTileHighlightArea(tile);
      
      var left = rect.Left;
      var top = rect.Top;
      var right = rect.Right;
      var bottom = rect.Bottom;

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

      var pen = new Pen(Grid.TextureTileFormattingMetaData.TileHighlightColor ?? Preferences.DefaultTileHighlightColor);

      g.DrawLines(pen, points.ToArray());

      pen.Dispose();
    }
  }
}