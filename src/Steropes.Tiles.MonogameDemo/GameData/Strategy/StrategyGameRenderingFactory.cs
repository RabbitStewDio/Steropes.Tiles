using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Matcher.TileTags;
using Steropes.Tiles.Monogame;
using Steropes.Tiles.Monogame.Tiles;
using Steropes.Tiles.MonogameDemo.GameData.Strategy.Model;
using Steropes.Tiles.MonogameDemo.GameData.Strategy.Rendering;
using Steropes.Tiles.MonogameDemo.GameData.Util;
using Steropes.Tiles.Plotter.Operations;
using Steropes.Tiles.Plotter.Operations.Builder;
using Steropes.Tiles.Renderer;
using Steropes.UI.Widgets.Container;

namespace Steropes.Tiles.MonogameDemo.GameData.Strategy
{
  /// <summary>
  ///   A Civ style rendering system.
  ///   <code>
  ///   A fully fledged system would use the following rendering layers:
  /// 
  ///    1. Background (not part of this renderer, the caller is responsible for clearing the screen)
  ///    2. Terrain: Configurable
  ///       a. Basic Terrain (3 layers max)
  ///       b. Water
  ///       c. Roads (after water to draw roads as bridges)
  ///    3. Terrain: Fixed
  ///       a. Grid - Terrain grid
  ///       b. Grid - Country Borders
  ///       c. Grid - Trade routes
  ///       d. Specials - low level improvements. Mines, farms, fortification 1st layer
  ///       e. Resource icons (iron, game, etc)
  ///       f. City Sprites
  ///       g. Specials - fortifications, 2nd layer
  ///    4. Fog
  ///    5. Units: All units, except for the focused one.
  ///    6. Terain: Fixed 2
  ///       a. Grid - City influence area overlay
  ///       b. Special3 - Fortification, 3rd layer, also : City - fortifications
  ///       c. City Overlays (riots)
  ///    7. Labels:
  ///       a. Unit flags and names
  ///       b. City size label
  ///       c. Titlelabel (city label)
  ///       d. Citybar (city label)
  ///       e. Worker task (a small icon indicating the unit's activity)
  ///    8. Focused unit
  ///    9. Focused Unit flags and names
  ///   10. User input
  ///   </code>
  /// </summary>
  public class StrategyGameRenderingFactory : ITileRenderModeContext
  {
    readonly BoolRenderingFactory boolRendering;
    readonly TagSetRenderingFactory cellRendering;
    readonly RendererControl renderControl;

    public StrategyGameRenderingFactory(GameRenderingConfig renderingConfig,
                                        StrategyGameData gameData,
                                        StrategyGameTileSet tileSet,
                                        Game game)
    {
      RenderingConfig = renderingConfig ?? throw new ArgumentNullException(nameof(RenderingConfig));
      GameData = gameData ?? throw new ArgumentNullException(nameof(GameData));
      TileSet = tileSet ?? throw new ArgumentNullException(nameof(TileSet));
      Game = game ?? throw new ArgumentNullException(nameof(Game));

      TileRegistry =
        tileSet.Textures.LoadTexturePack(new MonogameContentLoader(Game.Content, tileSet.Textures.BasePath));

      cellRendering = new TagSetRenderingFactory(this);
      boolRendering = new BoolRenderingFactory(this);
      renderControl = new RendererControl(tileSet.TileSize, renderingConfig.RenderType);
      EnableCache = true;
    }

    public Game Game { get; }

    public GameRenderingConfig RenderingConfig { get; }
    public StrategyGameData GameData { get; }
    public IStrategyGameTileSet TileSet { get; }
    public ITileRegistry<ITexturedTile> TileRegistry { get; }
    bool EnableCache { get; }

