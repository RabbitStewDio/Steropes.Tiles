using System;
using System.Collections.Generic;
using System.Linq;
using Steropes.Tiles.MonogameDemo.GameData.Strategy.Model;
using Steropes.Tiles.MonogameDemo.Util;

namespace Steropes.Tiles.MonogameDemo.GameData.Strategy.Rendering
{
  public class TaggedRendingFactoryBase
  {
    protected ITileRenderModeContext Context { get; }
    readonly TerrainToGraphicTagMapping mappingHelper;

    public TaggedRendingFactoryBase(ITileRenderModeContext context)
    {
      Context = context ?? throw new ArgumentNullException(nameof(context));
      mappingHelper = new TerrainToGraphicTagMapping(Context.GameData.Rules.TerrainTypes, Context.TileSet);
    }

    /// <summary>
    ///  Returns a mapping for a cell mapping with multiple matches. The dictionary is
    ///  keyed by tag name (from RenderLayerDefinition.MatchAs or RenderLayerDefinition.MatchWith)
    ///  and contains a list of all terrains that identify (via MatchAs) as suitable matches.
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="matchSpecs"></param>
    /// <returns></returns>
    public IReadOnlyDictionary<string, IReadOnlyList<ITerrain>>
      ComputeMatchingTerrains(int layer, IReadOnlyCollection<string> matchSpecs)
    {

      // finds all terrains that identify (via property "matchAs") as suitable targets for the match.
      // This has to check locality, as match specs are given per layer, and the same word can
      // have different meanings in different layers.
      bool MatchInLayer(TerrainGraphic g)
      {
        if (!g.MatchRule.TryGetValue(layer, out RenderLayerDefinition rld))
        {
          return false;
        }
        if (rld.MatchAs == null)
        {
          return false;
        }

        return matchSpecs.Contains(rld.MatchAs);
      }

      KeyValuePair<string, TerrainGraphic> TerrainWithMatchSpec(TerrainGraphic g)
      {
        if (!g.MatchRule.TryGetValue(layer, out RenderLayerDefinition rld))
        {
          throw new Exception();
        }
        if (rld.MatchAs == null)
        {
          throw new Exception();
        }
        return new KeyValuePair<string, TerrainGraphic>(rld.MatchAs, g);
      }

      var terrainGraphicsForLayer = Context.TileSet.TerrainsForLayer(layer);
      var x = terrainGraphicsForLayer.ToList();

      var matching = from r in x where MatchInLayer(r) select TerrainWithMatchSpec(r);

      var kb = new KeyedListBuilder<string, ITerrain>();
      foreach (var pair in matching)
      {
        var matches = mappingHelper.TerrainsForGraphic(pair.Value);
        kb.AddRange(pair.Key, matches);
      }

      return kb.Build();
    }


  }
}