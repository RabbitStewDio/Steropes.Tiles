using SkiaSharp;
using Steropes.Tiles.DataStructures;
using System;

namespace Steropes.Tiles.TemplateGen.Models.Rendering
{
    public class TextureCollectionPainter
    {
        readonly GeneratorPreferences prefs;

        public TextureCollectionPainter(GeneratorPreferences prefs)
        {
            this.prefs = prefs ?? throw new ArgumentNullException(nameof(prefs));
        }

        public void Produce(SKCanvas graphics, 
                            TileTextureCollection c)
        {
            foreach (var g in c.Grids)
            {
                var node = new TextureGridLayoutNode(prefs, g);
                var painter = new TextureGridPainter(prefs, node);
                painter.Draw(graphics);
            }
        }

        public IntDimension ComputeRenderedSize(TileTextureCollection c)
        {
            var width = 0;
            var height = 0;
            foreach (var grid in c.Grids)
            {
                var node = new TextureGridLayoutNode(prefs, grid);
                var p = node.Offset + node.Size;
                width = Math.Max(p.X, width);
                height = Math.Max(p.Y, height);
            }

            return new IntDimension(width, height);
        }

        public SKBitmap CreateBitmap(TileTextureCollection c, int scale = 1, SKBitmap? bmp = null)
        {
            scale = Math.Max(scale, 1);
            
            var expectedSize = ComputeRenderedSize(c);
            expectedSize = new IntDimension(expectedSize.Width * scale, expectedSize.Height * scale);
            if (bmp == null || bmp.Width != expectedSize.Width || bmp.Height != expectedSize.Height)
            {
                bmp?.Dispose();
                bmp = new SKBitmap(expectedSize.Width, expectedSize.Height);
            }

            using var ctx = new SKCanvas(bmp);
            ctx.Scale(scale, scale);
            ctx.Clear(SKColor.Empty);
            Produce(ctx, c);
            return bmp;
        }
    }
}
