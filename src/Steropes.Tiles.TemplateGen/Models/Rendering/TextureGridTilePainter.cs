using Avalonia.Media;
using SkiaSharp;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.TemplateGen.Models.Prefs;
using System;
using System.Collections.Generic;

namespace Steropes.Tiles.TemplateGen.Models.Rendering
{
    public class TextureGridTilePainter : TilePainterBase
    {
        public TextureGridTilePainter(GeneratorPreferences preferences, TextureGrid grid) : base(preferences, grid)
        { }

        public override void Draw(SKCanvas g, TextureTile tile)
        {
            DrawCellFrame(g, tile);

            var pen = Grid.TextureTileFormattingMetaData.TileOutlineColor ?? Preferences.DefaultTileColor;

            // to be pixel perfect, the rectangle size must be reduced by one so that the
            // line is drawing within the tile area.
            var tileArea = GetTileArea(tile);
            g.DrawRectangle(pen, tileArea);

            DrawSelectorHint(g, tile);
            DrawAnchor(g, tile);
        }

        public IntRect GetTileHighlightArea(TextureTile tile)
        {
            var rect = GetTileArea(tile);
            return new IntRect(rect.X + 2, rect.Y + 2, rect.Width - 4, rect.Height - 4);
        }

        int MidPoint(int p1, int p2)
        {
            return (p1 + p2) / 2;
        }

        protected override void DrawSubCell(SKCanvas g, TextureTile tile, Direction direction, Color c)
        {
            var baseArea = GetTileArea(tile);
            var tileArea = GetSubTileAreaForDirection(direction, baseArea);
            g.DrawRectangle(c, new IntRect(tileArea.X, tileArea.Y, tileArea.Width - 1, tileArea.Height - 1));
        }

        IntRect GetSubTileAreaForDirection(Direction d, IntRect baseArea)
        {
            var centerX = MidPoint(baseArea.X, baseArea.X + baseArea.Width);
            var centerY = MidPoint(baseArea.Y, baseArea.Y + baseArea.Height);
            switch (d)
            {
                case Direction.Up:
                    return FromEdges(baseArea.X, baseArea.Y, centerX, centerY);
                case Direction.Right:
                    return FromEdges(centerX, baseArea.Y, baseArea.X + baseArea.Width, centerY);
                case Direction.Down:
                    return FromEdges(centerX, centerY, baseArea.X + baseArea.Width, baseArea.Y + baseArea.Height);
                case Direction.Left:
                    return FromEdges(baseArea.X, centerY, centerX, baseArea.Y + baseArea.Height);
                default:
                    throw new ArgumentOutOfRangeException(nameof(d), d, null);
            }
        }

        protected override void DrawIndexedDirection(SKCanvas g, TextureTile tile, NeighbourIndex idx)
        {
            var points = new List<IntPoint>();

            var rect = GetTileHighlightArea(tile);

            var left = rect.X;
            var top = rect.Y;
            var right = rect.X + rect.Width - 1;
            var bottom = rect.Y + rect.Height - 1;
            var centerX = MidPoint(rect.X, rect.X + rect.Width);
            var centerY = MidPoint(rect.Y, rect.Y + rect.Height);
            switch (idx)
            {
                case NeighbourIndex.North:
                    points.Add(new IntPoint(left, top));
                    points.Add(new IntPoint(right, top));
                    break;
                case NeighbourIndex.NorthEast:
                    points.Add(new IntPoint(centerX, top));
                    points.Add(new IntPoint(right, top));
                    points.Add(new IntPoint(right, centerY));
                    break;
                case NeighbourIndex.East:
                    points.Add(new IntPoint(right, top));
                    points.Add(new IntPoint(right, bottom));
                    break;
                case NeighbourIndex.SouthEast:
                    points.Add(new IntPoint(right, centerY));
                    points.Add(new IntPoint(right, bottom));
                    points.Add(new IntPoint(centerX, bottom));
                    break;
                case NeighbourIndex.South:
                    points.Add(new IntPoint(left, bottom));
                    points.Add(new IntPoint(right, bottom));
                    break;
                case NeighbourIndex.SouthWest:
                    points.Add(new IntPoint(centerX, bottom));
                    points.Add(new IntPoint(left, bottom));
                    points.Add(new IntPoint(left, centerY));
                    break;
                case NeighbourIndex.West:
                    points.Add(new IntPoint(left, top));
                    points.Add(new IntPoint(left, bottom));
                    break;
                case NeighbourIndex.NorthWest:
                    points.Add(new IntPoint(left, centerY));
                    points.Add(new IntPoint(left, top));
                    points.Add(new IntPoint(centerX, top));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(idx), idx, null);
            }

            var pen = Grid.TextureTileFormattingMetaData.TileHighlightColor ?? Preferences.DefaultTileHighlightColor;
            g.DrawGeometry(pen, points);
        }
    }
}
