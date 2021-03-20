using Steropes.Tiles.Demo.Core.GameData.Dungeon;
using Steropes.Tiles.Demo.Core.GameData.Dungeon.Model;
using Steropes.Tiles.Matcher;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Matcher.Sprites;

namespace Steropes.Tiles.Unit2D.Demo.Components.Dungeon
{
    public class ItemListMatcher<TTile> : ITileMatcher<TTile, IItem>
    {
        readonly DungeonGameData gd;
        readonly ITileRegistry<TTile> tiles;

        public ItemListMatcher(DungeonGameData gd, ITileRegistry<TTile> tiles)
        {
            this.gd = gd;
            this.tiles = tiles;
        }

        public bool Match(int x, int y, TileResultCollector<TTile, IItem> onMatchFound)
        {
            bool matched = false;
            foreach (var item in gd.QueryItems(x, y))
            {
                var name = item.ItemType.Name;
                if (tiles.TryFind(name, out TTile tile))
                {
                    matched = true;
                    onMatchFound(SpritePosition.Whole, tile, item);
                }
            }

            return matched;
        }
    }
}