    public GameRendering Create(Group g)
    {
      var r = new GameRendering(Game, RenderingConfig, renderControl);

      var agg = new List<TileMatchControl>();
      agg.AddRange(CreateTerrainLayers());
      agg.Add(CreateResourceLayer());
      agg.AddRange(CreateRiverLayer());
      agg.AddRange(CreateRoadLayer());
      agg.Add(CreateImprovementLayer());
      agg.Add(CreateSettlementLayer());
      agg.Add(CreateFogOfWar());

      foreach (var tm in agg)
      {
        if (tm.Cachable && EnableCache)
        {
          var cache =
            PlotOperations.FromContext(RenderingConfig).Create(tm.Matcher)
              .WithCache()
              .ForViewport()
              .WithRenderer(CreateBaseRenderer<Nothing>())
              .Build();
          tm.CacheControl(cache);
          r.AddLayer(cache);
        }
        else
        {
          var po = PlotOperations.FromContext(RenderingConfig)
            .Create(tm.Matcher)
            .ForViewport()
            .WithRenderer(CreateBaseRenderer<Nothing>())
            .Build();
          r.AddLayer(po);
        }
      }

      r.AddLayer(CreateCityBarRenderer(g));

      return r;
    }

    IPlotOperation CreateCityBarRenderer(Group parent)
    {
      var map = GameData.Terrain;

      bool CityMapper(byte index, out ISettlement s, out Nothing context)
      {
        if (index == 0)
        {
          s = null;
          context = null;
          return false;
        }

        s = GameData.Settlements[index];
        context = null;
        return true;
      }

      var settlementMatcher = 
        new DirectMappingTileMatcher<byte, ISettlement, Nothing>((x, y) => map[x, y].City, CityMapper);

      return PlotOperations.FromContext(RenderingConfig)
        .Create(settlementMatcher)
        .ForViewport()
        .WithRenderer(new CityBarRenderer(parent, renderControl))
        .Build();
    }

    TileMatchControl CreateFogOfWar()
    {
      var fogCellMatcher = new FogCellMatcher(GameData.Fog);
      var cellRegistry = new CellMapTileRegistry<ITexturedTile>(TileRegistry);
      var matcher = new CellMapTileSelector<ITexturedTile, Nothing>(fogCellMatcher,
                                                                    RenderingConfig.MatcherNavigator,
                                                                    cellRegistry,
                                                                    "t.fog");

      void FogCacheControl(IPlotOperation c)
      {
        GameData.Fog.MapDataChanged += (s, a) => c.Invalidate(a.Coordinate, a.Range + 2);
        RotationCacheControl(c);
      }

      return new TileMatchControl(matcher, FogCacheControl);
    }

    void RotationCacheControl(IPlotOperation c)
    {
      const string pn = nameof(GameRenderingConfig.RotationSteps);
      RenderingConfig.PropertyChanged += (s, a) =>
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

      var matcher = new DistinctTileMatcher<string, ITexturedTile, Nothing>(Classify);
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

    ITileMatcher<ITexturedTile, Nothing> CreateSettlementMatch(string prefix)
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

      var cityTiles = new SortedList<long, ITexturedTile>();
      for (var size = 0; size < 5; size += 1)
      {
        if (TileRegistry.TryFind($"{prefix}_{size}", out ITexturedTile tile))
        {
          cityTiles.Add(size, tile);
        }
      }

      return new SequenceTileSelector<ITexturedTile, Nothing, long>(SettlementQuery, cityTiles);
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

      var m = new DistinctTileMatcher<int, ITexturedTile, Nothing>(Query);
      for (var idx = 0; idx < resourceTypes.Count; idx++)
      {
        var resource = resourceTypes[idx];
        if (resource.GraphicTag != null)
        {
          m.Add(idx, new BasicTileSelector<ITexturedTile, Nothing>(AlwaysTrue,
                                                                   RenderingConfig.MatcherNavigator,
                                                                   TileRegistry,
                                                                   resource.GraphicTag));
        }
      }

      // alternative ...

      Tuple<ITexturedTile, Nothing> LookupTileFromResourceId(int idx)
      {
        var resource = resourceTypes[idx];
        if (TileRegistry.FindFirstTile(resource, out ITexturedTile retval))
        {
          return Tuple.Create(retval, Nothing.Instance);
        }

        return null;
      }

      var cachedLookup = new LookupTable<Tuple<ITexturedTile, Nothing>>(resourceTypes.Count,
                                                                        LookupTileFromResourceId);

      Tuple<ITexturedTile, Nothing> QueryCachedData(int x, int y)
      {
        var data = map[x, y].Resources;
        return cachedLookup.Lookup(data);
      }

      var dm =
        new DirectMappingTileMatcher<Tuple<ITexturedTile, Nothing>, ITexturedTile, Nothing>(
          QueryCachedData, LookupTable.UnwrapTuple);
      return new TileMatchControl(dm, RotationCacheControl);
    }

