using Steropes.Tiles.Demo.Core.GameData.Util;

namespace Steropes.Tiles.Demo.Core.GameData.Strategy.Model
{
    public interface IRoadType : ITerrainExtra
    {
        /// <summary>
        ///   Movement costs when a road of this type is built on a tile. 
        /// </summary>
        int MoveCost { get; }

        /// <summary>
        ///  Extra flag designating this road type as a river. While
        ///  each tile can only have one road type active, tiles can have
        ///  both rivers and roads.
        /// </summary>
        bool River { get; }
    }

    public static class TerrainExtra
    {
        public static bool HasExtra(this ushort flags, ITerrainExtra extra)
        {
            if (extra == null || extra.DataId == -1)
            {
                return false;
            }

            return flags.Read(extra.DataId);
        }

        public static ushort AddExtra(this ushort flags, ITerrainExtra extra)
        {
            if (extra.Requires != null)
            {
                if (!flags.HasExtra(extra.Requires))
                {
                    flags = flags.AddExtra(extra.Requires);
                }
            }

            if (extra.DataId != -1)
            {
                flags = flags.Write(extra.DataId, true);
            }

            return flags;
        }
    }

    public class RoadType : RuleElement, IRoadType
    {
        public const int NoMoveCostBonus = -1;

        public RoadType(int dataId,
                        string id,
                        char asciiId,
                        string name,
                        bool river,
                        int moveCost,
                        RoadType requiredExtra,
                        string graphicTag,
                        params string[] alternativeGraphicTags) : base(id, asciiId, name, graphicTag,
                                                                       alternativeGraphicTags)
        {
            DataId = dataId;
            River = river;
            MoveCost = moveCost;
            Requires = requiredExtra;
        }

        public RoadType(int dataId,
                        string id,
                        char asciiId,
                        string name,
                        bool river,
                        int moveCost,
                        string graphicTag,
                        params string[] alternativeGraphicTags) :
            this(dataId, id, asciiId, name, river, moveCost, null, graphicTag, alternativeGraphicTags)
        { }

        public bool River { get; }
        public int MoveCost { get; }
        public int DataId { get; }
        public ITerrainExtra Requires { get; }
    }
}
