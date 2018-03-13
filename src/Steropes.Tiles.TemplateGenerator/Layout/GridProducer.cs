﻿using System;
using System.Diagnostics;
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

      Debug.WriteLine("Producing new preview ({0},{1}) with {2} elements.", width, height, grids.Count);
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
}