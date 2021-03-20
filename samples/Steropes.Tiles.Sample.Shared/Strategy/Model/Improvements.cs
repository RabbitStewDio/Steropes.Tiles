namespace Steropes.Tiles.Demo.Core.GameData.Strategy.Model
{
    public class Improvements : RuleElement, ITerrainExtra
    {
        public Improvements(int dataId,
                            string id,
                            char asciiId,
                            string name,
                            Improvements requires,
                            string graphicTag,
                            params string[] alternativeGraphicTags)
            : base(id, asciiId, name, graphicTag, alternativeGraphicTags)
        {
            DataId = dataId;
            Requires = requires;
        }

        public int DataId { get; }
        public ITerrainExtra Requires { get; }
    }
}
