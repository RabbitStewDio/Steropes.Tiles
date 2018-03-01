using System.Collections.Generic;
using Steropes.Tiles.Matcher.TileTags;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Matcher.Registry
{
  public static class TileTagEntries
  {
    public static ITileTagEntrySelectionFactory<bool> CreateFlagTagEntries()
    {
      return new TileTagEntrySelectionFactory<bool>(new KeyValuePair<bool, string>(true, "1"),
                                                    new KeyValuePair<bool, string>(false, "0"));
    }

    public static ITileTagEntrySelectionFactory<CardinalIndex> CreateShortCardinalIndexTagEntries()
    {
      return new TileTagEntrySelectionFactory<CardinalIndex>(
        new KeyValuePair<CardinalIndex, string>(CardinalIndex.North, "n"),
        new KeyValuePair<CardinalIndex, string>(CardinalIndex.East, "e"),
        new KeyValuePair<CardinalIndex, string>(CardinalIndex.South, "s"),
        new KeyValuePair<CardinalIndex, string>(CardinalIndex.West, "w"));
    }

    public static ITileTagEntrySelectionFactory<CardinalIndex> CreateCardinalIndexTagEntries()
    {
      return new TileTagEntrySelectionFactory<CardinalIndex>(
        new KeyValuePair<CardinalIndex, string>(CardinalIndex.North, "north"),
        new KeyValuePair<CardinalIndex, string>(CardinalIndex.East, "east"),
        new KeyValuePair<CardinalIndex, string>(CardinalIndex.South, "south"),
        new KeyValuePair<CardinalIndex, string>(CardinalIndex.West, "west"));
    }

    public static ITileTagEntrySelectionFactory<Direction> CreateDirectionTagEntries()
    {
      return new TileTagEntrySelectionFactory<Direction>(new KeyValuePair<Direction, string>(Direction.Up, "u"),
                                                         new KeyValuePair<Direction, string>(Direction.Left, "l"),
                                                         new KeyValuePair<Direction, string>(Direction.Down, "d"),
                                                         new KeyValuePair<Direction, string>(Direction.Right, "r"));
    }
  }
}