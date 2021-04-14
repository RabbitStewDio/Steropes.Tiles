using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Matcher.TileTags;
using Steropes.Tiles.Plotter.Operations;
using Steropes.Tiles.Plotter.Operations.Builder;
using Steropes.Tiles.Sample.Shared.Util;
using Steropes.Tiles.TexturePack;
using Steropes.Tiles.TexturePack.Grids;
using Steropes.Tiles.TexturePack.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

// using Steropes.Tiles.Unity2D;
// using UnityEngine;

namespace Steropes.Tiles.Sample.Shared.Strategy.Rendering
{
    public class StrategyGameRenderingFactory<TTile, TTexture, TColor> : ITileRenderModeContext, IRenderingFactoryConfig<TTile>
        where TTile : ITexturedTile<TTexture>
        where TTexture : ITexture
    {
        readonly IDerivedTileProducer<TTile, TTexture> tileProducer;
        readonly ITextureOperations<TTexture, TColor> textureOperations;
        readonly BoolRenderingFactory boolRendering;
        readonly TagSetRenderingFactory cellRendering;

        public StrategyGameRenderingFactory(GameRenderingConfig renderingConfig,
                                            StrategyGameData gameData,
                                            StrategyGameTileSet<TTile> tileSet,
                                            IDerivedTileProducer<TTile, TTexture> tileProducer,
                                            ITextureOperations<TTexture, TColor> textureOperations)
        {
            this.tileProducer = tileProducer;
            this.textureOperations = textureOperations;
            RenderingConfig = renderingConfig ?? throw new ArgumentNullException(nameof(RenderingConfig));
            GameData = gameData ?? throw new ArgumentNullException(nameof(GameData));
            TileSet = tileSet ?? throw new ArgumentNullException(nameof(TileSet));

            var r = new BasicTileRegistry<TTile>();
            foreach (var t in tileSet.Textures.Tiles)
            {
                Console.WriteLine("Adding tile " + t.Tag);
                r.Add(t.Tag, t);
            }

            Tiles = r;

            cellRendering = new TagSetRenderingFactory(this);
            boolRendering = new BoolRenderingFactory(this);
            RenderControl = new RendererControl(tileSet.TileSize, renderingConfig.RenderType);
            EnableCache = true;
        }

        public RendererControl RenderControl { get; }
        public GameRenderingConfig RenderingConfig { get; }
        public StrategyGameData GameData { get; }
        public IStrategyGameTileSet TileSet { get; }
        public ITileRegistry<TTile> Tiles { get; }
        bool EnableCache { get; }


        public virtual IEnumerable<IPlotOperation> Create(IRenderCallbackFactory<Nothing, TTile> r)
        {
            var agg = new List<TileMatchControl>();
            agg.AddRange(CreateTerrainLayers());
            agg.Add(CreateResourceLayer());
            agg.AddRange(CreateRiverLayer());
            agg.AddRange(CreateRoadLayer());
            agg.Add(CreateImprovementLayer());
            agg.Add(CreateSettlementLayer());
            agg.Add(CreateFogOfWar());

            var retval = new List<IPlotOperation>();
            retval.AddRange(EnableCache
                                ? agg.Select(x => CreateFromMatchControl(x, r))
                                : agg.Select(x => CreateNonCachedFromMatchControl(x, r)));

            return retval;
        }

        IPlotOperation CreateFromMatchControl(TileMatchControl tm, IRenderCallbackFactory<Nothing, TTile> r)
        {
            if (tm.Cachable)
            {
                var cache =
                    PlotOperations.FromContext(RenderingConfig)
                                  .Create(tm.Matcher)
                                  .WithCache()
                                  .ForViewport()
                                  .WithRenderer(r.CreateRenderer<Nothing>(this, Nothing.Instance))
                                  .Build();
                tm.CacheControl(cache);
                return cache;
            }

            return CreateNonCachedFromMatchControl(tm, r);
        }

        IPlotOperation CreateNonCachedFromMatchControl(TileMatchControl tm, IRenderCallbackFactory<Nothing, TTile> r)
        {
            var po = PlotOperations.FromContext(RenderingConfig)
                                   .Create(tm.Matcher)
                                   .ForViewport()
                                   .WithRenderer(r.CreateRenderer<Nothing>(this, Nothing.Instance))
                                   .Build();
            return po;
        }

        TileMatchControl CreateFogOfWar()
        {
            var fogCellMatcher = new FogCellMatcher(GameData.Fog);
            var cellRegistry = new CellMapTileRegistry<TTile>(Tiles);
            var matcher = new CellMapTileSelector<TTile, Nothing>(fogCellMatcher,
                                                                  RenderingConfig.MatcherNavigator,
                                                                  cellRegistry,
                                                                  "t.fog");

            void FogCacheControl(IPlotOperation c)
            {
                GameData.Fog.MapDataChanged += (_, a) => c.Invalidate(a.Coordinate, a.Range + 2);
                RotationCacheControl(c);
            }

            return new TileMatchControl(matcher, FogCacheControl);
        }

        void RotationCacheControl(IPlotOperation c)
        {
            const string pn = nameof(GameRenderingConfig.RotationSteps);
            RenderingConfig.PropertyChanged += (_, a) =>
            {
                if (a.PropertyName == pn)
                {
                    c.InvalidateAll();
                }
            };
        }

        TileMatchControl CreateSettlementLayer()
        {
            var map = GameData.Terrain;

            string Classify(int x, int y)
            {
                var s = map[x, y].City;
                if (s != 0)
                {
                    var settlement = GameData.Settlements[s];
                    var owner = GameData.Players[settlement.Owner];
                    var culture = owner.Culture.ToString().ToLowerInvariant();
                    var walled = settlement.Walled ? "wall" : "city";
                    return $"city.{culture}_{walled}";
                }

                return "";
            }

            var matcher = new DistinctTileMatcher<string, TTile, Nothing>(Classify);
            matcher.Add("city.asian_wall", CreateSettlementMatch("city.asian_wall"));
            matcher.Add("city.asian_city", CreateSettlementMatch("city.asian_city"));
            matcher.Add("city.tropical_wall", CreateSettlementMatch("city.tropical_wall"));
            matcher.Add("city.tropical_city", CreateSettlementMatch("city.tropical_city"));
            matcher.Add("city.celtic_wall", CreateSettlementMatch("city.celtic_wall"));
            matcher.Add("city.celtic_city", CreateSettlementMatch("city.celtic_city"));
            matcher.Add("city.classical_wall", CreateSettlementMatch("city.classical_wall"));
            matcher.Add("city.classical_city", CreateSettlementMatch("city.classical_city"));
            matcher.Add("city.babylonian_wall", CreateSettlementMatch("city.babylonian_wall"));
            matcher.Add("city.babylonian_city", CreateSettlementMatch("city.babylonian_city"));
            return new TileMatchControl(matcher, RotationCacheControl);
        }

        ITileMatcher<TTile, Nothing> CreateSettlementMatch(string prefix)
        {
            var map = GameData.Terrain;

            bool SettlementQuery(int x, int y, out long population)
            {
                var s = map[x, y].City;
                if (s != 0)
                {
                    population = GameData.Settlements[s].Population / 1000;
                    return true;
                }

                population = 0;
                return false;
            }

            var cityTiles = new SortedList<long, TTile>();
            for (var size = 0; size < 5; size += 1)
            {
                if (Tiles.TryFind($"{prefix}_{size}", out var tile))
                {
                    cityTiles.Add(size, tile);
                }
            }

            return new SequenceTileSelector<TTile, Nothing, long>(SettlementQuery, cityTiles);
        }

        TileMatchControl CreateResourceLayer()
        {
            var resourceTypes = GameData.Rules.TerrainResourceTypes;
            var map = GameData.Terrain;

            int Query(int x, int y)
            {
                return map[x, y].Resources;
            }

            bool AlwaysTrue(int x, int y)
            {
                return true;
            }

            var m = new DistinctTileMatcher<int, TTile, Nothing>(Query);
            for (var idx = 0; idx < resourceTypes.Count; idx++)
            {
                var resource = resourceTypes[idx];
                if (resource.GraphicTag != null)
                {
                    m.Add(idx, new BasicTileSelector<TTile, Nothing>(AlwaysTrue,
                                                                     RenderingConfig.MatcherNavigator,
                                                                     Tiles,
                                                                     resource.GraphicTag));
                }
            }

            // alternative ...

            Tuple<TTile, Nothing> LookupTileFromResourceId(int idx)
            {
                var resource = resourceTypes[idx];
                if (Tiles.FindFirstTile(resource, out var retval))
                {
                    return Tuple.Create(retval, Nothing.Instance);
                }

                return null;
            }

            var cachedLookup = new LookupTable<Tuple<TTile, Nothing>>(resourceTypes.Count,
                                                                      LookupTileFromResourceId);

            Tuple<TTile, Nothing> QueryCachedData(int x, int y)
            {
                var data = map[x, y].Resources;
                return cachedLookup.Lookup(data);
            }

            var dm =
                new DirectMappingTileMatcher<Tuple<TTile, Nothing>, TTile, Nothing>(
                    QueryCachedData, LookupTable.UnwrapTuple);
            return new TileMatchControl(dm, RotationCacheControl);
        }

        TileMatchControl CreateImprovementLayer()
        {
            var improvementTypes = GameData.Rules.TerrainImprovementTypes;
            var min = improvementTypes.Where(i => i.DataId >= 0).Select(i => (int?)i.DataId).Min() ?? 0;
            var max = improvementTypes.Where(i => i.DataId >= 0).Select(i => (int?)i.DataId).Max() ?? 0;
            var t = GameData.Terrain;

            uint Query(int x, int y)
            {
                return t[x, y].Improvement;
            }

            bool Resolve(int index, out TTile tile)
            {
                foreach (var improvements in improvementTypes)
                {
                    if (index != improvements.DataId)
                    {
                        continue;
                    }

                    return Tiles.FindFirstTile(improvements, out tile);
                }

                tile = default;
                return false;
            }

            var matcher = new BitVector32Matcher<TTile, Nothing>(min, max, Query, Resolve);
            return new TileMatchControl(matcher, RotationCacheControl);
        }

        GridMatcher CreateTerrainMatcher(Func<int, bool> matcherByTerrainIndex)
        {
            var terrain = GameData.Terrain;

            int TerrainIndexFor(int x, int y)
            {
                return terrain[x, y].TerrainIdx;
            }

            return new MapLookupTable<bool>(GameData.Rules.TerrainTypes.Count,
                                            TerrainIndexFor,
                                            matcherByTerrainIndex).Match;
        }

        IEnumerable<TileMatchControl> CreateRiverLayer()
        {
            var terrain = GameData.Terrain;
            var riverId = GameData.Rules.Roads.River.DataId;

            bool IsRiver(int x, int y)
            {
                return terrain[x, y].Improvement.Read(riverId);
            }

            var isOceanMatcher = CreateTerrainMatcher(
                tidx =>
                    GameData.Rules.TerrainTypes[tidx]
                            .Class
                            .Contains(StrategyGameRules.DefinedTerrains.OceanicClass));

            bool IsRiverOrOcean(int x, int y)
            {
                return IsRiver(x, y) || isOceanMatcher(x, y);
            }

            // river matcher is a simple cardinal matcher.
            var riverBase = new CardinalTileSelector<TTile, Nothing>(
                IsRiverOrOcean, IsRiver,
                RenderingConfig.MatcherNavigator,
                new CardinalTileRegistry<TTile>(Tiles),
                "road.river_s");

            // outlets are rendered on top of ocean tiles.
            var outletMatcher = new RiverOutletTileSelector<TTile, Nothing>
            (IsRiver, isOceanMatcher, RenderingConfig.MatcherNavigator,
             CardinalIndexTileRegistry<TTile>.CreateShort(Tiles), "road.river_outlet");

            return new List<TileMatchControl>
            {
                new TileMatchControl(riverBase, TileMatchControl.CacheControlNoOp),
                new TileMatchControl(outletMatcher, TileMatchControl.CacheControlNoOp)
            };
        }

        IEnumerable<TileMatchControl> CreateRoadLayer()
        {
            var terrain = GameData.Terrain;
            var road = GameData.Rules.Roads.Road.DataId;

            bool IsRoad(int x, int y)
            {
                return terrain[x, y].Improvement.Read(road);
            }

            var roadBase = new SeparateNeighbourTileSelector<TTile, Nothing>(
                IsRoad,
                RenderingConfig.MatcherNavigator,
                NeighbourIndexTileRegistry<TTile>.CreateShort(Tiles),
                "road.road");

            var rwy = GameData.Rules.Roads.Railroad.DataId;

            bool IsRailway(int x, int y)
            {
                return terrain[x, y].Improvement.Read(rwy);
            }

            var railBase = new SeparateNeighbourTileSelector<TTile, Nothing>(
                IsRailway,
                RenderingConfig.MatcherNavigator,
                NeighbourIndexTileRegistry<TTile>.CreateShort(Tiles), "road.rail");

            return new List<TileMatchControl>
            {
                new TileMatchControl(roadBase, TileMatchControl.CacheControlNoOp),
                new TileMatchControl(railBase, TileMatchControl.CacheControlNoOp)
            };
        }

        public ITileMatcher<TTile, Nothing> GenerateCellGroupLayer(int layerIndex,
                                                                   RenderLayerDefinition rd)
        {
            if (rd.MatchWith.Count == 0)
            {
                throw new Exception();
            }

            return new CellMapTileSelector<TTile, Nothing>(cellRendering.CreateMatcher(layerIndex, rd),
                                                           RenderingConfig.MatcherNavigator,
                                                           new CellMapTileRegistry<TTile>(Tiles),
                                                           rd.Tag);
        }

        ITileMatcher<TTile, Nothing> GenerateCornerPairLayer(int layerIndex,
                                                             byte terrainIndex,
                                                             RenderLayerDefinition rd)
        {
            if (rd.MatchWith.Count == 0)
            {
                throw new Exception();
            }

            // binary matching 
            return new CornerTileSelector<TTile, Nothing>(boolRendering.CreateMatcher(layerIndex, rd),
                                                          CreateSelfMatcher(terrainIndex),
                                                          RenderingConfig.MatcherNavigator,
                                                          new CornerTileRegistry<TTile>(Tiles),
                                                          rd.Tag);
        }

        ITileMatcher<TTile, Nothing> GenerateCornerLayer(int layerIndex,
                                                         byte terrainIndex,
                                                         RenderLayerDefinition rd)
        {
            if (rd.MatchWith.Count == 0)
            {
                throw new Exception();
            }

            return new CornerCellSelector<TTile, Nothing>(cellRendering.CreateMatcher(layerIndex, rd),
                                                          CreateSelfMatcher(terrainIndex),
                                                          RenderingConfig.MatcherNavigator,
                                                          new CellMapTileRegistry<TTile>(Tiles),
                                                          rd.Tag);
        }

        GridMatcher CreateSelfMatcher(byte terrainIndex)
        {
            var gameDataTerrain = GameData.Terrain;
            return (x, y) => gameDataTerrain[x, y].TerrainIdx == terrainIndex;
        }

        ITileMatcher<TTile, Nothing> GenerateCardinalLayer(int layerIndex,
                                                           byte terrainIndex,
                                                           RenderLayerDefinition rd)
        {
            if (rd.MatchWith.Count == 0)
            {
                throw new Exception();
            }

            return new CardinalTileSelector<TTile, Nothing>
            (boolRendering.CreateMatcher(layerIndex, rd), CreateSelfMatcher(terrainIndex),
             RenderingConfig.MatcherNavigator,
             new CardinalTileRegistry<TTile>(Tiles), rd.Tag);
        }

        ITileMatcher<TTile, Nothing> GenerateBasicLayer(byte terrainIndex, RenderLayerDefinition rd)
        {
            return new BasicTileSelector<TTile, Nothing>(CreateSelfMatcher(terrainIndex),
                                                         RenderingConfig.MatcherNavigator,
                                                         Tiles, rd.Tag);
        }

        class FogCellMatcher : ICellMatcher
        {
            readonly IFogMap map;
            readonly ITileTagEntrySelection[] states;

            public FogCellMatcher(IFogMap map)
            {
                this.map = map;
                Owner = TileTagEntrySelectionFactory.FromTagsAsSingleCharKey("unknown", "fog", "known");
                states = new[]
                {
                    Owner[0],
                    Owner[1],
                    Owner[2],
                    Owner[2]
                };
            }

            public int Cardinality => Owner.Count;

            public ITileTagEntrySelectionFactory Owner { get; }

            public bool Match(int x, int y, out ITileTagEntrySelection result)
            {
                var state = map[x, y];
                result = states[(int)state];
                return true;
            }
        }

        #region Base Terrains

        List<TileMatchControl> CreateTerrainLayers()
        {
            var ops = new List<TileMatchControl>();

            var layers = TileSet.Layers;
            for (var layerIndex = 0; layerIndex <= layers; layerIndex += 1)
            {
                // if (layerIndex == 3) break;

                var tileMatcher = BuildLayer(layerIndex);
                ops.Add(new TileMatchControl(tileMatcher, RotationCacheControl));
                if (layerIndex == TileSet.BlendLayer)
                {
                    var builder = new StrategyGameBlendLayerBuilder<TTile, TTexture, TColor>(
                        RenderingConfig, tileProducer, textureOperations, GameData, TileSet, Tiles);
                    if (builder.TryBuildBlendLayer(out var blendLayer))
                    {
                        ops.Add(new TileMatchControl(blendLayer, RotationCacheControl));
                    }
                }
            }

            return ops;
        }

        ITileMatcher<TTile, Nothing> BuildLayer(int layerIdx)
        {
            var matcher =
                new DistinctTileMatcher<byte, TTile, Nothing>((x, y) => GameData.Terrain[x, y].TerrainIdx);

            foreach (var t in GameData.Rules.TerrainTypes)
            {
                var tag = TileSet.FindFirstTerrain(t);
                if (tag != null)
                {
                    if (!tag.MatchRule.TryGetValue(layerIdx, out RenderLayerDefinition rd))
                    {
                        continue;
                    }

                    var idx = (byte)GameData.Rules.TerrainTypes.IndexOf(t);
                    ITileMatcher<TTile, Nothing> tileMatcher;
                    switch (rd.MatchType)
                    {
                        case TerrainMatchType.None:
                            tileMatcher = NoOpTileMatcher<TTile, Nothing>.Instance;
                            break;
                        case TerrainMatchType.Basic:
                            tileMatcher = GenerateBasicLayer(idx, rd);
                            break;
                        case TerrainMatchType.Cardinal:
                            tileMatcher = GenerateCardinalLayer(layerIdx, idx, rd);
                            break;
                        case TerrainMatchType.Corner:
                            tileMatcher = GenerateCornerLayer(layerIdx, idx, rd);
                            break;
                        case TerrainMatchType.CornerPair:
                            tileMatcher = GenerateCornerPairLayer(layerIdx, idx, rd);
                            break;
                        case TerrainMatchType.CellGroup:
                            tileMatcher = GenerateCellGroupLayer(layerIdx, rd);
                            break;
                        default:
                            throw new InvalidOperationException();
                    }

                    matcher.Add(idx, tileMatcher);
                }
            }

            return matcher;
        }

        #endregion

        class TileMatchControl : TileMatchControl<TTile, Nothing>
        {
            public TileMatchControl(ITileMatcher<TTile, Nothing> matcher,
                                    Action<IPlotOperation> cacheControl = null) : base(matcher, cacheControl)
            { }
        }
    }
}