    TileMatchControl CreateImprovementLayer()
    {
      var improvementTypes = GameData.Rules.TerrainImprovementTypes;
      var min = improvementTypes.Where(i => i.DataId >= 0).Select(i => (int?) i.DataId).Min() ?? 0;
      var max = improvementTypes.Where(i => i.DataId >= 0).Select(i => (int?) i.DataId).Max() ?? 0;
      var t = GameData.Terrain;

      uint Query(int x, int y)
      {
        return t[x, y].Improvement;
      }

      ITexturedTile Resolve(int index)
      {
        return improvementTypes.Where(i => i.DataId == index).Select(imp =>
        {
          if (TileRegistry.FindFirstTile(imp, out ITexturedTile tile))
          {
            return tile;
          }

          TileRegistryTracing.EmitMissingTileWarning(imp.GraphicTag, imp.AlternativeGraphicTags);
          return null;
        }).FirstOrDefault();
      }

      var matcher = new BitVector32Matcher<ITexturedTile, Nothing>(min, max, Query, Resolve);
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
                                      matcherByTerrainIndex, false).Match;
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
      var riverBase = new CardinalTileSelector<ITexturedTile, Nothing>(
        IsRiverOrOcean, IsRiver,
        RenderingConfig.MatcherNavigator,
        new CardinalTileRegistry<ITexturedTile>(TileRegistry),
        "road.river_s");

      // outlets are rendered on top of ocean tiles.
      var outletMatcher = new RiverOutletTileSelector<ITexturedTile, Nothing>
      (IsRiver, isOceanMatcher, RenderingConfig.MatcherNavigator,
       CardinalIndexTileRegistry<ITexturedTile>.CreateShort(TileRegistry), "road.river_outlet");

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

      var roadBase = new SeparateNeighbourTileSelector<ITexturedTile, Nothing>(
        IsRoad,
        RenderingConfig.MatcherNavigator,
        NeighbourIndexTileRegistry<ITexturedTile>.CreateShort(TileRegistry),
        "road.road");

      var rwy = GameData.Rules.Roads.Railroad.DataId;

      bool IsRailway(int x, int y)
      {
        return terrain[x, y].Improvement.Read(rwy);
      }

      var railBase = new SeparateNeighbourTileSelector<ITexturedTile, Nothing>(
        IsRailway,
        RenderingConfig.MatcherNavigator,
        NeighbourIndexTileRegistry<ITexturedTile>.CreateShort(TileRegistry), "road.rail");

      return new List<TileMatchControl>
      {
        new TileMatchControl(roadBase, TileMatchControl.CacheControlNoOp),
        new TileMatchControl(railBase, TileMatchControl.CacheControlNoOp)
      };
    }

    public ITileMatcher<ITexturedTile, Nothing> GenerateCellGroupLayer(int layerIndex,
                                                                       RenderLayerDefinition rd)
    {
      if (rd.MatchWith.Count == 0)
      {
        throw new Exception();
      }

      return new CellMapTileSelector<ITexturedTile, Nothing>(cellRendering.CreateMatcher(layerIndex, rd),
                                                             RenderingConfig.MatcherNavigator,
                                                             new CellMapTileRegistry<ITexturedTile>(TileRegistry),
                                                             rd.Tag);
    }

    ITileMatcher<ITexturedTile, Nothing> GenerateCornerPairLayer(int layerIndex,
                                                                 byte terrainIndex,
                                                                 RenderLayerDefinition rd)
    {
      if (rd.MatchWith.Count == 0)
      {
        throw new Exception();
      }

      // binary matching 
      return new CornerTileSelector<ITexturedTile, Nothing>(boolRendering.CreateMatcher(layerIndex, rd),
                                                            CreateSelfMatcher(terrainIndex),
                                                            RenderingConfig.MatcherNavigator,
                                                            new CornerTileRegistry<ITexturedTile>(TileRegistry),
                                                            rd.Tag);
    }

