using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout
{
    public class GridCollectionPainter
    {
        readonly GeneratorPreferences prefs;

        public GridCollectionPainter(GeneratorPreferences prefs)
        {
            this.prefs = prefs ?? throw new ArgumentNullException(nameof(prefs));
        }

        public Bitmap Produce(TextureCollection c)
        {
            var grids = c.Grids.Select(g => new LayoutNode(prefs, g)).ToList();
            var width = 0;
            var height = 0;
            foreach (var grid in grids)
            {
                var p = grid.Offset + grid.Size;
                width = Math.Max(p.X, width);
                height = Math.Max(p.Y, height);
            }

            Debug.WriteLine("Producing new preview ({0},{1}) with {2} elements.", width, height, grids.Count);
            var b = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var graphics = Graphics.FromImage(b);
            foreach (var node in grids)
            {
                var painter = new GridPainter(prefs, node);
                painter.Draw(graphics);
            }

            graphics.Dispose();
            return b;
        }
    }
}
