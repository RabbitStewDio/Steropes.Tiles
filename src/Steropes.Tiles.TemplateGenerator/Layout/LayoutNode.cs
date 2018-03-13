using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout
{
  class LayoutNode
  {
    readonly TextureGrid grid;

    public LayoutNode(TextureGrid grid)
    {
      this.grid = grid ?? throw new ArgumentNullException(nameof(grid));
      this.Margin = grid.FormattingMetaData.Margin ?? 0;
      this.Size = ComputeSize();
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
        var painter = grid.CreateTilePainter();
        painter.Draw(g, tile);
      }
      finally
      {
        g.EndContainer(g2);
      }

    }

    Point ComputeRequiredTiles()
    {
      switch (grid.MatcherType)
      {
        case MatcherType.Basic:
        {
          return CountBasicTiles();
        }
        case MatcherType.CardinalFlags:
        {
          // 16 for all directions, 
          // plus one isolated tile.
          return new Point(9, 2);
        }
        case MatcherType.CardinalIndex:
        {
          // 4, one for each direction
          return new Point(2, 2);
        }
        case MatcherType.Diagonal:
        {
          // 4, one for each direction
          return new Point(2, 2);
        }
        case MatcherType.NeighbourIndex:
        {
          // 8, one for each direction including diagonals
          return new Point(3, 3);
        }
        case MatcherType.Corner:
        {
          // 3 corner neighbour cells that either match or don't match, 
          // therefore 2^3 = 8 combinations
          // we have 4 corners, therefore 4 * 8 combinations.
          return new Point(8, 4);
        }
        case MatcherType.CellMap:
        {
          // depends on the number of choices ...
          // we assume uniform choices for all 4 neighbour slots.
          var cellMapCount = Math.Max(1, grid.EffectiveCellMapElements.Count);
          var tileCount = Math.Pow(cellMapCount, 4);
          var width = (int) Math.Ceiling(Math.Sqrt(tileCount));
          var height = (int) Math.Ceiling(tileCount / width);
          return new Point(width, height);
        }
        default:
        {
          throw new ArgumentException();
        }
      }
    }

    Point CountBasicTiles()
    {
      var maxX = 0;
      var maxY = 0;

      foreach (var textureTile in grid.Tiles)
      {
        maxX = Math.Max(Math.Max(0, textureTile.X), maxX);
        maxY = Math.Max(Math.Max(0, textureTile.Y), maxY);
      }

      maxX += 1;
      maxY += 1;
      return new Point(maxX, maxY);
    }

    Size ComputeContentSize()
    {
      var tileCount = ComputeRequiredTiles();
      var maxX = tileCount.X;
      var maxY = tileCount.Y;

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