    ITileMatcher<ITexturedTile, Nothing> GenerateCornerLayer(int layerIndex,
                                                             byte terrainIndex,
                                                             RenderLayerDefinition rd)
    {
      if (rd.MatchWith.Count == 0)
      {
        throw new Exception();
      }

      return new CornerCellSelector<ITexturedTile, Nothing>(cellRendering.CreateMatcher(layerIndex, rd),
                                                            CreateSelfMatcher(terrainIndex),
                                                            RenderingConfig.MatcherNavigator,
                                                            new CellMapTileRegistry<ITexturedTile>(TileRegistry),
                                                            rd.Tag);
    }

    GridMatcher CreateSelfMatcher(byte terrainIndex)
    {
      var gameDataTerrain = GameData.Terrain;
      return (x, y) => gameDataTerrain[x, y].TerrainIdx == terrainIndex;
    }

    ITileMatcher<ITexturedTile, Nothing> GenerateCardinalLayer(int layerIndex,
                                                               byte terrainIndex,
                                                               RenderLayerDefinition rd)
    {
      if (rd.MatchWith.Count == 0)
      {
        throw new Exception();
      }

      return new CardinalTileSelector<ITexturedTile, Nothing>
      (boolRendering.CreateMatcher(layerIndex, rd), CreateSelfMatcher(terrainIndex),
       RenderingConfig.MatcherNavigator,
       new CardinalTileRegistry<ITexturedTile>(TileRegistry), rd.Tag);
    }

    ITileMatcher<ITexturedTile, Nothing> GenerateBasicLayer(byte terrainIndex, RenderLayerDefinition rd)
    {
      return new BasicTileSelector<ITexturedTile, Nothing>(CreateSelfMatcher(terrainIndex),
                                                           RenderingConfig.MatcherNavigator,
                                                           TileRegistry, rd.Tag);
    }

    struct TileMatchControl
    {
      public bool Cachable { get; }
      public ITileMatcher<ITexturedTile, Nothing> Matcher { get; }
      public Action<IPlotOperation> CacheControl { get; }

      public TileMatchControl(ITileMatcher<ITexturedTile, Nothing> matcher,
                              Action<IPlotOperation> cacheControl)
      {
        Matcher = matcher ?? throw new ArgumentNullException(nameof(matcher));
        Cachable = true;
        CacheControl = cacheControl ?? throw new ArgumentNullException(nameof(cacheControl));
      }

      public static void CacheControlNoOp(IPlotOperation op)
      {
      }
    }

    class FogCellMatcher : ICellMatcher
    {
      readonly IFogMap map;
      readonly ITileTagEntrySelection[] states;

      public FogCellMatcher(IFogMap map)
      {
        this.map = map;
        Owner = TileTagEntrySelectionFactory.FromTags("unknown", "fog", "known");
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
        result = states[(int) state];
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
        var tileMatcher = BuildLayer(layerIndex);
        ops.Add(new TileMatchControl(tileMatcher, RotationCacheControl));
        if (layerIndex == TileSet.BlendLayer)
        {
          var builder = new StrategyGameBlendLayerBuilder(RenderingConfig, GameData, TileSet, TileRegistry);
          var blendLayer = builder.BuildBlendLayer(Game.GraphicsDevice);
          ops.Add(new TileMatchControl(blendLayer, RotationCacheControl));
        }
      }

      return ops;
    }

    protected IRenderCallback<ITexturedTile, TContext> CreateBaseRenderer<TContext>()
    {
      return new MonoGameRenderer<TContext>(Game.Services.GetService<IGraphicsDeviceService>(), renderControl);
    }

    ITileMatcher<ITexturedTile, Nothing> BuildLayer(int layerIdx)
    {
      var matcher = new DistinctTileMatcher<byte, ITexturedTile, Nothing>((x, y) => GameData.Terrain[x, y].TerrainIdx);

      foreach (var t in GameData.Rules.TerrainTypes)
      {
        var tag = TileSet.FindFirstTerrain(t);
        if (tag != null)
        {
          if (!tag.MatchRule.TryGetValue(layerIdx, out RenderLayerDefinition rd))
          {
            continue;
          }

          var idx = (byte) GameData.Rules.TerrainTypes.IndexOf(t);
          ITileMatcher<ITexturedTile, Nothing> tileMatcher;
          switch (rd.MatchType)
          {
            case TerrainMatchType.None:
              tileMatcher = NoOpTileMatcher<ITexturedTile, Nothing>.Instance;
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
  }
}