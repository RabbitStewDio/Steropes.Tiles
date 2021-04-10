using Avalonia.Media;
using SkiaSharp;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Navigation;
using System;

namespace Steropes.Tiles.TemplateGen.Models.Rendering
{
    public abstract class TilePainterBase : ITilePainter
    {
        public GeneratorPreferences Preferences { get; }

        protected TilePainterBase(GeneratorPreferences preferences, TextureGrid grid)
        {
            this.Preferences = preferences;
            Grid = grid ?? throw new ArgumentNullException(nameof(grid));
        }

        public IntRect GetTileArea(TextureTile tile)
        {
            if (tile.Parent == null)
            {
                return default;
            }

            var tileSize = Grid.EffectiveTileSize;
            return new IntRect(0, 0,
                               tileSize.Width,
                               tileSize.Height);
        }

        public TextureGrid Grid { get; }

        public abstract void Draw(SKCanvas g, TextureTile tile);

        protected void DrawCellFrame(SKCanvas g, TextureTile tile)
        {
            var bg = Grid.FormattingMetaData.BackgroundColor ?? Preferences.DefaultBackgroundColor;
            if (bg != null)
            {
                g.FillRectangle(bg.Value, GetTileArea(tile));
            }
        }

        protected virtual void DrawIndexedDirection(SKCanvas g, TextureTile tile, NeighbourIndex idx)
        { }

        protected virtual void DrawSubCell(SKCanvas g, TextureTile tile, Direction direction, Color c)
        { }

        void DrawCellMap(SKCanvas g, TextureTile t)
        {
            var hint = (t.SelectorHint ?? "").Split(',');
            for (var i = 0; i < hint.Length; i++)
            {
                if (int.TryParse(hint[i], out var colorIndex) &&
                    colorIndex >= 0 &&
                    colorIndex < Preferences.TileColors.Count)
                {
                    var c = Preferences.TileColors[colorIndex];
                    DrawSubCell(g, t, (Direction)i, c);
                }
            }
        }

        protected static int Lerp4(int left, int right, int quarterFraction)
        {
            return left + (right - left) * quarterFraction / 4;
        }

        protected static IntRect FromEdges(int left, int top, int right, int bottom)
        {
            return new IntRect(left, top, right - left, bottom - top);
        }

        protected void DrawAnchor(SKCanvas g, TextureTile t)
        {
            var textureGrid = t.Parent;
            if (textureGrid == null)
            {
                return;
            }
            
            var size = textureGrid.ComputeTileDimension();
            if (size.Width < 32 || size.Height < 32)
            {
                // There is no point in drawing an anchor if the tile is so small ..
                return;
            }

            var anchor = textureGrid.ComputeEffectiveAnchorPoint(t);
            var pen = textureGrid.TextureTileFormattingMetaData.TileHighlightColor ?? Preferences.DefaultTileHighlightColor;
            g.DrawPixel(pen, anchor.X, anchor.Y);
            g.DrawRasterLine(pen, anchor.X - 4, anchor.Y, anchor.X - 2, anchor.Y);
            g.DrawRasterLine(pen, anchor.X + 4, anchor.Y, anchor.X + 2, anchor.Y);
            g.DrawRasterLine(pen, anchor.X, anchor.Y - 2, anchor.X, anchor.Y - 4);
            g.DrawRasterLine(pen, anchor.X, anchor.Y + 2, anchor.X, anchor.Y + 4);
        }

