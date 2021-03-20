using Steropes.Tiles.DataStructures;
using Steropes.Tiles.TexturePack.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Steropes.Tiles.TexturePack.Grids
{
    public abstract class TileProducerBase<TTile, TTexture, TRawTexture> : ITileProducer<TTile, TTexture, TRawTexture>,
                                                                           IDerivedTileProducer<TTile, TTexture>
        where TTile : ITexturedTile<TTexture>
        where TTexture : ITexture
        where TRawTexture : IRawTexture<TTexture>
    {
        readonly ITextureOperations<TTexture> textureOperations;
        readonly ITextureAtlasBuilder<TTexture> textureAtlas;

        protected TileProducerBase(ITextureOperations<TTexture> textureOperations,
                                ITextureAtlasBuilder<TTexture> atlasBuilder)
        {
            this.textureOperations = textureOperations;
            textureAtlas = atlasBuilder ?? throw new ArgumentNullException();
        }

        public IEnumerable<TTile> ProduceAll(IntDimension tileSize,
                                             IntRect gridBounds,
                                             IntPoint anchor,
                                             IEnumerable<string> tags,
                                             TRawTexture texture)
        {
            var tagsAsList = tags.ToArray();
            var subTextureName = string.Join(",", tagsAsList);
            var subTextureBounds = texture.Bounds.Clip(gridBounds);
            var nativeBounds = textureOperations.ToNative(texture.Bounds.Size, subTextureBounds);
            var nativeTexture = texture.CreateSubTexture(subTextureName, nativeBounds);
            var atlasTexture = textureAtlas.Add(nativeTexture);

            foreach (var tileTag in tagsAsList)
            {
                yield return CreateTile(tileTag, atlasTexture, tileSize, anchor);
            }
        }

        protected abstract TTile CreateTile(string tag, TTexture texture, IntDimension tileSize, IntPoint anchor);

        public TTile Produce(IntDimension tileSize, IntRect gridBounds, IntPoint anchor, string tag, TRawTexture texture)
        {
            var subTextureName = tag + "@" + texture.Name;
            var subTextureBounds = texture.Bounds.Clip(gridBounds);
            var nativeBounds = textureOperations.ToNative(texture.Bounds.Size, subTextureBounds);
            var nativeTexture = texture.CreateSubTexture(subTextureName, nativeBounds);
            var atlasTexture = textureAtlas.Add(nativeTexture);
            return CreateTile(tag, atlasTexture, tileSize, anchor);
        }

        public IEnumerable<TTile> ProduceAll(IntDimension tileSize, IntPoint anchor, IEnumerable<string> tags, TTexture texture)
        {
            var tagsAsList = tags.ToArray();
            var atlasTexture = textureAtlas.Add(texture);
            foreach (var tileTag in tagsAsList)
            {
                yield return CreateTile(tileTag, atlasTexture, tileSize, anchor);
            }
        }

        public TTile Produce(IntDimension tileSize, IntPoint anchor, string tag, TTexture texture)
        {
            var atlasTexture = textureAtlas.Add(texture);
            return CreateTile(tag, atlasTexture, tileSize, anchor);
        }
    }
}
