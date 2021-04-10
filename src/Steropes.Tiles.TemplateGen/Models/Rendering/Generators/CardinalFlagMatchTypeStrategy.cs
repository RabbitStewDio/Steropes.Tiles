using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Matcher.Sprites;
using System.Collections.Generic;

namespace Steropes.Tiles.TemplateGen.Models.Rendering.Generators
{
    public class CardinalFlagMatchTypeStrategy : IMatchTypeStrategy
    {
        /// <summary>
        ///   Generates a perfect mapping for an easy to draw tile
        ///   template that covers every possible tile exactly once. 
        /// </summary>
        static int[] KeyOrdering =
        {
            0, 2, 0xA, 8,
            4, 6, 7, 0xC,
            5, 0xE, 0xF, 0xD,
            1, 3, 0xb, 9
        };
        
        public IntDimension GetTileArea(TextureGrid g)
        {
            return RequiredTiles;
        }

        public IntDimension RequiredTiles => new IntDimension(4, 4);

        public List<TextureTile> Generate(TextureGrid grid)
        {
            var keys = CardinalTileSelectorKey.Values;
            var reg = new CardinalTileRegistry<string>(new ReflectorRegistry(), null, grid.Pattern);
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
