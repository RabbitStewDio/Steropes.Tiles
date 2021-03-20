using System;
using System.Collections.Generic;
using System.Drawing;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout
{
    public class GridTilePainter : TilePainterBase
    {
        public GridTilePainter(GeneratorPreferences preferences, TextureGrid grid) : base(preferences, grid)
        { }

        public override void Draw(Graphics g, TextureTile tile)
        {
            DrawCellFrame(g, tile);

            var pen = new Pen(Grid.TextureTileFormattingMetaData.TileOutlineColor ?? Preferences.DefaultTileColor);

            // to be pixel perfect, the rectangle size must be reduced by one so that the
            // line is drawing within the tile area.
            g.DrawRectangle(pen, GetTileArea(tile));

            pen.Dispose();

            DrawSelectorHint(g, tile);
            DrawAnchor(g, tile);
        }

        public Rectangle GetTileHighlightArea(TextureTile tile)
        {
            var rect = GetTileArea(tile);
            rect.X += 2;
            rect.Y += 2;
            rect.Width -= 4;
            rect.Height -= 4;
            return rect;
        }

        int MidPoint(int p1, int p2)
        {
            return (p1 + p2) / 2;
        }

        protected override void DrawSubCell(Graphics g, TextureTile tile, Direction direction, Color c)
        {
            var baseArea = GetTileArea(tile);
            var tileArea = GetSubTileAreaForDirection(direction, baseArea);
            using (var pen = new Pen(c))
            {
                g.DrawRectangle(pen, tileArea.X, tileArea.Y, tileArea.Width - 1, tileArea.Height - 1);
            }
        }

        Rectangle GetSubTileAreaForDirection(Direction d, Rectangle baseArea)
        {
            var centerX = MidPoint(baseArea.Left, baseArea.Right);
            var centerY = MidPoint(baseArea.Top, baseArea.Bottom);
            switch (d)
            {
                case Direction.Up:
                    return FromEdges(baseArea.Left, baseArea.Top, centerX, centerY);
                case Direction.Right:
                    return FromEdges(centerX, baseArea.Top, baseArea.Right, centerY);
                case Direction.Down:
                    return FromEdges(centerX, centerY, baseArea.Right, baseArea.Bottom);
                case Direction.Left:
                    return FromEdges(baseArea.Left, centerY, centerX, baseArea.Bottom);
                default:
                    throw new ArgumentOutOfRangeException(nameof(d), d, null);
            }
        }

        protected override void DrawIndexedDirection(Graphics g, TextureTile tile, NeighbourIndex idx)
        {
            var points = new List<Point>();

            var rect = GetTileHighlightArea(tile);

            var left = rect.Left;
            var top = rect.Top;
            var right = rect.Right;
            var bottom = rect.Bottom;
            var centerX = MidPoint(rect.Left, rect.Right);
            var centerY = MidPoint(rect.Top, rect.Bottom);
            switch (idx)
            {
                case NeighbourIndex.North:
                    points.Add(new Point(left, top));
                    points.Add(new Point(right, top));
                    break;
                case NeighbourIndex.NorthEast:
                    points.Add(new Point(centerX, top));
                    points.Add(new Point(right, top));
                    points.Add(new Point(right, centerY));
                    break;
                case NeighbourIndex.East:
                    points.Add(new Point(right, top));
                    points.Add(new Point(right, bottom));
                    break;
                case NeighbourIndex.SouthEast:
                    points.Add(new Point(right, centerY));
                    points.Add(new Point(right, bottom));
                    points.Add(new Point(centerX, bottom));
                    break;
                case NeighbourIndex.South:
                    points.Add(new Point(left, bottom));
                    points.Add(new Point(right, bottom));
                    break;
                case NeighbourIndex.SouthWest:
                    points.Add(new Point(centerX, bottom));
                    points.Add(new Point(left, bottom));
                    points.Add(new Point(left, centerY));
                    break;
                case NeighbourIndex.West:
                    points.Add(new Point(left, top));
                    points.Add(new Point(left, bottom));
                    break;
                case NeighbourIndex.NorthWest:
                    points.Add(new Point(left, centerY));
                    points.Add(new Point(left, top));
                    points.Add(new Point(centerX, top));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(idx), idx, null);
            }

            var pen = new Pen(Grid.TextureTileFormattingMetaData.TileHighlightColor ?? Preferences.DefaultTileHighlightColor);

            g.DrawLines(pen, points.ToArray());

            pen.Dispose();
        }
    }
}
