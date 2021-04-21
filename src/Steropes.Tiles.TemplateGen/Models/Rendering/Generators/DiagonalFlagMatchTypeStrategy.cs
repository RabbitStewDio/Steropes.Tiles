using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Matcher.Sprites;
using System.Collections.Generic;

namespace Steropes.Tiles.TemplateGen.Models.Rendering.Generators
{
    public class DiagonalFlagMatchTypeStrategy : IMatchTypeStrategy
    {
        public IntDimension GetTileArea(TextureGrid g)
        {
            return RequiredTiles;
        }

        public IntDimension RequiredTiles => new IntDimension(4, 4);

        public List<TextureTile> Generate(TextureGrid grid)
        {
            var keys = DiagonalTileSelectionKey.Values;
            var reg = new DiagonalTileRegistry<string>(new ReflectorRegistry(), null, grid.Pattern);
            var retval = new List<TextureTile>();
            for (var index = 0; index < keys.Length; index++)
            {
                var key = keys[index];
                if (!reg.TryFind(grid.Name, key, out var tileName))
                {
                    continue;
                }

                var x = index % 4;
                var y = index / 4;
                retval.Add(new TextureTile(true, x, y, tileName)
                {
                    SelectorHint = key.ToString()
                });
            }

            return retval;
        }
    }
}