        void DrawCornerMap(SKCanvas g, TextureTile t)
        {
            var hint = (t.SelectorHint ?? "").PadLeft(5);

            void IfFlagSet(int pos, Action a)
            {
                var c = hint[pos];
                if (c == '1')
                {
                    a();
                }
            }

            var highlightColor = t.Parent?.TextureTileFormattingMetaData.TileHighlightColor ?? Preferences.DefaultTileHighlightColor;
            var baseColor = t.Parent?.TextureTileFormattingMetaData.TileAnchorColor ?? Preferences.DefaultTileAnchorColor;
            var direction = hint[0];
            if (direction == 'U')
            {
                DrawSubCell(g, t, Direction.Down, baseColor);
                IfFlagSet(2, () => DrawSubCell(g, t, Direction.Left, highlightColor));
                IfFlagSet(3, () => DrawSubCell(g, t, Direction.Up, highlightColor));
                IfFlagSet(4, () => DrawSubCell(g, t, Direction.Right, highlightColor));
            }
            else if (direction == 'R')
            {
                DrawSubCell(g, t, Direction.Left, baseColor);
                IfFlagSet(2, () => DrawSubCell(g, t, Direction.Up, highlightColor));
                IfFlagSet(3, () => DrawSubCell(g, t, Direction.Right, highlightColor));
                IfFlagSet(4, () => DrawSubCell(g, t, Direction.Down, highlightColor));
            }
            else if (direction == 'D')
            {
                DrawSubCell(g, t, Direction.Up, baseColor);
                IfFlagSet(2, () => DrawSubCell(g, t, Direction.Right, highlightColor));
                IfFlagSet(3, () => DrawSubCell(g, t, Direction.Down, highlightColor));
                IfFlagSet(4, () => DrawSubCell(g, t, Direction.Left, highlightColor));
            }
            else if (direction == 'L')
            {
                DrawSubCell(g, t, Direction.Right, baseColor);
                IfFlagSet(2, () => DrawSubCell(g, t, Direction.Down, highlightColor));
                IfFlagSet(3, () => DrawSubCell(g, t, Direction.Left, highlightColor));
                IfFlagSet(4, () => DrawSubCell(g, t, Direction.Up, highlightColor));
            }
        }

        protected void DrawSelectorHint(SKCanvas g, TextureTile t)
        {
            var matcherType = t.Parent?.MatcherType ?? MatcherType.Basic;
            switch (matcherType)
            {
                case MatcherType.Basic:
                    break;
                case MatcherType.CardinalFlags:
                    DrawCardinalFlagHint(g, t);
                    break;
                case MatcherType.CardinalIndex:
                    DrawIndexedDirectionHint(g, t);
                    break;
                case MatcherType.CellMap:
                    DrawCellMap(g, t);
                    break;
                case MatcherType.Corner:
                    DrawCornerMap(g, t);
                    break;
                case MatcherType.DiagonalFlags:
                    DrawDiagonalFlagHint(g, t);
                    break;
                case MatcherType.NeighbourIndex:
                    DrawIndexedDirectionHint(g, t);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void DrawCardinalFlagHint(SKCanvas g, TextureTile t)
        {
            var text = (t.SelectorHint ?? "").PadLeft(4);

            void IfFlagSet(CardinalIndex pos, Action a)
            {
                var c = text[(int)pos];
                if (c == '1')
                {
                    a();
                }
            }

            IfFlagSet(CardinalIndex.North, () => DrawIndexedDirection(g, t, NeighbourIndex.North));
            IfFlagSet(CardinalIndex.East, () => DrawIndexedDirection(g, t, NeighbourIndex.East));
            IfFlagSet(CardinalIndex.South, () => DrawIndexedDirection(g, t, NeighbourIndex.South));
            IfFlagSet(CardinalIndex.West, () => DrawIndexedDirection(g, t, NeighbourIndex.West));
        }

        void DrawDiagonalFlagHint(SKCanvas g, TextureTile t)
        {
            var text = (t.SelectorHint ?? "").PadLeft(4);

            void IfFlagSet(DiagonalIndex pos, Action a)
            {
                var c = text[(int)pos];
                if (c == '1')
                {
                    a();
                }
            }

            IfFlagSet(DiagonalIndex.NorthWest, () => DrawIndexedDirection(g, t, NeighbourIndex.NorthWest));
            IfFlagSet(DiagonalIndex.NorthEast, () => DrawIndexedDirection(g, t, NeighbourIndex.NorthEast));
            IfFlagSet(DiagonalIndex.SouthEast, () => DrawIndexedDirection(g, t, NeighbourIndex.SouthEast));
            IfFlagSet(DiagonalIndex.SouthWest, () => DrawIndexedDirection(g, t, NeighbourIndex.SouthWest));
        }

        void DrawIndexedDirectionHint(SKCanvas g, TextureTile t)
        {
            var text = t.SelectorHint ?? "";
            // I intentionally parse the NeighbourMatchIndex value as NeighbourIndex so that the parsing 
            // ignores the "isolated" tile. 
            if (Enum.TryParse(text, out NeighbourIndex idx))
            {
                DrawIndexedDirection(g, t, idx);
            }
        }
    }
}
