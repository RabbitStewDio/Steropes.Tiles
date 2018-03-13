using System.Drawing;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout
{
  public class IsoTilePainter : TilePainterBase
  {

    public IsoTilePainter(TextureGrid grid): base(grid)
    {
    }

    public override void Draw(Graphics g, TextureTile tile)
    {
      DrawCell(g);

      var pen = new Pen(Grid.FormattingMetaData.BorderColor ?? Color.Green);

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
    }
  }
}