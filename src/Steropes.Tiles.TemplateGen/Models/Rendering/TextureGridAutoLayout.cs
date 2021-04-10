using Serilog;
using Steropes.Tiles.DataStructures;
using System;
using System.Linq;

namespace Steropes.Tiles.TemplateGen.Models.Rendering
{
    public static class TextureGridAutoLayout
    {
        static readonly ILogger Logger = SLog.ForContext(typeof(TextureGridAutoLayout));

        public static bool ArrangeGrids(GeneratorPreferences prefs, TileTextureCollection c)
        {
            _ = prefs ?? throw new ArgumentNullException(nameof(prefs));
            var grids = c.Grids.Select(g => new TextureGridLayoutNode(prefs, g)).OrderBy(e => -e.Size.Width).ToList();
            foreach (var node in grids)
            {
                Logger.Verbose("Layout: {GridName} Weight: {Weight} Size:{Size}", node.Grid.Name, node.Size.Width * node.Size.Height, node.Size);
            }

            // All grids sorted by largest space consumed.
            var root = new ArrangeNode<TextureGridLayoutNode>(2048, 2048);
            var success = true;
            foreach (var grid in grids)
            {
                if (!root.Insert(grid.Size, grid))
                {
                    success = false;
                }

                root.Print();
            }

            if (success)
            {
                root.Apply(n =>
                {
                    var content = n.Content;
                    if (content != null)
                    {
                        content.Offset = new IntPoint(n.X, n.Y);
                    }
                });
            }

            return success;
        }
    }

    
}
