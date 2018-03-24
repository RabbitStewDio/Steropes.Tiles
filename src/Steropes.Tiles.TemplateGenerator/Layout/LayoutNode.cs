using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
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
      this.ContentSize = ComputeContentSize();
      this.TextAreaSize = ComputeTextSize(ContentSize);
      this.Size = ComputeGridSize(TextAreaSize, ContentSize);
    }

    public Size ContentSize { get; }

    public TextureGrid Grid
    {
      get { return grid; }
    }

    Size ComputeContentSize()
    {
      var tileCount = MatchTypeStrategyFactory.StrategyFor(grid.MatcherType).GetTileArea(grid);
      var maxX = tileCount.Width;
      var maxY = tileCount.Height;

      var cs = grid.EffectiveCellSize;
      var pixelWidth = cs.Width * maxX;
      var pixelHeight = cs.Height * maxY;

      var paddedWidth = Math.Max(0, maxX - 1) * grid.CellSpacing;
      var paddedHeight = Math.Max(0, maxY - 1) * grid.CellSpacing;

      return new Size(pixelWidth + paddedWidth, pixelHeight + paddedHeight);
    }

    public int TextSpacing => 5;

    Size ComputeTextSize(Size contentSize)
    {
      var title = grid.FormattingMetaData.Title;
      if (string.IsNullOrEmpty(title))
      {
        return new Size();
      }

      using (var b = new Bitmap(1, 1, PixelFormat.Format32bppArgb))
      using (var g = Graphics.FromImage(b))
      {
        var rval = g.MeasureString(title,
                                   prefs.DefaultFont,
                                   new Size(contentSize.Width, int.MaxValue));
        return new Size((int) Math.Ceiling(rval.Width), (int) Math.Ceiling(rval.Height));
      }
    }

    public Size TextAreaSize { get; }

    Size ComputeGridSize(Size labelSize, Size contentSize)
    {
      var textSizeWithSpacing = labelSize.Height;
      if (textSizeWithSpacing > 0)
      {
        textSizeWithSpacing += prefs.TextSpacing;
      }

      var s = new Size(Math.Max(contentSize.Width, labelSize.Width),
                       contentSize.Height + textSizeWithSpacing);

      var border = grid.FormattingMetaData.Border ?? 0;
      var padding = grid.FormattingMetaData.Padding ?? 0;
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