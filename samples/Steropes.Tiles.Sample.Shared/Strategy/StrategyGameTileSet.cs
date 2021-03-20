using System.Collections.Generic;
using System.Linq;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Demo.Core.GameData.Strategy.Model;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.TexturePack;

namespace Steropes.Tiles.Demo.Core.GameData.Strategy
{
    public interface IStrategyGameTileSet
    {
        IntDimension TileSize { get; }
        RenderType RenderType { get; }
        int Layers { get; }
        int BlendLayer { get; }

        bool TryGetTerrain(string tag, out TerrainGraphic g);
        IEnumerable<TerrainGraphic> TerrainsForLayer(int layer);
    }

    public static class StrategyGameTileSetExtensions
    {
        public static TerrainGraphic FindFirstTerrain(this IStrategyGameTileSet tileSet, ITerrain t)
        {
            foreach (var tg in t.AllGraphicTags())
            {
                if (tileSet.TryGetTerrain(tg, out TerrainGraphic g))
                {
                    return g;
                }
            }

            return null;
        }

        public static bool FindFirstTile<TRenderTile>(this ITileRegistry<TRenderTile> tileSet, IRuleElement t,
                                                      out TRenderTile result)
        {
            foreach (var tg in t.AllGraphicTags())
            {
                if (tileSet.TryFind(tg, out TRenderTile g))
                {
                    result = g;
                    return true;
                }
            }

            result = default(TRenderTile);
            return false;
        }
    }

    public class StrategyGameTileSet<TTile> : IStrategyGameTileSet
    {
        public StrategyGameTileSet(ITexturePack<TTile> textures,
                                   RenderType renderType)
        {
            Textures = textures;
            RenderType = renderType;
            Terrains = new Dictionary<string, TerrainGraphic>();
            BlendLayer = 2;
        }

        public int BlendLayer { get; }
        public Dictionary<string, TerrainGraphic> Terrains { get; }
        public ITexturePack<TTile> Textures { get; }
        public RenderType RenderType { get; }

        public IntDimension TileSize
        {
            get { return Textures.TileSize; }
        }

        public int Layers
        {
            get
            {
                var l = -1;
                foreach (var t in Terrains)
                {
                    foreach (var tg in t.Value.MatchRule.Keys)
                    {
                        if (tg > l)
                        {
                            l = tg;
                        }
                    }
                }

                return l;
            }
        }

        public IEnumerable<TerrainGraphic> TerrainsForLayer(int layer)
        {
            return
                from t in Terrains.Values
                where t.MatchRule.ContainsKey(layer)
                select t;
        }

        public bool TryGetTerrain(string tag, out TerrainGraphic g)
        {
            return Terrains.TryGetValue(tag, out g);
        }

        public void Add(TerrainGraphic g)
        {
            Terrains.Add(g.GraphicTag, g);
        }

        public IEnumerable<KeyValuePair<string, RenderLayerDefinition>> DefinitionsForLayer(int layer)
        {
            return from t in Terrains
                where t.Value.MatchRule.ContainsKey(layer)
                select new KeyValuePair<string, RenderLayerDefinition>(t.Key, t.Value.MatchRule[layer]);
        }

        public void InitializeBlendingRules(StrategyGameRules r)
        {
            var ts = this;
            ts.Add(new TerrainGraphic("coast", false, "t.blend.coast")
                .WithMatchRule(0, TerrainMatchType.CellGroup, "t.l0.cellgroup", "shallow", "deep", "land")
                .WithMatchRule(1, TerrainMatchType.Corner, "t.l1.coast_cell", "water", "ice"));

            ts.Add(new TerrainGraphic("floor", false, "t.blend.coast")
                .WithMatchRule(0, TerrainMatchType.CellGroup, "t.l0.cellgroup", "deep", "shallow", "land")
                .WithMatchRule(1, TerrainMatchType.Corner, "t.l1.floor_cell", "water", "ice"));

            ts.Add(new TerrainGraphic("arctic", false, "t.blend.arctic")
                .WithMatchRule(0, TerrainMatchType.CellGroup, "t.l0.cellgroup", "shallow", "deep", "land")
                .WithMatch(1, "ice")
                .WithMatchRule(2, TerrainMatchType.Basic, "t.l0.arctic1", "ice"));

            ts.Add(new TerrainGraphic("desert", true)
                .WithMatchRule(0, TerrainMatchType.CellGroup, "t.l0.cellgroup", "land", "deep", "shallow")
                .WithMatch(1, "land")
                .WithMatchRule(2, TerrainMatchType.Basic, "t.l0.desert1", "land"));

            ts.Add(new TerrainGraphic("forest", true)
                .WithMatchRule(0, TerrainMatchType.CellGroup, "t.l0.cellgroup", "land", "deep", "shallow")
                .WithMatch(1, "land")
                .WithMatchRule(2, TerrainMatchType.Basic, "t.l0.forest1", "land")
                .WithMatchRule(3, TerrainMatchType.Cardinal, "t.l1.forest", "forest", "forest"));

            ts.Add(new TerrainGraphic("grassland", true)
                .WithMatchRule(0, TerrainMatchType.CellGroup, "t.l0.cellgroup", "land", "deep", "shallow")
                .WithMatch(1, "land")
                .WithMatchRule(2, TerrainMatchType.Basic, "t.l0.grassland1", "land"));

            ts.Add(new TerrainGraphic("hills", true)
                .WithMatchRule(0, TerrainMatchType.CellGroup, "t.l0.cellgroup", "land", "deep", "shallow")
                .WithMatch(1, "land")
                .WithMatchRule(2, TerrainMatchType.Basic, "t.l0.hills1", "land")
                .WithMatchRule(3, TerrainMatchType.Cardinal, "t.l1.hills", "hills", "hills"));

            ts.Add(new TerrainGraphic("mountains", true)
                .WithMatchRule(0, TerrainMatchType.CellGroup, "t.l0.cellgroup", "land", "deep", "shallow")
                .WithMatch(1, "land")
                .WithMatchRule(2, TerrainMatchType.Basic, "t.l0.mountains1", "land")
                .WithMatchRule(3, TerrainMatchType.Cardinal, "t.l1.mountains", "mountains", "mountains"));

            ts.Add(new TerrainGraphic("tundra", true)
                .WithMatchRule(0, TerrainMatchType.CellGroup, "t.l0.cellgroup", "land", "deep", "shallow")
                .WithMatch(1, "land")
                .WithMatchRule(2, TerrainMatchType.Basic, "t.l0.tundra1", "land"));

            ts.Add(new TerrainGraphic("plains", true)
                .WithMatchRule(0, TerrainMatchType.CellGroup, "t.l0.cellgroup", "land", "deep", "shallow")
                .WithMatch(1, "land")
                .WithMatchRule(2, TerrainMatchType.Basic, "t.l0.plains1", "land"));

            ts.Add(new TerrainGraphic("swamp", true)
                .WithMatchRule(0, TerrainMatchType.CellGroup, "t.l0.cellgroup", "land", "deep", "shallow")
                .WithMatch(1, "land")
                .WithMatchRule(2, TerrainMatchType.Basic, "t.l0.swamp1", "land"));
        }
    }
}