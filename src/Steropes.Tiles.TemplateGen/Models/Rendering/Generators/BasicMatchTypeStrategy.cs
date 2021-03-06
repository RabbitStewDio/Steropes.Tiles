﻿using Steropes.Tiles.DataStructures;
using System;
using System.Collections.Generic;

namespace Steropes.Tiles.TemplateGen.Models.Rendering.Generators
{
    public class BasicMatchTypeStrategy : IMatchTypeStrategy
    {
        public IntDimension GetTileArea(TextureGrid grid)
        {
            var maxX = 0;
            var maxY = 0;

            foreach (var textureTile in grid.Tiles)
            {
                maxX = Math.Max(Math.Max(0, textureTile.X), maxX);
                maxY = Math.Max(Math.Max(0, textureTile.Y), maxY);
            }

            maxX += 1;
            maxY += 1;
            return new IntDimension(maxX, maxY);
        }

        public List<TextureTile> Generate(TextureGrid grid)
        {
            var w = grid.Width ?? 0;
            var h = grid.Height ?? 0;
            if (w <= 0 || h <= 0)
            {
                return new List<TextureTile>();
            }

            static bool HasTileAtPosition(TextureGrid g, int x, int y)
            {
                foreach (var t in g.Tiles)
                {
                    if (t.AutoGenerated)
                    {
                        continue;
                    }

                    if (t.X == x && t.Y == y)
                    {
                        return true;
                    }
                }

                return false;
            }
            
            var retval = new List<TextureTile>();
            
            for (var y = 0; y < h; y += 1)
            for (var x = 0; x < w; x += 1)
            {
                if (HasTileAtPosition(grid, x, y))
                {
                    retval.Add(new TextureTile()
                    {
                        AutoGenerated = true,
                        X = x,
                        Y = y
                    });
                }
            }

            return retval;
        }
    }
}
