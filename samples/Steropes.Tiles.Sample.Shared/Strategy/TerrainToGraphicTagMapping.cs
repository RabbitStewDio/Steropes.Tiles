using System;
using System.Collections.Generic;
using Steropes.Tiles.Demo.Core.GameData.Strategy.Model;

namespace Steropes.Tiles.Demo.Core.GameData.Strategy
{
  /// <summary>
  ///  Helper class that encapsulate the precomputed forward and reverse mapping between
  ///  terrains (as seen in the map) and tile definitions. Terrains link to tiles via
  ///  the graphicsTag property and the "alternative graphics" list of tags. Terrains
  ///  will look for matching tile definitions in the order the tags are listed. Terrains
  ///  can share the same tile definition.
  /// </summary>
  public class TerrainToGraphicTagMapping
  {
    readonly Dictionary<ITerrain, TerrainGraphic> forwardMapping;
    readonly Dictionary<string, List<ITerrain>> reverseMapping;

    public TerrainToGraphicTagMapping(IEnumerable<ITerrain> terrainData, 
                                      IStrategyGameTileSet tileSet)
    {
      forwardMapping = new Dictionary<ITerrain, TerrainGraphic>();
      reverseMapping = new Dictionary<string, List<ITerrain>>();

      foreach (var t in terrainData)
      {
        var first = tileSet.FindFirstTerrain(t);
        if (first == null)
        {
          throw new Exception($"No graphics mapping for terrain type {t.Id}");
        }

        forwardMapping.Add(t, first);
        if (!reverseMapping.TryGetValue(first.GraphicTag, out List<ITerrain> terrains))
        {
          terrains = new List<ITerrain>();
          reverseMapping.Add(first.GraphicTag, terrains);
        }
        terrains.Add(t);
      }
    }


    public TerrainGraphic Find(ITerrain t)
    {
      return forwardMapping[t];
    }

    public List<ITerrain> TerrainsForGraphic(TerrainGraphic g)
    {
      if (reverseMapping.TryGetValue(g.GraphicTag, out List<ITerrain> retval))
      {
        return retval;
      }
      return new List<ITerrain>();
    }
  }
}