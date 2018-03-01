using System;
using Microsoft.Xna.Framework;

namespace Steropes.Tiles.Monogame.Tiles
{
  public class TexturedTile : ITexturedTile
  {
    public ITexture Texture { get; }
    public string Tag { get; }
    public Vector2 Anchor { get; }

    public TexturedTile(string tag, ITexture texture = null, Vector2 anchor = new Vector2())
    {
      Anchor = anchor;
      Tag = tag ?? throw new ArgumentNullException(nameof(tag));
      Texture = texture;
    }

    public static ITexturedTile None = new TexturedTile("*", null, Vector2.Zero);
  }
}