using System.Collections.Generic;
using System.Linq;

namespace Steropes.Tiles.TexturePack.Operations
{
    public interface ITextureAtlasBuilder<TTexture>
    {
        IEnumerable<TTexture> GetTextures();
        TTexture Add(TTexture texture);
    }

    public class NoOpTextureAtlasBuilder<TTexture> : ITextureAtlasBuilder<TTexture>
    {
        public IEnumerable<TTexture> GetTextures()
        {
            return Enumerable.Empty<TTexture>();
        }

        public TTexture Add(TTexture texture)
        {
            return texture;
        }
    }
}
