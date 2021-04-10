using Steropes.Tiles.DataStructures;
using System;

namespace Steropes.Tiles.TemplateGen.Models.Rendering
{
    public static class TilePainterExtensions
    {
        public static ITilePainter CreateTilePainter(this TextureGrid grid, GeneratorPreferences prefs)
        {
            var t = grid.Parent?.Parent?.TileType ?? TileType.Grid;
            switch (t)
            {
                case TileType.Grid:
                    return new TextureGridTilePainter(prefs, grid);
                case TileType.Isometric:
                    return new IsoTilePainter(prefs, grid);
                //case TileType.Hex:
                //    return new GridTilePainter(prefs, grid); // todo Hex is not really supported here or in the base library for now.
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static IntDimension ComputeTileDimension(this TextureGrid grid)
        {
            var textureFile = grid.Parent?.Parent;
            if (textureFile == null)
            {
                throw new ArgumentException("TextureGrid is not associated with a texture file.");
            }

            var w = textureFile.Width;
            var h = textureFile.Height;
            if (grid.MatcherType == MatcherType.Corner)
            {
                return new IntDimension(w / 2, h / 2);
            }

            return new IntDimension(w, h);
        }
    }
}
