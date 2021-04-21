using Steropes.Tiles.DataStructures;
using Steropes.Tiles.TexturePack.Grids;
using Steropes.Tiles.TexturePack.Operations;

namespace Steropes.Tiles.Monogame
{
    public class MonoGameTileProducer : TileProducerBase<MonoGameTile, XnaTexture, XnaRawTexture>
    {
        public MonoGameTileProducer(ITextureOperations<XnaTexture> textureOperations) : base(textureOperations, textureOperations.CreateAtlasBuilder())
        { }

        public MonoGameTileProducer(ITextureOperations<XnaTexture> textureOperations, ITextureAtlasBuilder<XnaTexture> atlasBuilder) : base(textureOperations, atlasBuilder)
        { }

        protected override MonoGameTile CreateTile(string tag, XnaTexture texture, IntDimension tileSize, IntPoint anchor)
        {
            return new MonoGameTile(tag, texture, anchor);
        }
    }
}
