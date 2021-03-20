using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Demo.Game;
using Steropes.Tiles.Demo.TextMode;
using Steropes.Tiles.Matcher;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.Plotter;
using Steropes.Tiles.Plotter.Operations;
using Steropes.Tiles.Renderer;

namespace Steropes.Tiles.Demo
{
    public class MapGame
    {
        object Old(TerrainTypes terrainTypes, TerrainMap map)
        {
            var desert = new TextTile("Desert", '.', ConsoleColor.DarkYellow);
            var tileSet = new Dictionary<TerrainType, TextTile>
            {
                {terrainTypes.Grasland, new TextTile("Gras", 'g', ConsoleColor.DarkGreen)},
                {terrainTypes.Desert, desert},
                {terrainTypes.Hills, new TextTile("Hills", 'h', ConsoleColor.Gray)},
                {terrainTypes.Plains, new TextTile("Plains", '_', ConsoleColor.Green)}
            };

            bool Mapper(TerrainType key, out TextTile result, out Nothing context)
            {
                context = Nothing.Instance;
                return tileSet.TryGetValue(key, out result);
            }

            return new DirectMappingTileMatcher<TerrainType, TextTile, Nothing>(new TerrainLayer(map, terrainTypes).Read, Mapper);
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Starting");
            var terrainTypes = new TerrainTypes();
            var map = TerrainMap.CreateMap(terrainTypes);

            var renderType = RenderType.Grid;
            var navigator = new LimitedRangeNavigator<GridDirection>(GridNavigation.CreateNavigator(renderType), map.Width, map.Height);

            var desert = new TextTile("Desert", '.', ConsoleColor.DarkYellow);
            var tileRegistry = new BasicTileRegistry<TextTile>()
            {
                {"terrain.grasland", new TextTile("Gras", 'g', ConsoleColor.DarkGreen)},
                {"terrain.desert", desert},
                {"terrain.hills", new TextTile("Hills", 'h', ConsoleColor.Gray)},
                {"terrain.plains", new TextTile("Plains", '_', ConsoleColor.Green)}
            };

            var list = new List<ITileMatcher<TextTile, Nothing>>
            {
                new BasicTileSelector<TextTile, Nothing>(
                    (x, y) => map[x, y].Terrain == terrainTypes.Desert,
                    navigator,
                    tileRegistry,
                    "terrain.desert"),
                new BasicTileSelector<TextTile, Nothing>(
                    (x, y) => map[x, y].Terrain == terrainTypes.Grasland,
                    navigator,
                    tileRegistry,
                    "terrain.grasland"),
                new BasicTileSelector<TextTile, Nothing>(
                    (x, y) => map[x, y].Terrain == terrainTypes.Hills,
                    navigator,
                    tileRegistry,
                    "terrain.hills"),
                new BasicTileSelector<TextTile, Nothing>(
                    (x, y) => map[x, y].Terrain == terrainTypes.Plains,
                    navigator,
                    tileRegistry,
                    "terrain.plains")
            };

            var bMatcher = new AggregateTileMatcher<TextTile, Nothing>(list);

            var viewport = new MapViewport(renderType)
            {
                SizeInTiles = new IntDimension(20, 20),
                CenterPoint = new ContinuousViewportCoordinates(0, 0)
            };

            var consoleRenderer = new ViewportRenderer<TextTile, Nothing>(viewport)
            {
                RenderTarget = new ConsoleRenderer()
            };
            var plotOp = new PlotOperation<TextTile, Nothing>(bMatcher, renderType, consoleRenderer);
            var t = new GridPlotter(viewport, navigator);

            do
            {
                t.Draw(plotOp);
                var consoleKeyInfo = Console.ReadKey(true);
                if (consoleKeyInfo.Key == ConsoleKey.Escape || consoleKeyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }

                var center = viewport.CenterPoint;
                if (consoleKeyInfo.Key == ConsoleKey.LeftArrow)
                {
                    center.X -= 1;
                }

                if (consoleKeyInfo.Key == ConsoleKey.RightArrow)
                {
                    center.X += 1;
                }

                if (consoleKeyInfo.Key == ConsoleKey.UpArrow)
                {
                    center.Y -= 1;
                }

                if (consoleKeyInfo.Key == ConsoleKey.DownArrow)
                {
                    center.Y += 1;
                }

                viewport.CenterPoint = center;
            } while (true);
        }

        class TerrainLayer
        {
            readonly TerrainMap map;
            readonly TerrainTypes types;

            public TerrainLayer(TerrainMap map, TerrainTypes types)
            {
                this.map = map;
                this.types = types;
            }

            public TerrainType Read(int x, int y)
            {
                if (x < 0 || x >= map.Width)
                {
                    return types.Desert;
                }

                if (y < 0 || y >= map.Height)
                {
                    return types.Desert;
                }

                return map[x, y].Terrain;
            }
        }
    }
}
