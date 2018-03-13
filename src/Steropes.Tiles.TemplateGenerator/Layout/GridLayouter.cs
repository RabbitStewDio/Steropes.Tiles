using System;
using System.Drawing;
using System.Linq;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout
{
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
      ArrangeNode left;
      LayoutNode content;

      int x;
      int y;
      int width;
      int height;

      public ArrangeNode(int width = 0, int height = 0)
      {
        this.width = width;
        this.height = height;
      }

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
        if (width - contentSize.Width > height - contentSize.Height)
        {
          // vertical split 
          left = new ArrangeNode
          {
            x = x + contentSize.Width,
            y = y,
            width = width - contentSize.Width,
            height = contentSize.Height
          };

          bottom = new ArrangeNode
          {
            x = x,
            y = y + contentSize.Height,
            width = width,
            height = height - contentSize.Height
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
            height = height
          };

          bottom = new ArrangeNode
          {
            x = x,
            y = y + contentSize.Height,
            width = contentSize.Width,
            height = height - contentSize.Height
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