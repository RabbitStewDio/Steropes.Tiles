using Microsoft.Xna.Framework.Graphics;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.TexturePack;
using Steropes.Tiles.TexturePack.Operations;

namespace Steropes.Tiles.Monogame
{
    public readonly struct XnaRawTexture : IRawTexture<XnaTexture>
    {
        public string Name { get; }
        public IntRect Bounds { get; }
        public Texture2D Texture { get; }
        public bool Valid => !(Texture?.IsDisposed ?? true);

        public XnaRawTexture(string name, Texture2D texture) : this()
        {
            Name = name;
            Texture = texture;
            Bounds = texture.Bounds.ToTileRect();
        }

        public XnaRawTexture(string name, IntRect bounds, Texture2D texture)
        {
            Name = name;
            Bounds = bounds;
            Texture = texture;
        }

        public XnaTexture CreateSubTexture(string name, TextureCoordinateRect bounds)
        {
            return new XnaTexture(name ?? Name, bounds, Texture);
        }
    }
}
