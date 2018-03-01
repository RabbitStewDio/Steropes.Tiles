using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Steropes.Tiles.Monogame.Tiles
{
  public interface ITexture
  {
    string Name { get; }
    Texture2D Tex2D { get; }
    Rectangle Bounds { get; }
  }
}