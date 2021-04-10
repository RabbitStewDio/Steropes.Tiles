using System;

namespace Steropes.Tiles.TexturePack.Operations
{
    public class BoundedTextureData<TColor>
    {
        public BoundedTextureData(TextureCoordinateRect bounds, TColor[] textureData)
        {
            Bounds = bounds;
            TextureData = textureData ?? throw new ArgumentNullException(nameof(textureData));
        }

        /// <summary>
        ///  Treat this as read-only.
        /// </summary>
        public TColor[] TextureData { get; }

        public TextureCoordinateRect Bounds { get; }

        public TColor this[int x, int y]
        {
            get
            {
                if (!Bounds.Contains(x, y))
                {
                    return default;
                }

                var tx = x - Bounds.X;
                var ty = y - Bounds.Y;
                return TextureData[ty * Bounds.Width + tx];
            }
        }
    }
}
