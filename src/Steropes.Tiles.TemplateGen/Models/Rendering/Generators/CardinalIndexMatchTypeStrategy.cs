using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Navigation;
using System.Collections.Generic;

namespace Steropes.Tiles.TemplateGen.Models.Rendering.Generators
{
    public class CardinalIndexMatchTypeStrategy : IMatchTypeStrategy
    {
        public IntDimension RequiredTiles => new IntDimension(2, 2);

        public IntDimension GetTileArea(TextureGrid g)
        {
            return RequiredTiles;
        }

        public List<TextureTile> Generate(TextureGrid grid)
        {
            var ni = new Dictionary<CardinalIndex, IntPoint>
            {
                {CardinalIndex.North, new IntPoint(0, 0)},
                {CardinalIndex.East, new IntPoint(1, 0)},
                {CardinalIndex.South, new IntPoint(0, 1)},
                {CardinalIndex.West, new IntPoint(1, 1)}
            };

            var reg = new CardinalIndexTileRegistry<string>(new ReflectorRegistry(),
                                                            TileTagEntries.CreateCardinalIndexTagEntries(),
                                                            grid.Pattern);

            var retval = new List<TextureTile>();
            foreach (var name in ni)
            {
                if (!reg.TryFind(grid.Name, name.Key, out var tileName))
                {
                    continue;
                }

                retval.Add(new TextureTile(true, name.Value.X, name.Value.Y, tileName)
                {
                    SelectorHint = name.Key.ToString()
                });
            }

            return retval;
        }
    }
}
