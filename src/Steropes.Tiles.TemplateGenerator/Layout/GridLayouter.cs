using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout
{
  public class GridLayouter
  {
    readonly GeneratorPreferences prefs;

    public GridLayouter(GeneratorPreferences prefs)
    {
      this.prefs = prefs ?? throw new ArgumentNullException(nameof(prefs));
    }

    public bool ArrangeGrids(TextureCollection c)
    {
      var grids = c.Grids.Select(g => new LayoutNode(prefs, g)).OrderBy(e => -e.Size.Width).ToList();
      foreach (var node in grids)
      {
        Console.WriteLine($"Layout:{node.Grid.Name} Weight:{node.Size.Width * node.Size.Height} Size:{node.Size}");
      }

      // All grids sorted by largest space consumed.
      var root = new ArrangeNode<LayoutNode>(2048, 2048);
      var success = true;
      foreach (var grid in grids)
      {
        if (!root.Insert(grid.Size, grid))
        {
          success = false;
        }

        root.Print();
      }

      root.Apply(n => n.Content.Offset = new Point(n.X, n.Y));
      return success;
    }
  }

  public static class ArrangeNode
  {
    public static Tuple<ArrangeNode<T>, ArrangeNode<T>> Split<T>(this ArrangeNode<T> n, Size size)
    {
      var hspace = n.SpaceAvailableX - size.Width;
      var vspace = n.SpaceAvailableY - size.Height;
      
      // estimate the available area 
      //var estH = hspace * n.SpaceAvailableY;
      //var estV = vspace * n.SpaceAvailableX;
      var estH = n.X + size.Width;
      var estV = n.Y + size.Height;
      if (estH >= estV)
      {
        Console.WriteLine($"Splitting vert for {estH} , {estV}");
        // vertical split 
        //
        //   content  |  Left
        //   ----------------
        //   Bottom
        //
        var left = new ArrangeNode<T>(n.X + size.Width, n.Y, hspace, size.Height);
        var bottom = new ArrangeNode<T>(n.X, n.Y + size.Height, n.SpaceAvailableX, vspace);
        return Tuple.Create(left, bottom);
      }
      else
      {
        Console.WriteLine($"Splitting horizontal for {estH} , {estV}");
        // horizontal split
        //
        //   content  |  Left
        //   ---------|
        //   Bottom   | 
        //
        var left = new ArrangeNode<T>(n.X + size.Width, n.Y, hspace, n.SpaceAvailableY);
        var bottom = new ArrangeNode<T>(n.X, n.Y + size.Height, size.Width, vspace);
        return Tuple.Create(left, bottom);
      }
    }

  }

  public class ArrangeNode<T>
  {
    ArrangeNode<T> bottom;
    ArrangeNode<T> left;
    public T Content { get; private set; }

    public int X { get; private set; }
    public int Y { get; private set; }
    int width;
    int height;
    public int SpaceAvailableX { get; }
    public int SpaceAvailableY { get; }
    int childCount;

    public ArrangeNode(int width, int height)
    {
      this.SpaceAvailableX = width;
      this.SpaceAvailableY = height;
    }

    public ArrangeNode(int x, int y, int width, int height)
    {
      X = x;
      Y = y;
      this.SpaceAvailableX = width;
      this.SpaceAvailableY = height;
    }

    public override string ToString()
    {
      return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}, {nameof(SpaceAvailableX)}: {SpaceAvailableX}, {nameof(SpaceAvailableY)}: {SpaceAvailableY}, {nameof(width)}: {width}, {nameof(height)}: {height} : {Content}" ;
    }

    public void Apply(Action<ArrangeNode<T>> a)
    {
      if (Content == null)
      {
        return;
      }

      a(this);
      left.Apply(a);
      bottom.Apply(a);
    }

    public ArrangeNode<T> FindInsertPosition(Size size)
    {
      if (Content == null)
      {
        if (Fits(size))
        {
          Console.WriteLine("Choosing <empty node>");
          return this;
        }

        Console.WriteLine("Rejecting <empty node>");
        return null;
      }

      Console.WriteLine("Choosing .. " + Content);

      var leftPos = left.FindInsertPosition(size);
      var bottomPos = bottom.FindInsertPosition(size);
      if (leftPos == null)
      {
        Console.WriteLine("Choosing bottom by default for " + Content);
        return bottomPos;
      }

      if (bottomPos == null)
      {
        Console.WriteLine("Choosing left by default for " + Content);
        return leftPos;
      }

      var weightLeft = leftPos.ComputeWeight(size);
      var weightBottom = bottomPos.ComputeWeight(size);

      Console.WriteLine(weightLeft + "  == " +leftPos);
      Console.WriteLine(weightBottom + "  == " +bottomPos);

      if (weightLeft <= weightBottom)
      {
        Console.WriteLine("Choosing left for " + Content);
        return leftPos;
      }

      Console.WriteLine("Choosing bottom for " + Content);
      return bottomPos;
    }

    double ComputeWeight(Size s)
    {
      return Math.Pow(X + s.Width, 2) + Math.Pow(Y + s.Height, 2);
    }

    void ApplyInsert(Size size, T c)
    {
      if (c == null)
      {
        throw new ArgumentNullException();
      }

      if (Content != null)
      {
        throw new InvalidOperationException();
      }

      if (!Fits(size))
      {
        throw new InvalidOperationException();
      }

      Content = c;
      width = size.Width;
      height = size.Height;
      childCount += 1;
      var s = this.Split(size);
      left = s.Item1;
      bottom = s.Item2;
    }

    public bool Insert(Size size, T c)
    {
      Console.WriteLine("Insert " + c);

      try
      {
        var insertPos = FindInsertPosition(size);
        if (insertPos != null)
        {
          insertPos.ApplyInsert(size, c);
          return true;
        }

        return false;
      }
      finally
      {
        Console.WriteLine("Done Insert " + c);
      }
    }


    bool Fits(Size c)
    {
      if (c.Height > SpaceAvailableY)
      {
        return false;
      }

      if (c.Width > SpaceAvailableX)
      {
        return false;
      }

      return true;
    }

    public void Print(int indent = 0)
    {
      Console.WriteLine("".PadLeft(indent) + this);
      if (left != null)
      {
        Console.WriteLine("".PadLeft(indent) + "Left:");
        left.Print(indent + 5);
      }

      if (bottom != null)
      {
        Console.WriteLine("".PadLeft(indent) + "Bottom:");
        bottom.Print(indent + 5);
      }
    }

  }
}