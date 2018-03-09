using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Steropes.Tiles.TemplateGenerator.Model
{
  public class TextureFileLoader
  {
    public class TexturePackLoaderContext
    {
      public string BasePath { get; }
      public TileType TextureType { get; }
      public int Width { get; }
      public int Height { get; }

      public TexturePackLoaderContext(string basePath, TileType textureType, int width, int height)
      {
        BasePath = basePath ?? throw new ArgumentNullException(nameof(basePath));
        TextureType = textureType;
        Width = width;
        Height = height;
      }
    }

    public static TextureFile Read(string path)
    {
      return Read(XDocument.Load(path).Root, path);
    }

    public static TextureFile Read(XDocument document, string path)
    {
      return Read(document.Root, path);
    }

    public static TextureFile Read(XElement root, string documentPath = null)
    {
      var defaultName = "Unnamed";
      if (documentPath != null)
      {
        defaultName = Path.GetFileNameWithoutExtension(Path.GetFileName(documentPath));
      }

      var width = (int?) root.AttributeLocal("width") ?? 32;
      var height = (int?) root.AttributeLocal("height") ?? 32;
      var textureType = ParseTextureType(root, (string) root.AttributeLocal("type"), TileType.Grid);

      var directoryInfo = Directory.GetParent(documentPath);
      var basePath = directoryInfo.FullName;
      var name = root.AttributeLocal("name")?.Value ?? defaultName;
      var collections = ReadContent(root, new TexturePackLoaderContext(basePath, textureType, width, height));
      var includes = ParseIncludeFiles(root);

      var metaData = root.ElementLocal("metadata");

      var file = new TextureFile
      {
        Name = name,
        Width = width,
        Height = height,
        TileType = textureType,
        Properties = ParseMetaDataProperties(metaData)
      };

      file.IncludeFiles.AddRange(includes);
      file.Collections.AddRange(collections);
      ParseFormattingInfo(root, file.FormattingMetaData);
      return file;
    }

    static IReadOnlyDictionary<string, string> ParseMetaDataProperties(XElement e)
    {
      var dictionary = new Dictionary<string, string>();
      if (e == null)
      {
        return new ReadOnlyDictionary<string, string>(dictionary);
      }

      var properties = e.Elements().Where(el => el.Name.LocalName == "property");
      foreach (var property in properties)
      {
        string value = (string) property;
        string key = (string) property.AttributeLocal("name");
        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
        {
          continue;
        }

        dictionary[key] = value;
      }

      return new ReadOnlyDictionary<string, string>(dictionary);
    }

    static void ParseFormattingInfo(XElement e, FormattingMetaData target)
    {
      var metaData = e.ElementLocal("metadata");
      if (metaData == null)
      {
        return;
      }

      target.Title = (string) metaData.AttributeLocal("title") ?? target.Title;
      target.TextColor = metaData.AttributeLocal("text-color").AsColor() ?? target.TextColor;
      target.BorderColor = metaData.AttributeLocal("border-color").AsColor() ?? target.BorderColor;
      target.BackgroundColor = metaData.AttributeLocal("background-color").AsColor() ?? target.BackgroundColor;

      target.Margin = (int?) metaData.AttributeLocal("margin") ?? target.Margin;
      target.Padding = (int?) metaData.AttributeLocal("padding") ?? target.Padding;
      target.Border = (int?) metaData.AttributeLocal("border") ?? target.Border;
    }


    static IEnumerable<TextureCollection> ReadContent(XElement root, TexturePackLoaderContext context)
    {
      var collections =
        from e in root.Elements()
        where e.Name.LocalName == "collection"
        select ReadCollection(e, context);

      return collections;
    }

    public static TextureCollection ReadCollection(XElement root, TexturePackLoaderContext context)
    {
      var id = (string) root.AttributeLocal("id") ?? "Unnamed Collection";

      var grids =
        from e in root.Elements()
        where e.Name.LocalName == "grid"
        select ReadGrid(e, context);

      var retval = new TextureCollection();
      retval.Id = id;
      retval.Grids.AddRange(grids);
      ParseFormattingInfo(root, retval.FormattingMetaData);
      return retval;
    }

    public static TextureGrid ReadGrid(XElement grid, TexturePackLoaderContext context)
    {
      var name = (string) grid.AttributeLocal("name") ?? "Unnamed Grid";
      var x = (int) grid.AttributeLocal("x");
      var y = (int) grid.AttributeLocal("y");
      var width = (int?) grid.AttributeLocal("cell-width") ?? (int?) grid.AttributeLocal("width") ?? context.Width;
      var height = (int?) grid.AttributeLocal("cell-height") ?? (int?) grid.AttributeLocal("height") ?? context.Height;
      var border = (int?) grid.AttributeLocal("cell-spacing") ?? (int?) grid.AttributeLocal("border") ?? 0;

      var anchorX = (int?) grid.AttributeLocal("anchor-x");
      var anchorY = (int?) grid.AttributeLocal("anchor-y");

      var gridValue = new TextureGrid
      {
        Name = name,
        X = x,
        Y = y,
        CellWidth = width,
        CellHeight = height,
        AnchorX = anchorX,
        AnchorY = anchorY,
        CellSpacing = border
      };

      var metadata = grid.ElementLocal("metadata");
      if (metadata != null)
      {
        gridValue.MatcherType = ParseMatchType(metadata, (string) metadata.AttributeLocal("match-type"), MatcherType.Basic);
        gridValue.Pattern = (string) metadata.AttributeLocal("pattern");

        gridValue.Width = (int?) metadata.AttributeLocal("grid-width");
        gridValue.Height = (int?) metadata.AttributeLocal("grid-height");
        gridValue.CellMapElements = (int?) metadata.AttributeLocal("cell-map-elements") ?? 0;
      }

      var tiles =
        from e in grid.Elements()
        where e.Name.LocalName == "tile"
        select ReadTiles(e);

      gridValue.Tiles.AddRange(tiles);
      ParseFormattingInfo(grid, gridValue.FormattingMetaData);

      return gridValue;
    }

    static MatcherType ParseMatchType(XElement lineInfo, string t, MatcherType? defaultValue = null)
    {
      if (string.IsNullOrEmpty(t))
      {
        return defaultValue ?? throw new TexturePackLoaderException("Match type missing", lineInfo);
      }

      if (!Enum.TryParse(t, out MatcherType result))
      {
        throw new TexturePackLoaderException("Match type invalid.", lineInfo);
      }

      return result;
    }

    public static TextureTile ReadTiles(XElement tile)
    {
      var x = (int) tile.AttributeLocal("x");
      var y = (int) tile.AttributeLocal("y");
      var anchorX = (int?) tile.AttributeLocal("anchor-x");
      var anchorY = (int?) tile.AttributeLocal("anchor-y");
      var name = (string) tile.ElementLocal("name");

      var tags =
        from e in tile.Elements()
        where e.Name.LocalName == "tag"
        select (string) e;

      var tagsAsList = tags.ToList();
      if (name != null && tagsAsList.Count == 0)
      {
        tagsAsList.Add(name);
      }

      var tileValue = new TextureTile()
      {
        X = x,
        Y = y,
        AnchorX = anchorX,
        AnchorY = anchorY
      };
      tileValue.Tags.AddRange(tagsAsList);
      return tileValue;
    }

    static IEnumerable<string> ParseIncludeFiles(XElement root)
    {
      var includes =
        from e in root.Elements()
        where e.Name.LocalName == "include"
        where !string.IsNullOrEmpty(e.AttributeLocal("file")?.Value)
        select e.AttributeLocal("file").Value;
      return includes.ToList();
    }

    static TileType ParseTextureType(IXmlLineInfo lineInfo, string t, TileType? defaultValue = null)
    {
      if (string.IsNullOrEmpty(t))
      {
        return defaultValue ?? throw new TexturePackLoaderException("Texture type missing", lineInfo);
      }

      if (!Enum.TryParse(t, out TileType result))
      {
        throw new TexturePackLoaderException("Texture type invalid.", lineInfo);
      }

      return result;
    }

  }
}