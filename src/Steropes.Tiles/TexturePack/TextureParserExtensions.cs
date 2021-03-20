using System.Linq;
using System.Xml.Linq;

namespace Steropes.Tiles.TexturePack
{
    public static class TextureParserExtensions
    {
        public static XElement ElementLocal(this XElement e, string localName)
        {
            return e.Elements().FirstOrDefault(ex => ex.Name.LocalName == localName);
        }

        public static XAttribute AttributeLocal(this XElement e, string localName)
        {
            return e.Attributes().FirstOrDefault(ex => ex.Name.LocalName == localName);
        }
    }
}
