using System.Collections.Generic;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Registry;

namespace Steropes.Tiles.Monogame.Tiles
{
  /// <summary>
  ///  A texture pack contains the sum of all textures for a given game.
  /// The textures are provided as one or more texture files.
  /// </summary>
  public interface ITexturePack
  {
    List<ITextureFile> TextureFiles { get; }
    IntDimension TileSize { get; }
    TextureType TextureType { get; }
    string BasePath { get; }
  }

  public static class TexturePackExtensions
  {
    public static ITileRegistry<ITexturedTile> LoadTexturePack(this ITexturePack pack, IContentLoader loader)
    {
      var registry = new BasicTileRegistry<ITexturedTile>(TexturedTile.None);
      foreach (var tf in pack.TextureFiles)
      {
        foreach (var x in tf.ProduceTextures(loader))
        {
          registry.Add(x.Tag, x);
        }
      }
      return registry;
    }

  }
}