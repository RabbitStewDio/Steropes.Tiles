using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Steropes.Tiles.Monogame.Tiles
{
  public class TileTexture : ITexture
  {
    public string Name { get; }
    public Texture2D Tex2D { get; }
    public Rectangle Bounds { get; }

    public TileTexture(string name, Texture2D tex2D, Rectangle bounds)
    {
      Name = name ?? throw new ArgumentNullException(nameof(name));
      Tex2D = tex2D ?? throw new ArgumentNullException(nameof(tex2D));
      Bounds = bounds;
    }
  }
}