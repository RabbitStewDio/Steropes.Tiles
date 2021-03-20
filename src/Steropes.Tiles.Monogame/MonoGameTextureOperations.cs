using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.TexturePack.Atlas;
using Steropes.Tiles.TexturePack.Operations;
using System;

namespace Steropes.Tiles.Monogame
{
    public class MonoGameTextureOperations : ITextureOperations<XnaTexture, Color>
    {
        readonly GraphicsDevice device;

        class Cache<TCached, TKey>
            where TCached : class
            where TKey : IEquatable<TKey>
        {
            public int Miss;
            public int Expired;
            public int Hit;

            TKey key;
            WeakReference<TCached> reference;

            public bool TryGet(out TCached t)
            {
                if (reference == null)
                {
                    t = default;
                    return false;
                }

                if (reference.TryGetTarget(out t))
                {
                    Hit += 1;
                    return true;
                }
                else
                {
                    Miss += 1;
                    return false;
                }
            }

            public bool TryGet(TKey k, out TCached t)
            {
                if (reference == null)
                {
                    Miss += 1;
                    t = default;
                    return false;
                }

                if (!reference.TryGetTarget(out t))
                {
                    Expired += 1;
                    t = default;
                    return false;
                }


                if (k.Equals(key))
                {
                    Hit += 1;
                    return true;
                }
                else
                {
                    Miss += 1;
                    return false;
                }
            }

            public void Put(TKey key, TCached data)
            {
                reference = new WeakReference<TCached>(data);
                this.key = key;
            }

            public override string ToString()
            {
                return $"{nameof(Miss)}: {Miss}, {nameof(Expired)}: {Expired}, {nameof(Hit)}: {Hit}";
            }
        }

        readonly Cache<Color[], int> clearTextureOperation;

        public MonoGameTextureOperations([NotNull] GraphicsDevice device)
        {
            this.device = device ?? throw new ArgumentNullException(nameof(device));
            clearTextureOperation = new Cache<Color[], int>();
        }

        public TextureCoordinateRect ToNative(IntDimension context, IntRect src)
        {
            return new TextureCoordinateRect(src.X, src.Y, src.Width, src.Height);
        }

        public ITextureAtlasBuilder<XnaTexture> CreateAtlasBuilder()
        {
            return new MultiTextureAtlasBuilder<XnaTexture, Color>(this);
        }

        public BoundedTextureData<Color> ExtractData(XnaTexture srcTexture, TextureCoordinateRect rect)
        {
            var texture = srcTexture.Texture;
            if (texture == null)
            {
                return CreateClearTexture(rect.Size);
            }

            var data = new Color[rect.Width * rect.Height];
            var srcBounds = srcTexture.Bounds;
            var b = srcBounds.Clip(new TextureCoordinateRect(rect.X + srcBounds.X, rect.Y + srcBounds.Y, rect.Width, rect.Height));
            texture.GetData(0, b.ToXna(), data, 0, data.Length);

            var textureBounds = new TextureCoordinateRect(rect.X, rect.Y, b.Width, b.Height);
            return new BoundedTextureData<Color>(textureBounds, data);
        }

        public BoundedTextureData<Color> CombineMask(BoundedTextureData<Color> color,
                                                     BoundedTextureData<Color> mask)
        {
            var colorBounds = color.Bounds;
            var maskBounds = mask.Bounds;
            if (colorBounds.Size != maskBounds.Size)
            {
                throw new ArgumentException("Masking requires equal-sized texture data: " + colorBounds.Size + " vs mask " + maskBounds.Size);
            }

            var width = colorBounds.Width;
            var height = colorBounds.Height;
            var retval = new Color[width * height];
            for (var y = 0; y < height; y += 1)
            {
                for (var x = 0; x < width; x += 1)
                {
                    var px = colorBounds.X + x;
                    var py = colorBounds.Y + y;
                    var c = color[px, py];

                    var mx = maskBounds.X + x;
                    var my = maskBounds.Y + y;
                    var a = mask[mx, my];

                    var tidx = y * width + x;
                    c.A = a.A;
                    retval[tidx] = c;
                }
            }

            return new BoundedTextureData<Color>(colorBounds, retval);
        }

