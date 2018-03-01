using System.Collections.Generic;

namespace Steropes.Tiles.Monogame.Tiles
{
  public interface ITextureFile
  {
    IEnumerable<ITexturedTile> ProduceTextures(IContentLoader loader);
  }
}