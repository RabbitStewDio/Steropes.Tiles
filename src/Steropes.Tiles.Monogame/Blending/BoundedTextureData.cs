using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Monogame.Tiles;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Monogame.Blending
{
  public class BoundedTextureData
  {
    public BoundedTextureData(IntRect bounds, Color[] textureData)
    {
      Bounds = bounds;
      TextureData = textureData ?? throw new ArgumentNullException(nameof(textureData));
    }

    public Color[] TextureData { get; }

    public IntRect Bounds { get; }

    public Color this[int x, int y]
    {
      get
      {
        if (!Bounds.Contains(x, y))
        {
          return Color.Transparent;
        }

        var tx = x - Bounds.X;
        var ty = y - Bounds.Y;
        return TextureData[ty * Bounds.Width + tx];
      }
    }
  }

  public static class TextureOperations
  {
    public static Texture2D Apply(this Texture2D texture, BoundedTextureData result)
    {
      texture.SetData(0, result.Bounds.ToXna(), result.TextureData, 0, result.TextureData.Length);
      return texture;
    }

    public static Color[] CreateBlank(IntDimension rect)
    {
      var data = new Color[rect.Width * rect.Height];
      for (var i = 0; i < data.Length; i++)
      {
        data[i] = Color.TransparentBlack;
      }

      return data;
    }

    public static BoundedTextureData Combine(BoundedTextureData color, BoundedTextureData mask)
    {
      var colorBounds = color.Bounds;
      var width = colorBounds.Width;
      var height = colorBounds.Height;

      var retval = new Color[width * height];
      for (var y = 0; y < height; y += 1)
      {
        for (var x = 0; x < width; x += 1)
        {
          var tidx = y * width + x;

          var px = colorBounds.X + x;
          var py = colorBounds.Y + y;
          var c = color[px, py];
          var a = mask[px, py];

          retval[tidx] = c * (a.A / 255f);
        }
      }

      return new BoundedTextureData(colorBounds, retval);
    }

    public static IntRect TileAreaForCardinalDirection(IntDimension ts, CardinalIndex dir)
    {
      var w = ts.Width;
      var h = ts.Height;
      var wHalf = ts.Width / 2;
      var hHalf = ts.Height / 2;

      switch (dir)
      {
        case CardinalIndex.West:
          return new IntRect(0, 0, wHalf, hHalf);
        case CardinalIndex.North:
          return new IntRect(wHalf, 0, w - wHalf, hHalf);
        case CardinalIndex.East:
          return new IntRect(wHalf, hHalf, w - wHalf, h - hHalf);
        case CardinalIndex.South:
          return new IntRect(0, hHalf, wHalf, h - hHalf);
        default:
          throw new ArgumentException();
      }
    }

    public static BoundedTextureData ExtractData(ITexture t, IntRect rect)
    {
      var texture = t.Tex2D;
      if (texture == null)
      {
        return new BoundedTextureData(rect, CreateBlank(rect.Size));
      }

      var data = new Color[rect.Width * rect.Height];
      var b = t.Bounds.Clip(new Rectangle(rect.X + t.Bounds.X, rect.Y + t.Bounds.Y, rect.Width, rect.Height));
      texture.GetData(0, b, data, 0, data.Length);

      var textureBounds = new IntRect(rect.X, rect.Y, b.Width, b.Height);
      return new BoundedTextureData(textureBounds, data);
    }
  }
}