using System;
using System.Collections.Generic;
using System.Linq;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Demo.Core.GameData.Dungeon;
using Steropes.Tiles.Demo.Core.GameData.Dungeon.Model;
using Steropes.Tiles.Demo.Core.GameData.Util;
using Steropes.Tiles.Matcher;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Plotter.Operations;
using Steropes.Tiles.Plotter.Operations.Builder;
using Steropes.Tiles.Renderer.Graphics;
using Steropes.Tiles.Sample.Shared;
using Steropes.Tiles.Unit2D.Demo.GameData.Dungeon;

namespace Steropes.Tiles.Unit2D.Demo.Components.Dungeon
{
    public class DungeonGameRenderingFactory<TTile> : RenderingFactoryBase<TTile>
    {
        public DungeonGameRenderingFactory(GameRenderingConfig renderConfig,
                                           DungeonGameData gameData,
                                           ITileSet<TTile> tileSet) : base(renderConfig, tileSet)
        {
            this.GameData = gameData;
        }

        public DungeonGameData GameData { get; }

        ITileMatcher<TTile, Nothing> CreateFloorMatcher(ITileRegistry<TTile> tiles)
        {
            bool Mapper(IFloorType floor, out TTile x, out Nothing context)
            {
                context = default(Nothing);
                return tiles.TryFind(floor.Name, out x);
            }

            var map = GameData.Map.FloorLayer;
            return new DirectMappingTileMatcher<IFloorType, TTile, Nothing>((x, y) => map[x, y], Mapper);
        }

        protected static GridMatcher CreateMatcher<T>(IMap2D<T> map, T matchee)
            where T : IEquatable<T>
        {
            return (x, y) => map[x, y].Equals(matchee);
        }

        ITileMatcher<TTile, Nothing> CreateWallMatcher(ITileRegistry<TTile> tiles)
        {
            var gd = GameData;
            var map = gd.Map.WallLayer;

            bool IsWallOrPassageFn(int x, int y)
            {
                var tile = map[x, y];
                return gd.Rules.Walls.Stone.Equals(tile) || gd.Rules.Walls.Passage.Equals(tile);
            }

            var wallsAsCardinals = new CardinalTileRegistry<TTile>(tiles);

            var wallTypeSelector = new DistinctTileMatcher<IWallType, TTile, Nothing>((x, y) => map[x, y]);
            wallTypeSelector.Add(gd.Rules.Walls.Stone,
                                 new CardinalTileSelector<TTile, Nothing>(IsWallOrPassageFn,
                                                                          CreateMatcher(map, gd.Rules.Walls.Stone),
                                                                          RenderingConfig.MatcherNavigator,
                                                                          wallsAsCardinals,
                                                                          gd.Rules.Walls.Stone.Name));
            return wallTypeSelector;
        }

        ITileMatcher<TTile, Nothing> CreateDecoMatcher(ITileRegistry<TTile> tiles)
        {
            var gd = GameData;
            var map = gd.Map.DecorationLayer;
            var wallMap = gd.Map.WallLayer;

            bool IsNeitherWallOrPassageFn(int x, int y)
            {
                var tile = wallMap[x, y];
                return !gd.Rules.Walls.Stone.Equals(tile) && !gd.Rules.Walls.Passage.Equals(tile);
            }

            var wallsAsCardinals = new CardinalTileRegistry<TTile>(tiles);

            var wallTypeSelector =
                new DistinctTileMatcher<IDecorationType, TTile, Nothing>((x, y) => map[x, y]);

            foreach (var decorationType in gd.Rules.DecorationTypes.Skip(1))
            {
                var target = decorationType;
                wallTypeSelector.Add(decorationType,
                                     new CardinalTileSelector<TTile, Nothing>(IsNeitherWallOrPassageFn,
                                                                              CreateMatcher(map, target),
                                                                              RenderingConfig.MatcherNavigator,
                                                                              wallsAsCardinals, target.Name));
            }

            return wallTypeSelector;
        }

        /// <summary>
        ///  Produces the rendering pipeline for rendering the wall and item layer. 
        /// 
        ///  This operation consists of three tile renderer operations per map
        ///  coordinate. Each operation must be executed as a single batch for each
        ///  map coordinate  so that later tiles can correctly paint over these
        ///  tiles if needed.
        /// </summary>
        /// <returns></returns>
        IPlotOperation CreateItemLayerPlotOperation<TRenderParameter>(IRenderCallbackFactory<TRenderParameter, TTile> rendererFactory,
                                                                      ITileRegistry<TTile> tiles,
                                                                      TRenderParameter sortingLayer)
        {
            IRenderPlotOperation<TTile, Nothing> CreateWallPlotter()
            {
                var matcher = new AggregateTileMatcher<TTile, Nothing>
                    (CreateWallMatcher(tiles), CreateDecoMatcher(tiles));
                return PlotOperations.FromContext(RenderingConfig)
                                     .Create(matcher)
                                     .WithCache()
                                     .ForViewport()
                                     .Build();
            }

            IRenderPlotOperation<TTile, Nothing> CreateItemPlotter()
            {
                // Selects all items stored at a give map location. 
                // The item will be passed through to the renderer layer as context for post processing.
                var itemMatcher = new ItemListMatcher<TTile>(GameData, tiles);

                // Take the item context and update the rendered position of the item based on the item's location.
                // This converts the context from IItem to Nothing after adjusting the coordinates.
                var conv = new DungeonGameItemLocationResolver<TTile, Nothing>(GameData.ItemService,
                                                                               RenderingConfig.Viewport);
                return PlotOperations.FromContext(RenderingConfig)
                                     .Create(itemMatcher)
                                     .ForViewport()
                                     .WithConversion(conv)
                                     .Build();
            }

            var renderer = new BatchedPositionedSpriteRenderer<TTile, Nothing>(rendererFactory.CreateRenderer<Nothing>(this, sortingLayer));
            var batch = new BatchedPlotOperation<TTile, Nothing>(renderer,
                                                                 CreateWallPlotter(), CreateItemPlotter());
            return batch;
        }

        protected IPlotOperation CreatePlot<TContext, TRenderParameter>(IRenderCallbackFactory<TRenderParameter, TTile> renderer,
                                                                        ITileMatcher<TTile, TContext> matcher,
                                                                        TRenderParameter sortingLayer)
        {
            var p = PlotOperations.FromContext(RenderingConfig)
                                  .Create(matcher)
                                  .WithCache()
                                  .ForViewport()
                                  .WithRenderer(renderer.CreateRenderer<TContext>(this, sortingLayer));
            return p.Build();
        }

        public IEnumerable<IPlotOperation> Create<TRenderParameter>(IRenderCallbackFactory<TRenderParameter, TTile> renderer,
                                                                    TRenderParameter floorLayer,
                                                                    TRenderParameter itemLayer)
        {
            var r = new List<IPlotOperation>
            {
                CreatePlot(renderer, CreateFloorMatcher(Tiles), floorLayer),
                CreateItemLayerPlotOperation(renderer, Tiles, itemLayer)
            };
            return r;
        }
    }
}
