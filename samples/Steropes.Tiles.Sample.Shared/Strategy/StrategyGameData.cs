using Steropes.Tiles.Navigation;
using Steropes.Tiles.Sample.Shared.Strategy.Model;
using Steropes.Tiles.Sample.Shared.Util;
using System;
using System.Collections.Generic;
using System.IO;
using TerrainData = Steropes.Tiles.Sample.Shared.Strategy.Model.TerrainData;

namespace Steropes.Tiles.Sample.Shared.Strategy
{
    public class StrategyGameData
    {
        readonly TerrainMap terrain;
        MapCoordinate mousePosition;

        public StrategyGameData()
        {
            Rules = new StrategyGameRules();
            TerrainHeight = 300;
            TerrainWidth = 300;
            terrain = new TerrainMap(TerrainWidth, TerrainHeight);
            Fog = new WrappingFogMap(new FogMap(TerrainWidth, TerrainHeight), GridNavigation.CreateNavigator(RenderType.Grid).Wrap(TerrainWidth, TerrainHeight));

            Settlements = new SettlementManager();

            Players = new List<Player>
            {
                new Player("Barbarian", PlayerColor.Red, Culture.Celtic),
                new Player("Player A", PlayerColor.Green, Culture.Classical),
                new Player("Player B", PlayerColor.Blue, Culture.Celtic)
            };

            var r = new MapReader(Rules);
            r.Map = terrain;
            r.ReadTerrain(new StringReader(MapData.TerrainData), 0, 0, TerrainWidth, TerrainHeight);
            r.ReadRoads(new StringReader(MapData.RoadData), 0, 0, TerrainWidth, TerrainHeight);
            r.ReadRivers(new StringReader(MapData.RiverData), 0, 0, TerrainWidth, TerrainHeight);
            r.ReadImprovement(new StringReader(MapData.ImprovementData), 0, 0, TerrainWidth, TerrainHeight);
            r.ReadResources(new StringReader(MapData.ResourceData), 0, 0, TerrainWidth, TerrainHeight);

            AddSettlement(new Settlement("Capital City", new MapCoordinate(7, 13), 1, 4000, true));
            AddSettlement(new Settlement("Satellite Settlement", new MapCoordinate(20, 14), 2, 2000, false));

            Console.WriteLine(MapData.RiverData);
            Console.WriteLine(terrain.Print(t => t.Improvement.HasExtra(Rules.Roads.River) ? 'x' : ' '));

            // Explicitly set the initial visible tracker to the location of one of the cities 
            // here so that the visibility marking works correctly.
            Fog.MarkRangeVisible(0, 0, 2);
            MousePosition = new MapCoordinate(7, 13);
        }

        public List<Player> Players { get; }
        public SettlementManager Settlements { get; }

        public int TerrainWidth { get; }
        public int TerrainHeight { get; }
        public IFogMap Fog { get; }

        public StrategyGameRules Rules { get; }

        public MapCoordinate MousePosition
        {
            get { return mousePosition; }
            set
            {
                if (mousePosition == value)
                {
                    return;
                }

                Fog.MarkRangeInvisible(mousePosition.X, mousePosition.Y, 2);
                mousePosition = value;
                Fog.MarkRangeVisible(mousePosition.X, mousePosition.Y, 2);
                Fog.MarkRangeExplored(mousePosition.X, mousePosition.Y, 2);
            }
        }

        public IMap2D<TerrainData> Terrain
        {
            get { return terrain; }
        }

        public void AddSettlement(Settlement s)
        {
            var idx = Settlements.AddSettlement(s);

            var t = terrain[s.Location.X, s.Location.Y];
            t.City = (byte)idx;
            terrain[s.Location.X, s.Location.Y] = t;

            Fog.MarkRangeExplored(s.Location.X, s.Location.Y, 2);
            Fog.MarkRangeVisible(s.Location.X, s.Location.Y, 2);
        }

        /// <summary>
        ///   A holder of all settlements for all players. This is a 1-based list,
        ///   the id of zero is reserved as "no city" marker.
        /// </summary>
        public class SettlementManager
        {
            readonly List<Settlement> settlements;

            public SettlementManager()
            {
                settlements = new List<Settlement>();
            }

            public ISettlement this[int index]
            {
                get
                {
                    if (index == 0)
                    {
                        // no settlement marker.
                        return null;
                    }

                    return settlements[index - 1];
                }
            }

            public int Count => settlements.Count;

            internal int AddSettlement(Settlement s)
            {
                if (s.Owner == 0)
                {
                    throw new ArgumentException();
                }

                if (settlements.Count == 254)
                {
                    throw new ArgumentException();
                }

                settlements.Add(s);
                return settlements.Count;
            }
        }
    }
}
