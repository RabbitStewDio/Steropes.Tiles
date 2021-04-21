using Microsoft.Xna.Framework.Graphics;
using Steropes.Tiles.TexturePack;
using Steropes.Tiles.TexturePack.Operations;

namespace Steropes.Tiles.Monogame
{
    public readonly struct XnaTexture : ITexture
    {
        public string Name { get; }
        public TextureCoordinateRect Bounds { get; }
        public Texture2D Texture { get; }
        public bool Valid => !(Texture?.IsDisposed ?? true);

        public XnaTexture(string name, TextureCoordinateRect bounds, Texture2D texture)
        {
            Name = name;
            Bounds = bounds;
            Texture = texture;
        }
    }
}
