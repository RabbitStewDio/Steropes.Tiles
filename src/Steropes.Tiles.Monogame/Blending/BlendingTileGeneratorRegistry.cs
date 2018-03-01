using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Monogame.Tiles;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Monogame.Blending
{
  /// <summary>
  ///  Checks whether there are tiles in the format of "{prefix}.{tag}_{direction}" in the registry.
  ///  If so, use them as precomputed blend tiles. If no tiles are found, attempt to generate tiles
  ///  from the base tiles for the given terrains.
  /// <para/>
  ///  This class works directly with the given textures and therefore uses ITexturedTile as 
  ///  target type.
  /// </summary>
  public class BlendingTileGeneratorRegistry : ITileRegistryEx<CardinalIndex, ITexturedTile>
  {
    readonly BlendingTileGenerator generator;
    readonly Dictionary<TupleKey<string, CardinalIndex>, ITexturedTile> tiles;

    public BlendingTileGeneratorRegistry(GraphicsDevice device,
                                         ITileRegistry<ITexturedTile> tileRegistry,
                                         RenderType renderType,
                                         IntDimension tileSize,
                                         string prefix = null)
    {
      this.tiles = new Dictionary<TupleKey<string, CardinalIndex>, ITexturedTile>();
      this.generator = new BlendingTileGenerator(device, tileRegistry, renderType, tileSize, prefix);
    }

    public BlendingTileGeneratorRegistry WithPopulatedCache(IEnumerable<string> tags)
    {
      foreach (var tag in tags)
      {
        TryFind(tag, CardinalIndex.North, out var _);
        TryFind(tag, CardinalIndex.East, out var _);
        TryFind(tag, CardinalIndex.South, out var _);
        TryFind(tag, CardinalIndex.West, out var _);
      }

      return this;
    }

    public ITexturedTile Find(string tag, CardinalIndex selector)
    {
      if (TryFind(tag, selector, out var tile))
      {
        return tile;
      }

      throw new IndexOutOfRangeException($"No tile at '{tag}_{selector}'");
    }

    public bool TryFind(string tag, CardinalIndex selector, out ITexturedTile tile)
    {
      var key = new TupleKey<string, CardinalIndex>(tag, selector);
      if (tiles.TryGetValue(key, out tile))
      {
        return true;
      }

      tile = generator.Generate(tag, selector);
      tiles[key] = tile;
      return true;
    }
  }
}