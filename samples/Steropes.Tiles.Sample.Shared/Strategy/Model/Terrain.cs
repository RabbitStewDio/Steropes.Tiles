using System;
using System.Collections.Generic;

namespace Steropes.Tiles.Demo.Core.GameData.Strategy.Model
{
    /// <summary>
    ///   A basic, freeciv inspired terrain definition.
    ///   The interface is exposing readonly properties as no sane program changes
    ///   its basic data on my watch.
    /// </summary>
    public interface ITerrain : IRuleElement
    {
        /// <summary>
        ///   The terrain classification. FreeCiv uses only two tags: Land and Oceanic, but
        ///   we could group terrain into more sets if needed. Tagging terrain with
        ///   classes allows us to generalize movement of units, for instance.
        /// </summary>
        IReadOnlyCollection<string> Class { get; }

        /// <summary>
        ///   Base movement costs. Those can be modified by building roads on a tile.
        ///   Once a road is built, the road's move cost is used instead.
        /// </summary>
        int MoveCost { get; }

        /// <summary>
        ///   Base production.
        /// </summary>
        Resources Production { get; }

        /// <summary>
        ///   Production changes after building roads.
        /// </summary>
        Resources RoadBonus { get; }

        /// <summary>
        ///  Percentage boost after building roads. (ie 50% more trade = ResourceBoost(0,0,0.5))
        /// </summary>
        ResourcesBoost RoadBoost { get; }

        /// <summary>
        ///   Production changes after building mines.
        /// </summary>
        Resources MiningBonus { get; }

        /// <summary>
        ///   Production changes after building irrigation systems.
        /// </summary>
        Resources IrrigationBonus { get; }

        /// <summary>
        ///   Time in turns to build mines.
        /// </summary>
        int MiningTime { get; }

        /// <summary>
        ///   Time in turns to build irrigation.
        /// </summary>
        int IrrigationTime { get; }

        /// <summary>
        ///   Time in turns to build roads.
        /// </summary>
        int RoadTime { get; }
    }

    public class Terrain : ITerrain, IEquatable<Terrain>
    {
        IReadOnlyList<string> alternativeGraphicTags;
        IReadOnlyCollection<string> classes;

        public Terrain(char asciiId, string id)
        {
            AsciiId = asciiId;
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = Id; // a sensible default ..
            Class = new List<string>();
            AlternativeGraphicTags = new List<string>();
        }

        public bool Equals(Terrain other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return AsciiId == other.AsciiId && string.Equals(Id, other.Id);
        }

        public string Name { get; set; }
        public string GraphicTag { get; set; }

        public IReadOnlyList<string> AlternativeGraphicTags
        {
            get { return alternativeGraphicTags; }
            set { alternativeGraphicTags = value ?? throw new ArgumentNullException(nameof(value)); }
        }

        public char AsciiId { get; }
        public string Id { get; }

        public IReadOnlyCollection<string> Class
        {
            get { return classes; }
            set { classes = value ?? throw new ArgumentNullException(nameof(value)); }
        }

        public int MoveCost { get; set; }
        public Resources Production { get; set; }
        public Resources RoadBonus { get; set; }
        public ResourcesBoost RoadBoost { get; set; }
        public Resources MiningBonus { get; set; }
        public Resources IrrigationBonus { get; set; }
        public int MiningTime { get; set; }
        public int IrrigationTime { get; set; }
        public int RoadTime { get; set; }


        public void AddClass(string c)
        {
            Class = new List<string>(classes) {c ?? throw new ArgumentNullException()};
        }

        public void AddAlternateGraphicTag(string c)
        {
            Class = new List<string>(alternativeGraphicTags) {c ?? throw new ArgumentNullException()};
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Terrain)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (AsciiId.GetHashCode() * 397) ^ (Id != null ? Id.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Terrain left, Terrain right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Terrain left, Terrain right)
        {
            return !Equals(left, right);
        }
    }

    public static class TerrainExtensions
    {
        public static IEnumerable<string> AllGraphicTags(this IRuleElement t)
        {
            var tags = new List<string>();
            if (t.GraphicTag != null)
            {
                tags.Add(t.GraphicTag);
            }

            tags.AddRange(t.AlternativeGraphicTags);
            return tags;
        }

        public static Terrain WithGraphic(this Terrain t, string graphicTag, params string[] extraGraphics)
        {
            t.GraphicTag = graphicTag;

            var tags = new List<string>(t.AlternativeGraphicTags);
            tags.AddRange(extraGraphics);
            t.AlternativeGraphicTags = tags.AsReadOnly();
            return t;
        }

        public static Terrain WithClass(this Terrain t, string clazz, params string[] classes)
        {
            var tags = new List<string>(t.Class);
            tags.Add(clazz);
            tags.AddRange(classes);
            t.Class = tags.AsReadOnly();
            return t;
        }

        public static Terrain WithMoveCost(this Terrain t, int mc)
        {
            t.MoveCost = mc;
            return t;
        }

        public static Terrain WithMining(this Terrain t, int mc, Resources bonus)
        {
            t.MiningTime = mc;
            t.MiningBonus = bonus;
            return t;
        }

        public static Terrain WithIrrigation(this Terrain t, int mc, Resources bonus)
        {
            t.IrrigationTime = mc;
            t.IrrigationBonus = bonus;
            return t;
        }

        public static Terrain WithRoads(this Terrain t, int mc, ResourcesBoost bonus = new ResourcesBoost())
        {
            t.RoadTime = mc;
            t.RoadBoost = bonus;
            return t;
        }

        public static Terrain WithBaseProduction(this Terrain t, Resources prod)
        {
            t.Production = prod;
            return t;
        }
    }
}
