using System.Drawing;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout
{
  public class GridTilePainter : TilePainterBase
  {
    public GridTilePainter(TextureGrid grid): base(grid)
    {
    }

    public override void Draw(Graphics g, TextureTile tile)
    {
      DrawCell(g);
      
      var pen = new Pen(Grid.FormattingMetaData.BorderColor ?? Color.Green);

      var tileSize = Grid.EffectiveTileSize;
      var width2 = tileSize.Width / 2;
      var height2 = tileSize.Height / 2;
      
      g.DrawRectangle(pen, -width2, -height2, tileSize.Width, tileSize.Height);
      
      pen.Dispose();
    }
  }
}