using System;
using System.Drawing;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout
{
  public abstract class TilePainterBase: ITilePainter
  {
    public TextureGrid Grid { get; }

    protected TilePainterBase(TextureGrid grid)
    {
      this.Grid = grid ?? throw new ArgumentNullException(nameof(grid));
    }

    protected void DrawCell(Graphics g)
    {
      var bg = Grid.FormattingMetaData.BackgroundColor;
      if (bg != null)
      {
        var size = Grid.EffectiveCellSize;
        var pen = new Pen(bg.Value);
        g.DrawRectangle(pen, new Rectangle(size.Width / 2, size.Height / 2, size.Width, size.Height));
        pen.Dispose();
      }
    }

    public abstract void Draw(Graphics g, TextureTile tile);
  }
}