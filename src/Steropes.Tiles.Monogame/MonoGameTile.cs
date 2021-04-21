using Steropes.Tiles.DataStructures;
using Steropes.Tiles.TexturePack;

namespace Steropes.Tiles.Monogame
{
    public class MonoGameTile : ITexturedTile<XnaTexture>
    {
        public string Tag { get; }
        public bool HasTexture => Texture.Valid;
        public XnaTexture Texture { get; }
        public IntPoint Anchor { get; }

        public MonoGameTile(string tag, XnaTexture texture, IntPoint anchor)
        {
            Tag = tag;
            Texture = texture;
            Anchor = anchor;
        }
    }
}
