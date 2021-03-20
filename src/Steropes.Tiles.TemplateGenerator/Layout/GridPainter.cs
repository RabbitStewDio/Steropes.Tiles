using System;
using System.Diagnostics;
using System.Drawing;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout
{
    public class GridPainter
    {
        readonly GeneratorPreferences prefs;
        readonly TextureGrid grid;
        readonly LayoutNode node;

        public GridPainter(GeneratorPreferences prefs, LayoutNode node)
        {
            this.prefs = prefs;
            this.node = node ?? throw new ArgumentNullException(nameof(node));
            this.grid = node.Grid;
        }

        Size Size => node.Size;
        Point Offset => node.Offset;

        public void Draw(Graphics graphics)
        {
            if (Size.Width <= 0 || Size.Height <= 0)
            {
                return;
            }

            Debug.WriteLine("Start Grid ({0}), Offset: {1}, Size: {2}", grid.Name, Offset, Size);

            DrawBorder(graphics);

            var state = graphics.Save();
            try
            {
                graphics.TranslateTransform(ContentOffset.X, ContentOffset.Y);

                DrawTitle(graphics);

                foreach (var tile in grid.Tiles)
                {
                    DrawTile(graphics, tile);
                }
            }
            finally
            {
                graphics.Restore(state);
            }
        }

        void DrawBorder(Graphics graphics)
        {
            if ((grid.FormattingMetaData.Border ?? 1) > 0)
            {
                using (var borderPen = new Pen(grid.FormattingMetaData.BorderColor ?? Color.Gainsboro))
                {
                    graphics.DrawRectangle(borderPen, new Rectangle(Offset, new Size(Size.Width - 1, Size.Height - 1)));
                }
            }
        }

        Point ContentOffset
        {
            get
            {
                var border = grid.FormattingMetaData.Border ?? 0;
                var padding = grid.FormattingMetaData.Padding ?? 0;
                return new Point(Offset.X + border + padding,
                                 Offset.Y + border + padding);
            }
        }

        void DrawTitle(Graphics graphics)
        {
            if (!string.IsNullOrEmpty(grid.FormattingMetaData.Title))
            {
                Debug.WriteLine(" Title: {0}", grid.FormattingMetaData.Title);

                var contentSize = node.ContentSize;
                var textOffset = new Point(0, contentSize.Height + prefs.TextSpacing);
                var textArea = new Rectangle(textOffset, node.TextAreaSize);
                textArea.Width += 1;

                using (var brush = new SolidBrush(grid.FormattingMetaData.TextColor ?? Color.Black))
                {
                    using (var pen = new Pen(brush))
                    {
                        graphics.DrawLine(pen, textOffset, new Point(textOffset.X + contentSize.Width, textOffset.Y));
                        graphics.DrawString(grid.FormattingMetaData.Title, prefs.DefaultFont, brush, textArea);
                    }
                }
            }
        }

        void DrawTile(Graphics g, TextureTile tile)
        {
            var cw = grid.EffectiveCellSize;
            var x = tile.X * (cw.Width + grid.CellSpacing);
            var y = tile.Y * (cw.Height + grid.CellSpacing);

            var g2 = g.BeginContainer();
            try
            {
                g.TranslateTransform(x, y);
                var painter = grid.CreateTilePainter(prefs);
                painter.Draw(g, tile);
            }
            finally
            {
                g.EndContainer(g2);
            }
        }
    }
}
