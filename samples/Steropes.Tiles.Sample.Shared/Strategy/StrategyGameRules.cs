using Steropes.Tiles.Sample.Shared.Strategy.Model;
using Steropes.Tiles.Sample.Shared.Util;
using static Steropes.Tiles.Sample.Shared.Strategy.Model.ResourcesExt;

namespace Steropes.Tiles.Sample.Shared.Strategy
{
    public class StrategyGameRules
    {
        public StrategyGameRules()
        {
            Terrains = new DefinedTerrains();
            TerrainTypes = TypeRegistry.CreateFrom(Terrains, t => t.Nothing);

            TerrainResources = new DefinedResources();
            TerrainResourceTypes = TypeRegistry.CreateFrom(TerrainResources, r => r.None);

            var idGen = new IdGenerator();

            Roads = new DefinedRoads(idGen);
            RoadTypes = TypeRegistry.CreateFrom(Roads.Nothing, Roads.Road, Roads.Railroad, Roads.River);

            TerrainImprovements = new DefinedImprovements(idGen);
            TerrainImprovementTypes = TypeRegistry.CreateFrom(TerrainImprovements, i => i.None);

            MoveFragments = 3;
        }

        public TypeRegistry<Improvements> TerrainImprovementTypes { get; }

        public DefinedImprovements TerrainImprovements { get; }

        public TypeRegistry<ITerrainResource> TerrainResourceTypes { get; }

        public DefinedResources TerrainResources { get; }

        public int MoveFragments { get; }

        public ITypeRegistry<IRoadType> RoadTypes { get; }

        public DefinedRoads Roads { get; }

        public ITypeRegistry<ITerrain> TerrainTypes { get; }

        public DefinedTerrains Terrains { get; }

        public class DefinedImprovements
        {
            public DefinedImprovements(IdGenerator idGen)
            {
                None = new Improvements(-1, "", ' ', "None", null, null);
                Farms = new Improvements(idGen.Next(), "farms", 'f', "Farms", null, "tx.farmland");
                Mining = new Improvements(idGen.Next(), "mine", 'm', "Mine", null, "tx.mine");
            }

            public Improvements None { get; }
            public Improvements Farms { get; }
            public Improvements Mining { get; }
        }

        public class DefinedResources
        {
            public DefinedResources()
            {
                None = new TerrainResource("", ' ', "", new Resources(), null);
                Oasis = new TerrainResource("oasis", 'o', "Oasis", new Resources(3, 0, 0), "ts.oasis");
                GrasslandBonus = new TerrainResource("bonus", 'b', "Resources", new Resources(0, 1, 0),
                                                     "ts.grassland_resources");
                Coal = new TerrainResource("coal", 'c', "Coal", new Resources(0, 2, 0), "ts.coal");
                Furs = new TerrainResource("furs", 'f', "Furs", new Resources(1, 0, 3), "ts.furs");
                Pheasant = new TerrainResource("pheasant", 'p', "Pheasant", new Resources(2, 0, 0), "ts.pheasant");
            }

            public ITerrainResource None { get; }
            public ITerrainResource Oasis { get; }
            public ITerrainResource GrasslandBonus { get; }
            public ITerrainResource Coal { get; }
            public ITerrainResource Furs { get; }
            public ITerrainResource Pheasant { get; }
        }

        public class DefinedRoads
        {
            internal DefinedRoads(IdGenerator ids)
            {
                Nothing = new RoadType(-1, "None", ' ', "", false, -1, null);
                Road = new RoadType(ids.Next(), "road", '+', "Road", false, 2, "road.road");
                Railroad = new RoadType(ids.Next(), "Railroad", '#', "Railroad", false, 1, "road.railroad");
                River = new RoadType(ids.Next(), "River", 'x', "River", true, -1, "road.river");
            }

            public IRoadType Nothing { get; }
            public IRoadType Road { get; }
            public IRoadType Railroad { get; }
            public IRoadType River { get; }
        }

        public class DefinedTerrains
        {
            public const string OceanicClass = "oceanic";

            internal DefinedTerrains()
            {
                Nothing = new Terrain('i', "inaccessible")
                          .WithClass("land")
                          .WithGraphic("inaccessible", "arctic");

                Ocean = new Terrain(' ', "ocean")
                        .WithClass(OceanicClass)
                        .WithGraphic("coast")
                        .WithMoveCost(1)
                        .WithBaseProduction(Resource(1, 0, 2));

                DeepOcean = new Terrain(':', "deep_ocean")
                            .WithClass(OceanicClass)
                            .WithGraphic("floor", "coast")
                            .WithMoveCost(1)
                            .WithBaseProduction(Resource(1, 0, 2));

                Arctic = new Terrain('a', "glacier")
                         .WithClass("land")
                         .WithGraphic("arctic")
                         .WithMoveCost(2)
                         .WithRoads(4)
                         .WithMining(10, Prod(1));

                Desert = new Terrain('d', "desert")
                         .WithClass("land")
                         .WithGraphic("desert")
                         .WithMoveCost(1)
                         .WithRoads(2, TradeBoost(1f))
                         .WithBaseProduction(Prod(1))
                         .WithMining(5, Prod(1))
                         .WithIrrigation(5, Food(1));

                Forest = new Terrain('f', "forest")
                         .WithClass("land")
                         .WithGraphic("forest")
                         .WithMoveCost(2)
                         .WithRoads(4)
                         .WithBaseProduction(Resource(1, 2, 0));

                Gras = new Terrain('g', "gras")
                       .WithClass("land")
                       .WithGraphic("forest")
                       .WithMoveCost(1)
                       .WithRoads(2, TradeBoost(1f))
                       .WithBaseProduction(Resource(1, 0, 2))
                       .WithIrrigation(5, Food(1));

                Hills = new Terrain('h', "hills")
                        .WithClass("land")
                        .WithGraphic("hills")
                        .WithMoveCost(2)
                        .WithRoads(4)
                        .WithBaseProduction(Resource(1, 0, 0))
                        .WithIrrigation(10, Food(1))
                        .WithMining(10, Prod(3));

                Mountains = new Terrain('m', "mountains")
                            .WithClass("land")
                            .WithGraphic("mountains")
                            .WithMoveCost(3)
                            .WithRoads(6)
                            .WithBaseProduction(Prod(1))
                            .WithMining(10, Prod(1));

                Plains = new Terrain('p', "plains")
                         .WithClass("land")
                         .WithGraphic("plains")
                         .WithMoveCost(1)
                         .WithRoads(2, TradeBoost(1))
                         .WithBaseProduction(Resource(1, 1, 0))
                         .WithIrrigation(5, Food(1));

                Swamp = new Terrain('s', "swamp")
                        .WithClass("land")
                        .WithGraphic("swamp")
                        .WithMoveCost(1)
                        .WithRoads(4)
                        .WithBaseProduction(Food(1));

                Tundra = new Terrain('t', "tundra")
                         .WithClass("land")
                         .WithGraphic("tundra")
                         .WithMoveCost(1)
                         .WithRoads(4)
                         .WithBaseProduction(Food(1))
                         .WithIrrigation(5, Food(1));
            }

            public ITerrain Nothing { get; }
            public ITerrain Ocean { get; }
            public ITerrain DeepOcean { get; }
            public ITerrain Arctic { get; }
            public ITerrain Desert { get; }
            public ITerrain Forest { get; }
            public ITerrain Gras { get; }
            public ITerrain Hills { get; }
            public ITerrain Mountains { get; }
            public ITerrain Plains { get; }
            public ITerrain Tundra { get; }
            public ITerrain Swamp { get; }
        }
    }
}
