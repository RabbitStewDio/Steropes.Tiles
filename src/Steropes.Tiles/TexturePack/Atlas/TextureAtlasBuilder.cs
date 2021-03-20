using Steropes.Tiles.TexturePack;
using Steropes.Tiles.TexturePack.Operations;

namespace Steropes.Tiles.TexturePack.Atlas
{
    /// <summary>
    ///   Merges textures into one or more texture atlases. This class arranges
    ///   textures into bands and splits those bands into lines. It works best
    ///   with textures of uniform sizes.
    /// </summary>
    public class TextureAtlasBuilder<TTexture, TColor>
        where TTexture : ITexture
    {
        class TreeNode
        {
            bool HasTexture { get; set; }
            TTexture Texture { get; set; }
            TextureCoordinateRect CellBounds { get; }
            TreeNode Left { get; set; }
            TreeNode Right { get; set; }
            bool Leaf => Left == null && Right == null;


            public TreeNode(TextureCoordinateRect bounds)
            {
                CellBounds = bounds;
            }

            public TreeNode Insert(TTexture tex, int padding)
            {
                if (!Leaf)
                {
                    var maybeLeft = Left.Insert(tex, padding);
                    if (maybeLeft != null)
                    {
                        return maybeLeft;
                    }

                    return Right.Insert(tex, padding);
                }

                if (HasTexture)
                {
                    // There is already something here 
                    return null;
                }

                var texBounds = tex.Bounds;
                if (texBounds.Width > CellBounds.Width ||
                    texBounds.Height > CellBounds.Height)
                {
                    // does not fit into the available space
                    return null;
                }

                if (texBounds.Width == CellBounds.Width &&
                    texBounds.Height == CellBounds.Height)
                {
                    HasTexture = true;
                    Texture = tex;
                    return this;
                }

                if ((CellBounds.Width - texBounds.Width) > (CellBounds.Height - texBounds.Height))
                {
                    // vertical split 
                    Left = new TreeNode(new TextureCoordinateRect(CellBounds.X, CellBounds.Y, texBounds.Width, CellBounds.Height));
                    Right = new TreeNode(new TextureCoordinateRect(CellBounds.X + padding + texBounds.Width, CellBounds.Y,
                                                                   CellBounds.Width - texBounds.Width - padding, CellBounds.Height));
                }
                else
                {
                    Left = new TreeNode(new TextureCoordinateRect(CellBounds.X, CellBounds.Y, CellBounds.Width, texBounds.Height));
                    Right = new TreeNode(new TextureCoordinateRect(CellBounds.X, CellBounds.Y + padding + texBounds.Height,
                                                                   CellBounds.Width,
                                                                   CellBounds.Height - texBounds.Height - padding));
                }

                return Left.Insert(tex, padding);
            }

            public bool Harvest(ITextureOperations<TTexture, TColor> ops,
                                TTexture targetTexture,
                                out TTexture result)
            {
                if (HasTexture)
                {
                    var textureBounds = Texture.Bounds;
                    // Debug.Log("Extract " + Texture.Name + " via " + textureBounds);
                    var localTextureBounds = new TextureCoordinateRect
                        (0, 0, textureBounds.Width, textureBounds.Height);
                    var srcData = ops.ExtractData(Texture, localTextureBounds);
                    ops.MakeDebugVisible(srcData);
                    ops.ApplyTextureData(targetTexture, srcData,
                                         new TextureCoordinatePoint(CellBounds.X, CellBounds.Y));

                    result = ops.Clip(targetTexture.Name + ":" + Texture.Name, targetTexture, CellBounds);
                    // Debug.Log("Harvest: " + result.Name + ":" + Texture.Name + " " + result.Bounds + " @ " + textureBounds);
                    return true;
                }

                if (Left.Harvest(ops, targetTexture, out var l))
                {
                    result = l;
                    return true;
                }

                return Right.Harvest(ops, targetTexture, out result);
            }
        }

        public const int MaxTextureSize = 4096;

        readonly TreeNode root;
        readonly ITextureOperations<TTexture, TColor> textureOperations;

        public TTexture Texture { get; }
        public const int Padding = 2;

        public TextureAtlasBuilder(ITextureOperations<TTexture, TColor> textureOperations,
                                   TTexture texture)
        {
            root = new TreeNode(texture.Bounds);
            this.Texture = texture;
            this.textureOperations = textureOperations;
        }

        public bool Insert(TTexture tile, out TTexture result)
        {
            if (!tile.Valid)
            {
                result = tile;
                return true;
            }

            var res = root.Insert(tile, Padding);
            if (res != null)
            {
                return res.Harvest(textureOperations, Texture, out result);
            }

            result = tile;
            return false;
        }
    }
}
