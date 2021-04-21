using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Matcher.Sprites;
using System.Collections.Generic;

namespace Steropes.Tiles.TemplateGen.Models.Rendering.Generators
{
    public class CornerMatchTypeStrategy : IMatchTypeStrategy
    {
        public IntDimension GetTileArea(TextureGrid g)
        {
            return RequiredTiles;
        }

        public IntDimension RequiredTiles => new IntDimension(8, 4);

        /// Arrange the tile for each bit combination in a 2x2 square 
        /// and place each tile in a clock wise fashion based on the
        /// direction (U, L, D, R) 
        /// 
        /// There are 8 different bit combinations (identified by 'idx'), 
        /// arranged in two lines. Therefore there are 16 x 8 tiles in
        /// the grid.
        void Add(Dictionary<CornerTileSelectionKey, IntPoint> k, int idx)
        {
            var baseX = (idx % 4) * 2;
            var baseY = (idx / 4) * 2;

            var b0 = (idx & 1) == 1;
            var b1 = (idx & 2) == 2;
            var b2 = (idx & 4) == 4;

            k.Add(CornerTileSelectionKey.ValueOf(Direction.Up, b0, b1, b2), new IntPoint(baseX, baseY));
            k.Add(CornerTileSelectionKey.ValueOf(Direction.Right, b0, b1, b2), new IntPoint(baseX + 1, baseY));
            k.Add(CornerTileSelectionKey.ValueOf(Direction.Down, b0, b1, b2), new IntPoint(baseX + 1, baseY + 1));
            k.Add(CornerTileSelectionKey.ValueOf(Direction.Left, b0, b1, b2), new IntPoint(baseX, baseY + 1));
        }

        public List<TextureTile> Generate(TextureGrid grid)
        {
            var keys = new Dictionary<CornerTileSelectionKey, IntPoint>();
            for (var i = 0; i < 8; i += 1)
            {
                Add(keys, i);
            }

            var reg = new CornerTileRegistry<string>(new ReflectorRegistry(), null, null, grid.Pattern);

            var retval = new List<TextureTile>();
            foreach (var name in keys)
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
