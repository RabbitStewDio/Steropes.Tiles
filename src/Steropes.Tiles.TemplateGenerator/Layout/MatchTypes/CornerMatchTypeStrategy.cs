using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout.MatchTypes
{
    public class CornerMatchTypeStrategy : IMatchTypeStrategy
    {
        public Size GetTileArea(TextureGrid g)
        {
            return RequiredTiles;
        }

        public Size RequiredTiles => new Size(8, 4);

        /// Arrange the tile for each bit combination in a 2x2 square 
        /// and place each tile in a clock wise fashion based on the
        /// direction (U, L, D, R) 
        /// 
        /// There are 8 different bit combinations (identified by 'idx'), 
        /// arranged in two lines. Therefore there are 16 x 8 tiles in
        /// the grid.
        void Add(Dictionary<CornerTileSelectionKey, Point> k, int idx)
        {
            var baseX = (idx % 4) * 2;
            var baseY = (idx / 4) * 2;

            var b0 = (idx & 1) == 1;
            var b1 = (idx & 2) == 2;
            var b2 = (idx & 4) == 4;

            k.Add(CornerTileSelectionKey.ValueOf(Direction.Up, b0, b1, b2), new Point(baseX, baseY));
            k.Add(CornerTileSelectionKey.ValueOf(Direction.Right, b0, b1, b2), new Point(baseX + 1, baseY));
            k.Add(CornerTileSelectionKey.ValueOf(Direction.Down, b0, b1, b2), new Point(baseX + 1, baseY + 1));
            k.Add(CornerTileSelectionKey.ValueOf(Direction.Left, b0, b1, b2), new Point(baseX, baseY + 1));
        }

        public List<TextureTile> Generate(TextureGrid grid)
        {
            var keys = new Dictionary<CornerTileSelectionKey, Point>();
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
