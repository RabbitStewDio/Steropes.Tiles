using Avalonia.Media;
using Serilog;
using SkiaSharp;
using Steropes.Tiles.DataStructures;
using System;

namespace Steropes.Tiles.TemplateGen.Models.Rendering
{
    public class TextureGridPainter
    {
        static readonly ILogger Logger = SLog.ForContext<TextureGridPainter>();
        readonly GeneratorPreferences prefs;
        readonly TextureGrid grid;
        readonly TextureGridLayoutNode node;

        public TextureGridPainter(GeneratorPreferences prefs, TextureGridLayoutNode node)
        {
            this.prefs = prefs;
            this.node = node ?? throw new ArgumentNullException(nameof(node));
            this.grid = node.Grid;
        }

        IntDimension Size => node.Size;
        IntPoint Offset => node.Offset;

        public void Draw(SKCanvas graphics)
        {
            if (Size.Width <= 0 || Size.Height <= 0)
            {
                return;
            }

            Logger.Verbose("Start Grid ({GridName}), Offset: {Offset}, Size: {Size}, ContentOffset: {ContentOffset}", grid.Name, Offset, Size, ContentOffset);

            DrawBorder(graphics);

            var trs = graphics.TotalMatrix;
            graphics.Translate(ContentOffset.X, ContentOffset.Y);
            try
            {
                DrawTitle(graphics);

                foreach (var tile in grid.Tiles)
                {
                    DrawTile(graphics, tile);
                }
            }
            finally
            {
                graphics.SetMatrix(trs);
            }
        }

        void DrawBorder(SKCanvas graphics)
        {
            if ((grid.FormattingMetaData.Border ?? 1) > 0)
            {
                Color color = grid.FormattingMetaData.BorderColor ?? Colors.Gainsboro;
                graphics.DrawRectangle(color, new IntRect(Offset.X, Offset.Y, Size.Width, Size.Height));
            }
        }

        IntPoint ContentOffset
        {
            get
            {
                var border = grid.FormattingMetaData.Border ?? 0;
                var padding = grid.FormattingMetaData.Padding ?? 0;
                return new IntPoint(Offset.X + border + padding,
                                    Offset.Y + border + padding);
            }
        }

        void DrawTitle(SKCanvas graphics)
        {
            if (!string.IsNullOrEmpty(grid.FormattingMetaData.Title))
            {
                var contentSize = node.ContentSize;
                var textOffset = new IntPoint(0, contentSize.Height + prefs.TextSpacing);

                var brush = grid.FormattingMetaData.TextColor ?? Colors.Black;
                graphics.DrawLine(brush, textOffset, new IntPoint(textOffset.X + contentSize.Width, textOffset.Y));
                graphics.DrawTextLines(grid.FormattingMetaData.Title, prefs.DefaultFont, brush, textOffset, contentSize.Width);
            }
        }

        void DrawTile(SKCanvas g, TextureTile tile)
        {
            var cw = grid.EffectiveCellSize;
            var x = tile.X * (cw.Width + grid.CellSpacing);
            var y = tile.Y * (cw.Height + grid.CellSpacing);

            var trs = g.TotalMatrix;
            try
            {
                g.Translate(x, y);
                var painter = grid.CreateTilePainter(prefs);
                painter.Draw(g, tile);
            }
            finally
            {
                g.SetMatrix(trs);
            }
        }
    }
}
