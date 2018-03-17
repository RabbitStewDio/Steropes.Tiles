using System.Collections.Generic;
using System.Drawing;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout
{
  public class IsoTilePainter : TilePainterBase
  {
    public IsoTilePainter(GeneratorPreferences prefs, TextureGrid grid) : base(prefs, grid)
    {
    }

    public override void Draw(Graphics g, TextureTile tile)
    {
      DrawCellFrame(g, tile);

      var rect = GetTileArea(tile);
      using (var pen = new Pen(Grid.TextureTileFormattingMetaData.TileOutlineColor ?? Preferences.DefaultTileColor))
      {
        var shape = CreateShape(rect);
        shape.Draw(g, pen);
      }

      DrawHeightIndicator(g, tile);
      DrawSelectorHint(g, tile);
    }

    void DrawHeightIndicator(Graphics g, TextureTile tile)
    {
      var rect = GetTileArea(tile);
      var extraSpace = rect.Y;
      if (extraSpace == 0)
      {
        return;
      }

      var baseShape = CreateShape(rect);

      using (var pen = new Pen(Grid.TextureTileFormattingMetaData.TileOutlineColor ?? Preferences.DefaultTileColor))
      {
        var shape = CreateShape(new Rectangle(rect.X, rect.Y + extraSpace, rect.Width, rect.Height));
        shape.Draw(g, pen);

        g.DrawLine(pen, baseShape.Left, shape.Left);
        g.DrawLine(pen, baseShape.Right, shape.Right);
        g.DrawLine(pen, baseShape.Top, shape.Top);
        g.DrawLine(pen, baseShape.Bottom, shape.Bottom);
      }

    }

    static bool IsStandardIsoTile(Rectangle rect)
    {
      if ((rect.Width % 8) != 0)
      {
        return false;
      }

      return (rect.Width == rect.Height * 2);
    }

    public static IShape CreateShape(Rectangle rect)
    {
      // drawing in WinForms is odd. To fill the correct pixels in the 
      // bottom and right edge we have to correct the line drawing by
      // one pixel.
      var centerX = (rect.Left + rect.Right) / 2;
      var centerY = (rect.Top + rect.Bottom) / 2;

      var left = new Point(rect.Left, centerY);
      var right = new Point(rect.Right - 1, centerY);
      var top = new Point(centerX, rect.Top);
      var bottom = new Point(centerX, rect.Bottom - 1);
      if (!IsStandardIsoTile(rect))
      {
        return new IsoShape(top, left, bottom, right);
      }
      else
      {
        // These drawing instructions create a valid pixel shape 
        // for isometric tiles that fit together correctly. 
        //
        // These work only without rendering artefacts (like displaced
        // pixels) if the tile width is a number divisible by 8 and 
        // has a height half the defined width.
        return new PixelPerfectIsoShape(top, left, bottom, right);
      }
    }
    
    protected override void DrawIndexedDirection(Graphics g, TextureTile tile, NeighbourIndex idx)
    {
      var points = CreateShape(GetTileArea(tile)).ToHighlight().GetHighlightFor(idx);
      var pen = new Pen(Grid.TextureTileFormattingMetaData.TileHighlightColor ?? Preferences.DefaultTileHighlightColor);

      for (var pidx = 1; pidx < points.Count; pidx += 1)
      {
        var p1 = points[pidx - 1];
        var p2 = points[pidx];
        g.DrawLine(pen, p1, p2);
      }

      pen.Dispose();
    }
  }
}