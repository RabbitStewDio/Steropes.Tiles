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

      foreach (var includeFile in file.IncludeFiles.Where(fn => !string.IsNullOrEmpty(fn)))
      {
        e.Add(new XElement(Namespace + "include", new XAttribute("file", includeFile)));
      }

      if (file.Properties != null && file.Properties.Count > 0)
      {
        var metadata = new XElement(Namespace + "metadata");
        foreach (var pair in file.Properties)
        {
          if (!string.IsNullOrEmpty(pair.Key) && string.IsNullOrEmpty(pair.Value))
          {
            metadata.Add(new XElement(Namespace + "metadata", new XAttribute("name", pair.Key), pair.Value));
          }
        }

        e.Add(metadata);
      }

      e.AddRange(file.Collections.Select(GenerateCollection));
      return e;
    }

    public static XElement GenerateCollection(TextureCollection textureCollection)
    {
      var collectionElement = new XElement(Namespace + "collection");
      collectionElement.Add(new XAttribute("id", textureCollection.Id ?? ""));
      collectionElement.AddRange(textureCollection.Grids.Select(GenerateGrid));
      return collectionElement;
    }

    public static XElement GenerateGrid(TextureGrid grid)
    {
      var gridElement = new XElement(Namespace + "grid");
      gridElement.Add(new XAttribute("name", grid.Name ?? ""));
      gridElement.Add(new XAttribute("x", grid.X));
      gridElement.Add(new XAttribute("y", grid.Y));
      gridElement.Add(new XAttribute("width", grid.Width));
      gridElement.Add(new XAttribute("height", grid.Height));
      if (grid.BorderX == grid.BorderY)
      {
        gridElement.Add(new XAttribute("border", grid.BorderX));
      }
      else
      {
        gridElement.Add(new XAttribute("border-x", grid.BorderX));
        gridElement.Add(new XAttribute("border-y", grid.BorderY));
      }
      gridElement.Add(new XAttribute("anchor-x", grid.AnchorX));
      gridElement.Add(new XAttribute("anchor-y", grid.AnchorY));


      gridElement.AddRange(grid.Groups.Select(GenerateGroup));
      gridElement.AddRange(grid.Tiles.Select(GenerateTile));
      return gridElement;
    }

    public static XElement GenerateGroup(TextureGroup groupVal)
    {
      var meta = new XElement(Namespace + "metadata");
      meta.Add(new XAttribute("match-type", groupVal.MatcherType));
      meta.Add(new XAttribute("pattern", groupVal.Pattern ?? ""));
      meta.Add(new XAttribute("origin-x", groupVal.X));
      meta.Add(new XAttribute("origin-y", groupVal.Y));

      var gridElement = new XElement(Namespace + "group");
      gridElement.Add(new XAttribute("name", groupVal.Name ?? ""));
      gridElement.Add(meta);
      gridElement.AddRange(groupVal.Tiles.Select(GenerateTile));
      return gridElement;
    }

    public static XElement GenerateTile(TextureTile tile)
    {
      var meta = new XElement(Namespace + "tile");
      meta.Add(new XAttribute("x", tile.X));
      meta.Add(new XAttribute("y", tile.Y));
      tile.AnchorX.ForEach(a => meta.Add(new XAttribute("anchor-x", a)));
      tile.AnchorY.ForEach(a => meta.Add(new XAttribute("anchor-y", a)));

      var filteredTags = tile.Tags.Where(tn => !string.IsNullOrEmpty(tn)).ToList();
      if (filteredTags.Count == 1)
      {
        meta.Add(new XAttribute("tag", filteredTags[0]));
      }
      else
      {
        meta.AddRange(filteredTags.Select(t => new XElement(Namespace + "tag", t)));
      }

      return meta;
    }

    public static void ForEach<T>(this T? t, Action<T> a) where T: struct
    {
      if (t.HasValue)
      {
        a(t.Value);
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