        public BoundedTextureData<Color> CombineBlend(BoundedTextureData<Color> background,
                                                      BoundedTextureData<Color> foreground)
        {
            var backgroundBounds = background.Bounds;
            var foregroundBounds = foreground.Bounds;
            if (foreground.Bounds.Size != background.Bounds.Size)
            {
                throw new ArgumentException("Blending requires equal-sized texture data.");
            }

            var width = backgroundBounds.Width;
            var height = backgroundBounds.Height;

            var retval = new Color[width * height];
            for (var y = 0; y < height; y += 1)
            {
                for (var x = 0; x < width; x += 1)
                {
                    var px = backgroundBounds.X + x;
                    var py = backgroundBounds.Y + y;
                    Color src = background[px, py];

                    var tx = foregroundBounds.X + x;
                    var ty = foregroundBounds.Y + y;
                    Color tgt = foreground[tx, ty];

                    var tidx = y * width + x;

                    var srcW = src * (1 - tgt.A);
                    var tgtW = tgt * tgt.A;
                    retval[tidx] = new Color(srcW.R + tgtW.R, srcW.G + tgtW.G, srcW.B + tgtW.B, srcW.A + tgtW.A);
                }
            }

            return new BoundedTextureData<Color>(backgroundBounds, retval);
        }

        public XnaTexture ApplyTextureData(XnaTexture texture, BoundedTextureData<Color> result)
        {
            var b = result.Bounds;
            if (result.TextureData.Length != b.Width * b.Height)
            {
                throw new ArgumentException(
                    $"Texture data needs to have {b.Width} * {b.Height} = {b.Width * b.Height} pixels.");
            }

            if (texture.Bounds.Width != result.Bounds.Width)
            {
                throw new ArgumentException($"Width mismatch: src: {b.Width} target:{texture.Bounds.Width}");
            }

            if (texture.Bounds.Height != result.Bounds.Height)
            {
                throw new ArgumentException($"Width mismatch: src: {b.Width} target:{texture.Bounds.Width}");
            }

            texture.Texture.SetData(0, new Rectangle(b.X, b.Y, b.Width, b.Height), result.TextureData, 0, result.TextureData.Length);
            return texture;
        }

        public XnaTexture ApplyTextureData(XnaTexture texture, BoundedTextureData<Color> result, TextureCoordinatePoint offset)
        {
            var b = result.Bounds;
            if (result.TextureData.Length != b.Width * b.Height)
            {
                throw new ArgumentException();
            }

            if (offset.X + b.Width > texture.Texture.Width)
            {
                throw new IndexOutOfRangeException();
            }

            if (offset.Y + b.Height > texture.Texture.Height)
            {
                throw new IndexOutOfRangeException();
            }

            texture.Texture.SetData(0, new Rectangle(offset.X,
                                                     offset.Y,
                                                     b.Width,
                                                     b.Height),
                                    result.TextureData, 
                                    0, result.TextureData.Length);
            return texture;
        }

        public BoundedTextureData<Color> CreateClearTexture(IntDimension size)
        {
            var data = FillClearTextureCache(size);
            return new BoundedTextureData<Color>(new TextureCoordinateRect(0, 0, size.Width, size.Height), data);
        }


        public TextureCoordinateRect TileAreaForCardinalDirection(IntDimension ts, CardinalIndex dir)
        {
            var w = ts.Width;
            var h = ts.Height;
            var wHalf = ts.Width / 2;
            var hHalf = ts.Height / 2;

            switch (dir)
            {
                case CardinalIndex.West:
                    return new TextureCoordinateRect(0, 0, wHalf, hHalf);
                case CardinalIndex.North:
                    return new TextureCoordinateRect(wHalf, 0, w - wHalf, hHalf);
                case CardinalIndex.East:
                    return new TextureCoordinateRect(wHalf, hHalf, w - wHalf, h - hHalf);
                case CardinalIndex.South:
                    return new TextureCoordinateRect(0, hHalf, wHalf, h - hHalf);
                default:
                    throw new ArgumentException();
            }
        }

        public XnaTexture Clip(string name, XnaTexture texture, TextureCoordinateRect clipRegion)
        {
            return new XnaTexture(name, clipRegion.Clip(texture.Bounds), texture.Texture);
        }

        public XnaTexture CreateTexture(string name, IntDimension textureSize, bool clearToTransparentBlack = true)
        {
            var texture = new Texture2D(device, textureSize.Width, textureSize.Height, false, SurfaceFormat.Color);

            if (clearToTransparentBlack)
            {
                texture.SetData(FillClearTextureCache(textureSize));
            }

            return new XnaTexture(name, new TextureCoordinateRect(0, 0, textureSize.Width, textureSize.Height), texture);
        }

        Color[] FillClearTextureCache(IntDimension tileSize)
        {
            var l = tileSize.Width * tileSize.Height;
            if (clearTextureOperation.TryGet(l, out var t))
            {
                return t;
            }

            var clearDataCache = new Color[tileSize.Width * tileSize.Height];
            clearTextureOperation.Put(l, clearDataCache);
            return clearDataCache;
        }


        public void MakeDebugVisible(BoundedTextureData<Color> b)
        {
            for (var i = 0; i < b.TextureData.Length; i++)
            {
                if (b.TextureData[i].A > 0)
                {
                    return;
                }
            }

            // Debug.LogWarning("Data is all transparent pixels");
        }
    }
}
