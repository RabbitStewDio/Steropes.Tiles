using System;
using System.Collections.Generic;
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
                    center += new ContinuousViewportCoordinates(-1, 0);
                }

                if (consoleKeyInfo.Key == ConsoleKey.RightArrow)
                {
                    center += new ContinuousViewportCoordinates(+1, 0);
                }

                if (consoleKeyInfo.Key == ConsoleKey.UpArrow)
                {
                    center += new ContinuousViewportCoordinates(0, -1);
                }

                if (consoleKeyInfo.Key == ConsoleKey.DownArrow)
                {
                    center += new ContinuousViewportCoordinates(0, +1);
                }

                viewport.CenterPoint = center;
            } while (true);
        }
    }
}
