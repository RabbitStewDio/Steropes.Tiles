using Microsoft.Xna.Framework;

namespace Steropes.Tiles.Monogame.Tiles
{
  /// <summary>
  /// A representation of a tile rendering operation. 
  /// <para>
  /// There is one instance for each tagged tile, possibly sharing underlying Texture2D objects.
  /// </para>
  /// </summary>
  public interface ITexturedTile
  {
    ITexture Texture { get; }
    string Tag { get; }
    Vector2 Anchor { get; }
  }
}