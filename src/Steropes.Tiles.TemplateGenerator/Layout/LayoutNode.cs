using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout
{
  public class LayoutNode
  {
    readonly GeneratorPreferences prefs;
    readonly TextureGrid grid;

    public LayoutNode(GeneratorPreferences prefs, TextureGrid grid)
    {
      this.prefs = prefs;
      this.grid = grid ?? throw new ArgumentNullException(nameof(grid));
      this.Margin = grid.FormattingMetaData.Margin ?? 0;
      this.Size = ComputeSize();
    }

    public TextureGrid Grid
    {
      get { return grid; }
    }

    public void Draw(Graphics graphics)
    {
      Debug.WriteLine("Start Grid ({0}), Offset: {1}, Size: {2}", grid.Name, Offset, Size);

      if (!string.IsNullOrEmpty(grid.FormattingMetaData.Title))
      {
        Debug.WriteLine(" Title: {0}", grid.FormattingMetaData.Title);
        var contentSize = ComputeContentSize();
        var textSize = ComputeTextSize();
        var contentOffset = ContentOffset;
        var textOffset = new Point(contentOffset.X, contentOffset.Y + contentSize.Height + TextSpacing);

        var brush = new SolidBrush(grid.FormattingMetaData.TextColor ?? Color.Black);
        var pen = new Pen(brush);
        graphics.DrawLine(pen, textOffset, new Point(textOffset.X + contentSize.Width, textOffset.Y));
        graphics.DrawString(grid.FormattingMetaData.Title, TextFont, brush, new Rectangle(textOffset, textSize));

        pen.Dispose();
        brush.Dispose();
      }

      foreach (var tile in grid.Tiles)
      {
        DrawTile(graphics, tile);
      }

      Debug.WriteLine(" Title: {0}", grid.FormattingMetaData.Title);
      if ((grid.FormattingMetaData.Border ?? 1) > 0)
      {
        var borderPen = new Pen(grid.FormattingMetaData.BorderColor ?? Color.Gainsboro);
        graphics.DrawRectangle(borderPen, new Rectangle(Offset, Size));
        borderPen.Dispose();
      }
    }

    void DrawTile(Graphics g, TextureTile tile)
    {
      var border = grid.CellSpacing;
      var cw = grid.EffectiveCellSize;
      var x = tile.X * (cw.Width + border);
      var y = tile.Y * (cw.Height + border);

      var ap = grid.ComputeEffectiveAnchorPoint(tile);
      var g2 = g.BeginContainer();
      try
      {
        g.TranslateTransform(ContentOffset.X + x + ap.X, ContentOffset.Y + y + ap.Y);
        var painter = grid.CreateTilePainter(prefs);
        painter.Draw(g, tile);
      }
      finally
      {
        g.EndContainer(g2);
      }

    }

    Size ComputeRequiredTiles()
    {
      return MatchTypeStrategyFactory.StrategyFor(grid.MatcherType).GetTileArea(grid);
    }
    
    Size ComputeContentSize()
    {
      var tileCount = ComputeRequiredTiles();
      var maxX = tileCount.Width;
      var maxY = tileCount.Height;

      var cs = grid.EffectiveCellSize;
      var pixelWidth = cs.Width * maxX;
      var pixelHeight = cs.Height * maxY;

      var paddedWidth = Math.Max(0, maxX - 1) * grid.CellSpacing;
      var paddedHeight = Math.Max(0, maxY - 1) * grid.CellSpacing;

      return new Size(pixelWidth + paddedWidth, pixelHeight + paddedHeight);
    }

    int TextSpacing => 5;

    Size ComputeTextSize()
    {
      if (string.IsNullOrEmpty(grid.FormattingMetaData.Title))
      {
        return new Size();
      }

      Size contentSize = ComputeContentSize();
      // maximum two lines of text ..
      var result =  TextRenderer.MeasureText(grid.FormattingMetaData.Title, TextFont,
                                      new Size(contentSize.Width, int.MaxValue));
      result.Height += TextSpacing;
      return result;
    }

    Font TextFont => new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular);

    Point ContentOffset
    {
      get
      {
        var border = grid.FormattingMetaData.Border ?? 0;
        var padding = grid.FormattingMetaData.Padding ?? 0;
        return new Point(Offset.X + border + padding,
                         Offset.Y + border + padding);
      }
    }

    Size ComputeSize()
    {
      var border = grid.FormattingMetaData.Border ?? 0;
      var padding = grid.FormattingMetaData.Padding ?? 0;

      var contentSize = ComputeContentSize();
      var labelSize = ComputeTextSize();

      var s = new Size(Math.Max(contentSize.Width, labelSize.Width),
                       contentSize.Height + labelSize.Height);
      s.Width += 2 * border;
      s.Width += 2 * padding;
      s.Height += 2 * border;
      s.Height += 2 * padding;
      return s;
    }

    public Size Size { get; }

    public int Margin { get; }

    public Point Offset
    {
      get { return new Point(grid.X, grid.Y); }
      set
      {
        grid.X = value.X;
        grid.Y = value.Y;
      }
    }

    public Point BottomRight => Offset + Size;
  }
}