using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout
{
  public class GridProducer
  {
    public Bitmap Produce(TextureCollection c)
    {
      var grids = c.Grids.Select(g => new LayoutNode(g)).ToList();
      var width = 0;
      var height = 0;
      foreach (var grid in grids)
      {
        var p = grid.Offset + grid.Size;
        width = Math.Max(p.X, width);
        height = Math.Max(p.Y, height);
      }

      Bitmap b = new Bitmap(width + 1, height + 1, PixelFormat.Format32bppArgb);
      var graphics = Graphics.FromImage(b);
      foreach (var node in grids)
      {
        node.Draw(graphics);
      }
      graphics.Dispose();
      return b;
    }
  }

  public class GridLayouter
  {
    public bool ArrangeGrids(TextureCollection c)
    {
      var grids = c.Grids.Select(g => new LayoutNode(g)).OrderBy(e => e.Size.Width * e.Size.Height).ToList();
      // All grids sorted by largest space consumed.
      var root = new ArrangeNode(2048, 2048);
      var success = true;
      foreach (var grid in grids)
      {
        if (root.Insert(grid))
        {
          success = false;
        }
      }

      root.Apply();
      return success;
    }

    class ArrangeNode
    {
      ArrangeNode bottom;
      LayoutNode content;

      int height;
      ArrangeNode left;
      int width;
      int x;
      int y;

      public ArrangeNode(int width = 0, int height = 0)
      {
        this.width = width;
        this.height = height;
      }

      Rectangle CellBounds { get; set; }

      public void Apply()
      {
        if (content == null)
        {
          return;
        }

        content.Offset = new Point(x, y);
        left.Apply();
        bottom.Apply();
      }

      public bool Insert(LayoutNode c)
      {
        if (c == null)
        {
          throw new ArgumentNullException();
        }

        if (content == null)
        {
          if (Fits(c))
          {
            content = c;
            Split(c.Size);
          }
          else
          {
            // content is not suitable, so none of the child nodes
            // would be able to handle it either.
            return false;
          }
        }

        if (left.Insert(c))
        {
          return true;
        }

        if (bottom.Insert(c))
        {
          return true;
        }

        return false;
      }

      void Split(Size contentSize)
      {
        if (CellBounds.Width - contentSize.Width > CellBounds.Height - contentSize.Height)
        {
          // vertical split 
          left = new ArrangeNode
          {
            x = x + contentSize.Width,
            y = y,
            width = width - contentSize.Width,
            height = contentSize.Height,
          };

          bottom = new ArrangeNode
          {
            x = x,
            y = y + contentSize.Height,
            width = width,
            height = height - contentSize.Height,
          };
        }
        else
        {
          // horizontal split
          left = new ArrangeNode
          {
            x = x + contentSize.Width,
            y = y,
            width = width - contentSize.Width,
            height = height,
          };

          bottom = new ArrangeNode
          {
            x = x,
            y = y + contentSize.Height,
            width = contentSize.Width,
            height = height - contentSize.Height,
          };
        }
      }

      bool Fits(LayoutNode c)
      {
        if (c.Size.Height > height)
        {
          return false;
        }

        if (c.Size.Width > width)
        {
          return false;
        }

        return true;
      }
    }
  }
}