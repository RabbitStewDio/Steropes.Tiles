using System.Collections.Generic;
using Steropes.Tiles.Matcher;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Monogame.Tiles;
using Steropes.Tiles.MonogameDemo.GameData.Dungeon.Model;

namespace Steropes.Tiles.MonogameDemo.GameData.Dungeon
{
  internal class ItemListMatcher : ITileMatcher<ITexturedTile, IItem>
  {
    readonly DungeonGameData gd;
    readonly ITileRegistry<ITexturedTile> tiles;
    readonly List<IItem> collector;

    public ItemListMatcher(DungeonGameData gd, ITileRegistry<ITexturedTile> tiles)
    {
      this.gd = gd;
      this.tiles = tiles;
      this.collector = new List<IItem>();
    }

    public bool Match(int x, int y, TileResultCollector<ITexturedTile, IItem> onMatchFound)
    {
      bool matched = false;
      foreach (var item in gd.QueryItems(x, y, collector))
      {
        var name = item.ItemType.Name;
        if (tiles.TryFind(name, out ITexturedTile tile))
        {
          matched = true;
          onMatchFound(SpritePosition.Whole, tile, item);
        }
      }
      return matched;
    }
  }
}