using System.Collections.Generic;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Matcher.TileTags;
using Steropes.Tiles.Monogame.Tiles;

namespace Steropes.Tiles.MonogameDemo.GameData.Dungeon
{
  public interface ITileSet<TRenderTile>
  {
    IntDimension TileSize { get; }
    ITileRegistry<TRenderTile> LoadTexturePack(IContentLoader loader);
  }

  public class DungeonTileSet : ITileSet<ITexturedTile>
  {
    readonly TexturePack tp;

    public DungeonTileSet()
    {
      tp = new TexturePack("default", 
                           new IntDimension(50, 24), 
                           TextureType.Isometric, 
                           null,
                           LoadDecorations().ToArray());
    }

    public IntDimension TileSize
    {
      get { return tp.TileSize; }
    }

    /// <summary>
    ///   Loads all tiles using the given content loader. Returns a BasicTileRegistry to allow access to the
    ///   contained tiles.
    /// </summary>
    /// <param name="loader"></param>
    /// <returns></returns>
    public ITileRegistry<ITexturedTile> LoadTexturePack(IContentLoader loader)
    {
      var registry = new BasicTileRegistry<ITexturedTile>(TexturedTile.None);
      foreach (var tf in tp.TextureFiles)
      {
        foreach (var x in tf.ProduceTextures(loader))
        {
          registry.Add(x.Tag, x);
        }
      }
      return registry;
    }

    protected List<ITextureFile> LoadDecorations()
    {
      return new List<ITextureFile>
      {
        LoadWallsTextureFile(),
        LoadPeopleTextureFile(),
        new GridTextureFile("Tiles/Rpg/L2_Chest15_x",
                            new TileGrid(54, 54, 0, 0, 27, 40, new GridTileDefinition("item.chest-closed"))),
        new GridTextureFile("Tiles/Rpg/L2_Chest16_x",
                            new TileGrid(54, 54, 0, 0, 27, 40, new GridTileDefinition("item.chest-open"))),
        new GridTextureFile("Tiles/Rpg/L3_WallDeco03_x",
                            new TileGrid(54, 54, 0, 0, 27, 40,
                                         new GridTileDefinition("decoration.shelf_n1e0s1w0"))),
        new GridTextureFile("Tiles/Rpg/L3_WallDeco04_x",
                            new TileGrid(54, 54, 0, 0, 27, 40,
                                         new GridTileDefinition("decoration.shelf_n0e1s0w1"))),
        new GridTextureFile("Tiles/Rpg/L3_Window2_x",
                            new TileGrid(54, 54, 0, 0, 27, 40,
                                         new GridTileDefinition("decoration.window_n1e0s1w0"))),
        new GridTextureFile("Tiles/Rpg/L3_Window1_x",
                            new TileGrid(54, 54, 0, 0, 27, 40,
                                         new GridTileDefinition("decoration.window_n0e1s0w1"))),
      };
    }

    protected ITextureFile LoadPeopleTextureFile()
    {
      var path = "Tiles/Rpg/dg_people32";
      var grid = new TileGrid(32, 32, 0, 0, 16, 24)
      {
        new GridTileDefinition(0, 0, "character.child"),
        new GridTileDefinition(1, 0, "character.farmer"),
        new GridTileDefinition(0, 1, "character.jester"),
        new GridTileDefinition(3, 1, "character.thief")
      };
      return new GridTextureFile(path, grid);
    }

    protected ITextureFile LoadWallsTextureFile()
    {
      var path = "Tiles/Rpg/dg_iso32";
      var grid = new TileGrid(54, 49, 0, 0, 27, 35)
      {
        new GridTileDefinition(0, 0, "floor.gras"),
        new GridTileDefinition(2, 4, "floor.stone"),
        // Isolated stone
        new GridTileDefinition(0, 5, FormatName("wall.stone", CardinalFlags.North)),
        new GridTileDefinition(0, 5, FormatName("wall.stone", CardinalFlags.East)),
        new GridTileDefinition(0, 5, FormatName("wall.stone", CardinalFlags.South)),
        new GridTileDefinition(0, 5, FormatName("wall.stone", CardinalFlags.West)),
        new GridTileDefinition(0, 5, FormatName("wall.stone", CardinalFlags.None)),
        // 
        new GridTileDefinition(1, 5, FormatName("wall.stone", CardinalFlags.North | CardinalFlags.South)),
        new GridTileDefinition(2, 5, FormatName("wall.stone", CardinalFlags.East | CardinalFlags.West)),
        new GridTileDefinition(3, 5, FormatName("wall.stone", CardinalFlags.South | CardinalFlags.West)),
        new GridTileDefinition(4, 5, FormatName("wall.stone", CardinalFlags.North | CardinalFlags.West)),
        new GridTileDefinition(5, 5, FormatName("wall.stone", CardinalFlags.North | CardinalFlags.East)),
        new GridTileDefinition(6, 5, FormatName("wall.stone", CardinalFlags.South | CardinalFlags.East)),
        new GridTileDefinition(7, 5, FormatName("wall.stone",
                                                CardinalFlags.North |
                                                CardinalFlags.South |
                                                CardinalFlags.West)),
        new GridTileDefinition(8, 5, FormatName("wall.stone",
                                                CardinalFlags.North |
                                                CardinalFlags.East |
                                                CardinalFlags.West)),
        new GridTileDefinition(9, 5, FormatName("wall.stone",
                                                CardinalFlags.North |
                                                CardinalFlags.East |
                                                CardinalFlags.South)),
        new GridTileDefinition(10, 5, FormatName("wall.stone",
                                                 CardinalFlags.South |
                                                 CardinalFlags.East |
                                                 CardinalFlags.West)),
        new GridTileDefinition(11, 5, FormatName("wall.stone",
                                                 CardinalFlags.North |
                                                 CardinalFlags.South |
                                                 CardinalFlags.East |
                                                 CardinalFlags.West)),

        new GridTileDefinition(11, 6, FormatName("door-closed",
                                                 CardinalFlags.North | CardinalFlags.South)),

        new GridTileDefinition(12, 6, FormatName("door-closed",
                                                 CardinalFlags.East | CardinalFlags.West)),
        new GridTileDefinition(12, 6, "door-closed"),
        new GridTileDefinition(13, 6, FormatName("door-open",
                                                 CardinalFlags.North | CardinalFlags.South)),
        new GridTileDefinition(0, 7, FormatName("door-open",
                                                CardinalFlags.East | CardinalFlags.West)),
        new GridTileDefinition(0, 7, "door-open"),
        new GridTileDefinition(1, 7, FormatName("stairs",
                                                CardinalFlags.North | CardinalFlags.South)),
        new GridTileDefinition(2, 7, FormatName("stairs",
                                                CardinalFlags.East | CardinalFlags.West)),
        new GridTileDefinition(2, 7, "stairs")
      };

      return new GridTextureFile(path, grid);
    }

    static string FormatName(string tag, CardinalFlags key)
    {
      return $"{tag}_{Format(key.AsCardinalKey())}";
    }

    static string Format(CardinalTileSelectorKey k, 
                                ITileTagEntrySelectionFactory<bool> formatProvider = null,
                                string format = null)
    {
      formatProvider = formatProvider ?? TileTagEntries.CreateFlagTagEntries();
      format = format ?? "n{0}e{1}s{2}w{3}";
      var n = formatProvider.Lookup(k.North).Tag;
      var e = formatProvider.Lookup(k.East).Tag;
      var s = formatProvider.Lookup(k.South).Tag;
      var w = formatProvider.Lookup(k.West).Tag;
      return string.Format(format, n, e, s, w);
    }

  }
}