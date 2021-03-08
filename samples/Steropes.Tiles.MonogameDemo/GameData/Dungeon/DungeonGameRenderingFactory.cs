using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Monogame;
using Steropes.Tiles.Monogame.Tiles;
using Steropes.Tiles.MonogameDemo.GameData.Dungeon.Model;
using Steropes.Tiles.MonogameDemo.GameData.Util;
using Steropes.Tiles.Plotter.Operations;
using Steropes.Tiles.Plotter.Operations.Builder;
using Steropes.Tiles.Renderer;
using Steropes.Tiles.Renderer.Graphics;

namespace Steropes.Tiles.MonogameDemo.GameData.Dungeon
{
  public class DungeonGameRenderingFactory
  {
    readonly ITileSet<ITexturedTile> tileSet;
    readonly RendererControl renderControl;
    public Game Game { get; }

    public DungeonGameRenderingFactory(GameRenderingConfig renderingConfig,
                                       DungeonGameData gameData,
                                       ITileSet<ITexturedTile> tileSet,
                                       Game game)
    {
      RenderingConfig = renderingConfig;
      this.GameData = gameData;
      this.tileSet = tileSet;
      this.Game = game;
      this.renderControl = new RendererControl(tileSet.TileSize, renderingConfig.RenderType);
    }

    public GameRenderingConfig RenderingConfig { get; }
    public DungeonGameData GameData { get; }

    protected IRenderCallback<ITexturedTile, TContext> CreateRenderer<TContext>()
    {
      return new MonoGameRenderer<TContext>(Game.Services.GetService<IGraphicsDeviceService>(), renderControl);
    }

    ITileMatcher<ITexturedTile, Nothing> CreateFloorMatcher(ITileRegistry<ITexturedTile> tiles)
    {
      bool Mapper(IFloorType floor, out ITexturedTile x, out Nothing context)
      {
        context = default(Nothing);
        return tiles.TryFind(floor.Name, out x);
      }

      var map = GameData.Map.FloorLayer;
      return new DirectMappingTileMatcher<IFloorType, ITexturedTile, Nothing>((x, y) => map[x, y], Mapper);
    }

    protected static GridMatcher CreateMatcher<T>(IMap2D<T> map, T matchee) where T : class
    {
      return (x, y) => map[x, y] == matchee;
    }

    ITileMatcher<ITexturedTile, Nothing> CreateWallMatcher(ITileRegistry<ITexturedTile> tiles)
    {
      var gd = GameData;
      var map = gd.Map.WallLayer;

      bool IsWallOrPassageFn(int x, int y)
      {
        var tile = map[x, y];
        return tile == gd.Rules.Walls.Stone || tile == gd.Rules.Walls.Passage;
      }

      var wallsAsCardinals = new CardinalTileRegistry<ITexturedTile>(tiles);

      var wallTypeSelector = new DistinctTileMatcher<IWallType, ITexturedTile, Nothing>((x, y) => map[x, y]);
      wallTypeSelector.Add(gd.Rules.Walls.Stone,
                           new CardinalTileSelector<ITexturedTile, Nothing>(IsWallOrPassageFn,
                                                                            CreateMatcher(map, gd.Rules.Walls.Stone),
                                                                            RenderingConfig.MatcherNavigator,
                                                                            wallsAsCardinals,
                                                                            gd.Rules.Walls.Stone.Name));
      return wallTypeSelector;
    }

    ITileMatcher<ITexturedTile, Nothing> CreateDecoMatcher(ITileRegistry<ITexturedTile> tiles)
    {
      var gd = GameData;
      var map = gd.Map.DecorationLayer;
      var wallMap = gd.Map.WallLayer;

      bool IsNeitherWallOrPassageFn(int x, int y)
      {
        var tile = wallMap[x, y];
        return tile != gd.Rules.Walls.Stone && tile != gd.Rules.Walls.Passage;
      }

      var wallsAsCardinals = new CardinalTileRegistry<ITexturedTile>(tiles);

      var wallTypeSelector = new DistinctTileMatcher<IDecorationType, ITexturedTile, Nothing>((x, y) => map[x, y]);

      foreach (var decorationType in gd.Rules.DecorationTypes.Skip(1))
      {
        var target = decorationType;
        wallTypeSelector.Add(decorationType,
                             new CardinalTileSelector<ITexturedTile, Nothing>(IsNeitherWallOrPassageFn,
                                                                              CreateMatcher(map, target),
                                                                              RenderingConfig.MatcherNavigator,
                                                                              wallsAsCardinals, target.Name));
      }

      return wallTypeSelector;
    }

    /// <summary>
    ///  Produces the rendering pipeline for rendering the wall and item layer. 
    /// 
    ///  This operation consists of three tile renderer operations per map coordinate.
    ///  Each operation must be executed as a single batch for each map coordinate 
    ///  so that later tiles can correctly paint over these tiles if needed.
    /// </summary>
    /// <returns></returns>
    IPlotOperation CreateItemLayerPlotOperation(ITileRegistry<ITexturedTile> tiles)
    {

      IRenderPlotOperation<ITexturedTile, Nothing> CreateWallPlotter()
      {
        var matcher = new AggregateTileMatcher<ITexturedTile, Nothing>(CreateWallMatcher(tiles),
                                                                       CreateDecoMatcher(tiles));
        return PlotOperations.FromContext(RenderingConfig)
          .Create(matcher)
          .ForViewport()
          .Build();
      }

      IRenderPlotOperation<ITexturedTile, Nothing> CreateItemPlotter()
      {
        // Selects all items stored at a give map location. 
        // The item will be passed through to the renderer layer as context for post processing.
        var itemMatcher = new ItemListMatcher(GameData, tiles);

        // Take the item context and update the rendered position of the item based on the item's location.
        // This converts the context from IItem to Nothing after adjusting the coordinates.
        var conv = new DungeonGameItemLocationResolver<Nothing>(GameData.ItemService,
                                                                RenderingConfig.Viewport);
        return PlotOperations.FromContext(RenderingConfig).Create(itemMatcher)
          .ForViewport()
          .WithConversion(conv)
          .Build();
      }

      var renderer = new BatchedPositionedSpriteRenderer<ITexturedTile, Nothing>(CreateRenderer<Nothing>());
      var batch = new BatchedPlotOperation<ITexturedTile, Nothing>(renderer,
                                                              CreateWallPlotter(),
                                                              CreateItemPlotter());
      return batch;
    }

    protected IPlotOperation CreatePlot<TContext>(ITileMatcher<ITexturedTile, TContext> matcher)
    {
      var p = PlotOperations.FromContext(RenderingConfig)
        .Create(matcher)
        .WithCache()
        .ForViewport()
        .WithRenderer(CreateRenderer<TContext>());
      return p.Build();
    }

    public GameRendering Create()
    {
      var tiles = tileSet.LoadTexturePack(new MonogameContentLoader(Game.Content));

      var r = new GameRendering(Game, RenderingConfig, renderControl);
      r.AddLayer(CreatePlot(CreateFloorMatcher(tiles)));
      r.AddLayer(CreateItemLayerPlotOperation(tiles));
      return r;
    }
  }
}