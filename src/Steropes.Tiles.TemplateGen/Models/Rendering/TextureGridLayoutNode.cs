using Steropes.Tiles.DataStructures;
using Steropes.Tiles.TemplateGen.Models.Prefs;
using Steropes.Tiles.TemplateGen.Models.Rendering.Generators;
using System;

namespace Steropes.Tiles.TemplateGen.Models.Rendering
{
    public class TextureGridLayoutNode
    {
        readonly GeneratorPreferences prefs;
        readonly TextureGrid grid;

        public TextureGridLayoutNode(GeneratorPreferences prefs, TextureGrid grid)
        {
            this.prefs = prefs;
            this.grid = grid ?? throw new ArgumentNullException(nameof(grid));
            this.Margin = grid.FormattingMetaData.Margin ?? 0;
            this.ContentSize = ComputeContentSize();
            this.TextAreaSize = ComputeTextSize(ContentSize);
            this.Size = ComputeGridSize(TextAreaSize, ContentSize);
        }

        public IntDimension ContentSize { get; }

        public TextureGrid Grid
        {
            get { return grid; }
        }

        IntDimension ComputeContentSize()
        {
            var tileCount = MatchTypeStrategyFactory.StrategyFor(grid.MatcherType).GetTileArea(grid);
            var maxX = tileCount.Width;
            var maxY = tileCount.Height;

            var cs = grid.EffectiveCellSize;
            var pixelWidth = cs.Width * maxX;
            var pixelHeight = cs.Height * maxY;

            var paddedWidth = Math.Max(0, maxX - 1) * grid.CellSpacing;
            var paddedHeight = Math.Max(0, maxY - 1) * grid.CellSpacing;

            return new IntDimension(pixelWidth + paddedWidth, pixelHeight + paddedHeight);
        }

        public int TextSpacing => 5;

        IntDimension ComputeTextSize(IntDimension contentSize)
        {
            var title = grid.FormattingMetaData.Title;
            if (string.IsNullOrEmpty(title))
            {
                return new IntDimension();
            }

            return prefs.DefaultFont.MeasureText(title, contentSize.Width);
        }

        public IntDimension TextAreaSize { get; }

        IntDimension ComputeGridSize(IntDimension labelSize, IntDimension contentSize)
        {
            var textSizeWithSpacing = labelSize.Height;
            if (textSizeWithSpacing > 0)
            {
                textSizeWithSpacing += prefs.TextSpacing;
            }

            var (w, h) = (Math.Max(contentSize.Width, labelSize.Width),
                          contentSize.Height + textSizeWithSpacing);

            var border = grid.FormattingMetaData.Border ?? 0;
            var padding = grid.FormattingMetaData.Padding ?? 0;
            w += 2 * border;
            w += 2 * padding;
            h += 2 * border;
            h += 2 * padding;
            return new IntDimension(w, h);
        }

        public IntDimension Size { get; }

        public int Margin { get; }

        public IntPoint Offset
        {
            get { return new IntPoint(grid.X, grid.Y); }
            set
            {
                grid.X = value.X;
                grid.Y = value.Y;
            }
        }

        public IntPoint BottomRight => new IntPoint(Offset.X + Size.Width, Offset.Y + Size.Height);
    }
}
