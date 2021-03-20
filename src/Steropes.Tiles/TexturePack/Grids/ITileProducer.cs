using System.Collections.Generic;
using Steropes.Tiles.DataStructures;

namespace Steropes.Tiles.TexturePack.Grids
{
    public interface ITileProducer<TTile, TTexture, TRawTexture>
        where TTile : ITexturedTile<TTexture>
        where TTexture : ITexture
    {
        IEnumerable<TTile> ProduceAll(IntDimension tileSize,
                                      IntRect gridBounds,
                                      IntPoint anchor,
                                      IEnumerable<string> tags,
                                      TRawTexture texture);

        TTile Produce(IntDimension tileSize,
                      IntRect gridBounds,
                      IntPoint anchor,
                      string tag,
                      TRawTexture texture);
    }

    public interface IDerivedTileProducer<TTile, TTexture>
        where TTile : ITexturedTile<TTexture>
        where TTexture : ITexture
    {
        IEnumerable<TTile> ProduceAll(IntDimension tileSize,
                                      IntPoint anchor,
                                      IEnumerable<string> tags,
                                      TTexture texture);

        TTile Produce(IntDimension tileSize,
                      IntPoint anchor,
                      string tag,
                      TTexture texture);
    }
}
