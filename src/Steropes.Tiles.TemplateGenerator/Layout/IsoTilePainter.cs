using System;
using System.Drawing;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout
{
    public class IsoTilePainter : TilePainterBase
    {
        public IsoTilePainter(GeneratorPreferences prefs, TextureGrid grid) : base(prefs, grid)
        { }

        public override void Draw(Graphics g, TextureTile tile)
        {
            DrawCellFrame(g, tile);

            var rect = GetTileArea(tile);
            using (var pen = new Pen(Grid.TextureTileFormattingMetaData.TileOutlineColor ?? Preferences.DefaultTileColor))
            {
                var shape = CreateShape(rect);
                shape.Draw(g, pen);
            }

            DrawHeightIndicator(g, tile);
            DrawSelectorHint(g, tile);
            DrawAnchor(g, tile);
        }

        void DrawHeightIndicator(Graphics g, TextureTile tile)
        {
            var rect = GetTileArea(tile);
            var extraSpace = rect.Y;
            // Only draw the extra height if the resulting box would be 
            // sufficiently large. Otherwise the upper and lower shape
            // just meld together into a ugly blob.
            if (extraSpace < 5)
            {
                return;
            }

            var baseShape = CreateShape(rect);

            using (var pen = new Pen(Grid.TextureTileFormattingMetaData.TileOutlineColor ?? Preferences.DefaultTileColor))
            {
                var shape = CreateShape(new Rectangle(rect.X, rect.Y - extraSpace, rect.Width, rect.Height));
                shape.Draw(g, pen);

                g.DrawLine(pen, baseShape.Left, shape.Left);
                g.DrawLine(pen, baseShape.Right, shape.Right);
                g.DrawLine(pen, baseShape.Bottom, shape.Bottom);
            }
        }

        static bool IsStandardIsoTile(Rectangle rect)
        {
            if ((rect.Width % 8) != 0)
            {
                return false;
            }

            return (rect.Width == rect.Height * 2);
        }

        protected override void DrawSubCell(Graphics g, TextureTile tile, Direction direction, Color c)
        {
            var baseArea = GetTileArea(tile);
            var tileArea = GetSubTileAreaForDirection(direction, baseArea);
            using (var pen = new Pen(c))
            {
                CreateShape(tileArea).Draw(g, pen);
            }
        }

        Rectangle GetSubTileAreaForDirection(Direction direction, Rectangle baseArea)
        {
            Rectangle tileArea;
            var centerX = Lerp(baseArea.Left, baseArea.Right, 2);
            var centerY = Lerp(baseArea.Top, baseArea.Bottom, 2);
            switch (direction)
            {
                case Direction.Up:
                {
                    var left = Lerp(baseArea.Left, baseArea.Right, 1);
                    var right = Lerp(baseArea.Left, baseArea.Right, 3);
                    tileArea = FromEdges(left, baseArea.Top, right, centerY);
                    break;
                }
                case Direction.Left:
                {
                    var top = Lerp(baseArea.Top, baseArea.Bottom, 1);
                    var bottom = Lerp(baseArea.Top, baseArea.Bottom, 3);
                    tileArea = FromEdges(baseArea.Left, top, centerX, bottom);
                    break;
                }
                case Direction.Down:
                {
                    var left = Lerp(baseArea.Left, baseArea.Right, 1);
                    var right = Lerp(baseArea.Left, baseArea.Right, 3);
                    tileArea = FromEdges(left, centerY, right, baseArea.Bottom);
                    break;
                }
                case Direction.Right:
                {
                    var top = Lerp(baseArea.Top, baseArea.Bottom, 1);
                    var bottom = Lerp(baseArea.Top, baseArea.Bottom, 3);
                    tileArea = FromEdges(centerX, top, baseArea.Right, bottom);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            return tileArea;
        }

        public static IShape CreateShape(Rectangle rect)
        {
            // drawing in WinForms is odd. To fill the correct pixels in the 
            // bottom and right edge we have to correct the line drawing by
            // one pixel.
            var centerX = (rect.Left + rect.Right) / 2;
            var centerY = (rect.Top + rect.Bottom) / 2;

            var left = new Point(rect.Left, centerY);
            var right = new Point(rect.Right - 1, centerY);
            var top = new Point(centerX, rect.Top);
            var bottom = new Point(centerX, rect.Bottom - 1);
            if (!IsStandardIsoTile(rect))
            {
                return new IsoShape(top, left, bottom, right);
            }
            else
            {
                // These drawing instructions create a valid pixel shape 
                // for isometric tiles that fit together correctly. 
                //
                // These work only without rendering artefacts (like displaced
                // pixels) if the tile width is a number divisible by 8 and 
                // has a height half the defined width.
                return new PixelPerfectIsoShape(top, left, bottom, right);
            }
        }

        protected override void DrawIndexedDirection(Graphics g, TextureTile tile, NeighbourIndex idx)
        {
            var points = CreateShape(GetTileArea(tile)).ToHighlight().GetHighlightFor(idx);
            var pen = new Pen(Grid.TextureTileFormattingMetaData.TileHighlightColor ?? Preferences.DefaultTileHighlightColor);

            for (var pidx = 1; pidx < points.Count; pidx += 1)
            {
                var p1 = points[pidx - 1];
                var p2 = points[pidx];
                g.DrawLine(pen, p1, p2);
            }

            pen.Dispose();
        }
    }
}
