using Avalonia.Media;
using SkiaSharp;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.TemplateGen.Models.Prefs;
using Steropes.Tiles.TemplateGen.Models.Rendering.Shapes;
using System;

namespace Steropes.Tiles.TemplateGen.Models.Rendering
{
    public class IsoTilePainter : TilePainterBase
    {
        public IsoTilePainter(GeneratorPreferences prefs, TextureGrid grid) : base(prefs, grid)
        { }

        public override void Draw(SKCanvas g, TextureTile tile)
        {
            DrawCellFrame(g, tile);

            var rect = GetTileArea(tile);
            var pen = Grid.TextureTileFormattingMetaData.TileOutlineColor ?? Preferences.DefaultTileColor;
            
            var shape = CreateShape(rect);
            shape.Draw(g, pen);

            DrawHeightIndicator(g, tile);
            DrawSelectorHint(g, tile);
            DrawAnchor(g, tile);
        }

        void DrawHeightIndicator(SKCanvas g, TextureTile tile)
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

            var brush = Grid.TextureTileFormattingMetaData.TileOutlineColor ?? Preferences.DefaultTileColor;
            var shape = CreateShape(new IntRect(rect.X, rect.Y - extraSpace, rect.Width, rect.Height));
            shape.Draw(g, brush);

            g.DrawRasterLine(brush, baseShape.Left, shape.Left);
            g.DrawRasterLine(brush, baseShape.Right, shape.Right);
            g.DrawRasterLine(brush, baseShape.Bottom, shape.Bottom);
        }

        static bool IsStandardIsoTile(IntRect rect)
        {
            if ((rect.Width % 8) != 0)
            {
                return false;
            }

            return (rect.Width == rect.Height * 2);
        }

        protected override void DrawSubCell(SKCanvas g, TextureTile tile, Direction direction, Color c)
        {
            var baseArea = GetTileArea(tile);
            var tileArea = GetSubTileAreaForDirection(direction, baseArea);
            CreateShape(tileArea).Draw(g, c);
        }

        IntRect GetSubTileAreaForDirection(Direction direction, IntRect baseArea)
        {
            IntRect tileArea;
            var centerX = Lerp4(baseArea.X, baseArea.X + baseArea.Width, 2);
            var centerY = Lerp4(baseArea.Y, baseArea.Y + baseArea.Height, 2);
            switch (direction)
            {
                case Direction.Up:
                {
                    var left = Lerp4(baseArea.X, baseArea.X + baseArea.Width, 1);
                    var right = Lerp4(baseArea.X, baseArea.X + baseArea.Width, 3);
                    tileArea = FromEdges(left, baseArea.Y, right, centerY);
                    break;
                }
                case Direction.Left:
                {
                    var top = Lerp4(baseArea.Y, baseArea.Y + baseArea.Height, 1);
                    var bottom = Lerp4(baseArea.Y, baseArea.Y + baseArea.Height, 3);
                    tileArea = FromEdges(baseArea.X, top, centerX, bottom);
                    break;
                }
                case Direction.Down:
                {
                    var left = Lerp4(baseArea.X, baseArea.X + baseArea.Width, 1);
                    var right = Lerp4(baseArea.X, baseArea.X + baseArea.Width, 3);
                    tileArea = FromEdges(left, centerY, right, baseArea.Y + baseArea.Height);
                    break;
                }
                case Direction.Right:
                {
                    var top = Lerp4(baseArea.Y, baseArea.Y + baseArea.Height, 1);
                    var bottom = Lerp4(baseArea.Y, baseArea.Y + baseArea.Height, 3);
                    tileArea = FromEdges(centerX, top, baseArea.X + baseArea.Width, bottom);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            return tileArea;
        }

        public static IShape CreateShape(IntRect rect)
        {
            // drawing in WinForms is odd. To fill the correct pixels in the 
            // bottom and right edge we have to correct the line drawing by
            // one pixel.
            var centerX = rect.X + rect.Width / 2;
            var centerY = rect.Y + rect.Height / 2;

            var left = new IntPoint(rect.X, centerY);
            var right = new IntPoint(rect.X + rect.Width - 1, centerY);
            var top = new IntPoint(centerX, rect.Y);
            var bottom = new IntPoint(centerX, rect.Y + rect.Height - 1);
            if (!IsStandardIsoTile(rect))
            {
                return new IsoShape(top, left, bottom, right);
            }

            // These drawing instructions create a valid pixel shape 
            // for isometric tiles that fit together correctly. 
            //
            // These work only without rendering artefacts (like displaced
            // pixels) if the tile width is a number divisible by 8 and 
            // has a height half the defined width.
            return new PixelPerfectIsoShape(top, left, bottom, right);
        }

        protected override void DrawIndexedDirection(SKCanvas g, TextureTile tile, NeighbourIndex idx)
        {
            var points = CreateShape(GetTileArea(tile)).ToHighlight().GetHighlightFor(idx);
            var brush = Grid.TextureTileFormattingMetaData.TileHighlightColor ?? Preferences.DefaultTileHighlightColor;

            for (var pidx = 1; pidx < points.Count; pidx += 1)
            {
                var p1 = points[pidx - 1];
                var p2 = points[pidx];
                g.DrawRasterLine(brush, p1.X, p1.Y, p2.X, p2.Y);
            }

        }
    }
}
