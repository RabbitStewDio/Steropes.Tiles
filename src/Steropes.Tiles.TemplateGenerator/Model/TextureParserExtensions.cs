using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace Steropes.Tiles.TemplateGenerator.Model
{
  public static class TextureParserExtensions
  {
    public static ObservableCollection<T> AddRange<T>(this ObservableCollection<T> list, IEnumerable<T> range)
    {
      foreach (var r in range)
      {
        list.Add(r);
      }

      return list;
    }

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