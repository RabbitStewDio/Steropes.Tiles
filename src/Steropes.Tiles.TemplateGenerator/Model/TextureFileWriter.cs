using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Steropes.Tiles.TemplateGenerator.Model
{
  public static class TextureFileWriter
  {
    static readonly XNamespace Namespace = "http://www.steropes-ui.org/namespaces/tiles/1.0";

    public static XDocument GenerateXml(TextureFile file)
    {
      return new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), GenerateRoot(file));
    }

    public static XElement GenerateRoot(TextureFile file)
    {
      var e = new XElement(Namespace + "tileset");
       e.Add(new XAttribute("width", file.Width));
       e.Add(new XAttribute("height", file.Height));
       e.Add(new XAttribute("type", file.TileType));

      var metaData = GenerateMetaData(file.FormattingMetaData);
      if (file.Properties != null)
      {
        foreach (var pair in file.Properties)
        {
          if (!string.IsNullOrEmpty(pair.Key) && !string.IsNullOrEmpty(pair.Value))
          {
            metaData.Add(new XElement(Namespace + "property", new XAttribute("name", pair.Key), pair.Value));
          }
        }
      }

      e.Add(metaData);

      foreach (var includeFile in file.IncludeFiles.Where(fn => !string.IsNullOrEmpty(fn)))
      {
        e.Add(new XElement(Namespace + "include", new XAttribute("file", includeFile)));
      }

      e.AddRange(file.Collections.Select(GenerateCollection));
      return e;
    }

    public static XElement GenerateMetaData(FormattingMetaData m)
    {
      var me = new XElement(Namespace + "metadata");
      if (!string.IsNullOrEmpty(m.Title))
      {
        me.Add(new XAttribute("title", m.Title));
      }
      m.BackgroundColor.AsText().ForEach(c => me.Add(new XAttribute("background-color", c)));
      m.TextColor.AsText().ForEach(c => me.Add(new XAttribute("text-color", c)));
      m.BorderColor.AsText().ForEach(c => me.Add(new XAttribute("border-color", c)));

      m.Border.ForEach(b => me.Add(new XAttribute("border", b)));
      m.Padding.ForEach(b => me.Add(new XAttribute("padding", b)));
      m.Margin.ForEach(b => me.Add(new XAttribute("margin", b)));
      return me;
    }

    public static XElement GenerateCollection(TextureCollection textureCollection)
    {
      var collectionElement = new XElement(Namespace + "collection");
      collectionElement.Add(new XAttribute("id", textureCollection.Id ?? ""));
      collectionElement.AddRange(textureCollection.Grids.Select(GenerateGrid));
      collectionElement.Add(GenerateMetaData(textureCollection.FormattingMetaData));
      return collectionElement;
    }

    public static XElement GenerateGrid(TextureGrid grid)
    {
      var gridElement = new XElement(Namespace + "grid");
 
      var me = GenerateMetaData(grid.FormattingMetaData);
      grid.Width.ForEach(w => me.Add(new XAttribute("grid-width", w)));
      grid.Height.ForEach(h => me.Add(new XAttribute("grid-height", h)));
      if (grid.CellMapElements > 0)
      {
        me.Add(new XAttribute("cell-map-elements", grid.CellMapElements));
      }
      gridElement.Add(me);

      gridElement.Add(new XAttribute("name", grid.Name ?? ""));
      gridElement.Add(new XAttribute("x", grid.X));
      gridElement.Add(new XAttribute("y", grid.Y));
      gridElement.Add(new XAttribute("cell-width", grid.CellWidth));
      gridElement.Add(new XAttribute("cell-height", grid.CellHeight));
      
      grid.FormattingMetaData.Border.ForEach(border => gridElement.Add(new XAttribute("border", border)));

      grid.AnchorX.ForEach(x => gridElement.Add(new XAttribute("anchor-x", x)));
      grid.AnchorY.ForEach(y => gridElement.Add(new XAttribute("anchor-y", y)));

      gridElement.AddRange(grid.Tiles.Select(GenerateTile));
      return gridElement;
    }

    public static XElement GenerateTile(TextureTile tile)
    {
      var tileElement = new XElement(Namespace + "tile");
      tileElement.Add(new XAttribute("x", tile.X));
      tileElement.Add(new XAttribute("y", tile.Y));
      tile.AnchorX.ForEach(a => tileElement.Add(new XAttribute("anchor-x", a)));
      tile.AnchorY.ForEach(a => tileElement.Add(new XAttribute("anchor-y", a)));

      var filteredTags = tile.Tags.Where(tn => !string.IsNullOrEmpty(tn)).ToList();
      if (filteredTags.Count == 1)
      {
        tileElement.Add(new XAttribute("tag", filteredTags[0]));
      }
      else
      {
        tileElement.AddRange(filteredTags.Select(t => new XElement(Namespace + "tag", t)));
      }

      return tileElement;
    }

    public static void ForEach<T>(this T? t, Action<T> a) where T: struct
    {
      if (t.HasValue)
      {
        a(t.Value);
      }
    }

    public static void ForEach(this string t, Action<string> a) 
    {
      if (!string.IsNullOrEmpty(t))
      {
        a(t);
      }
    }

    public static XElement AddRange(this XElement target, IEnumerable<XElement> elements)
    {
      foreach (var element in elements)
      {
        if (element != null)
        {
          target.Add(element);
        }
      }

      return target;
    }
  }
}