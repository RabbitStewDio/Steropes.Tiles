using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Steropes.Tiles.DataStructures;

namespace Steropes.Tiles.Monogame.Tiles
{
  public static class TexturePackLoader
  {
    class TexturePackLoaderContext
    {
      public string BasePath { get; }
      public TextureType TextureType { get; }
      public int Width { get; }
      public int Height { get; }

      public TexturePackLoaderContext(string basePath, TextureType textureType, int width, int height)
      {
        BasePath = basePath ?? throw new ArgumentNullException(nameof(basePath));
        TextureType = textureType;
        Width = width;
        Height = height;
      }

      public TexturePackLoaderContext Create(string basePath, int width, int height)
      {
        return new TexturePackLoaderContext(basePath, TextureType, width, height);
      }
    }

    public static ITexturePack Read(string path)
    {
      return Read(XDocument.Load(path).Root, path, new HashSet<string>());
    }

    public static ITexturePack Read(XDocument document, string path)
    {
      return Read(document.Root, path, new HashSet<string>());
    }

    public static ITexturePack Read(XElement root, string documentPath, HashSet<string> path)
    {

      if (root == null)
      {
        throw new ArgumentNullException(nameof(root));
      }

      path.Add(documentPath);

      var width = (int?) root.AttributeLocal("width") ?? throw new TexturePackLoaderException("Texture pack requires width", root);
      var height = (int?) root.AttributeLocal("height") ?? throw new TexturePackLoaderException("Texture pack requires height", root);
      var textureType = ParseTextureType((string)root.AttributeLocal("type"));

      var directoryInfo = Directory.GetParent(documentPath);
      var basePath = directoryInfo.FullName;
      var name = root.AttributeLocal("name")?.Value ?? "unnamed";
      var collections = ReadContent(root, new TexturePackLoaderContext(basePath, textureType, width, height), path);
      return new TexturePack(name, new IntDimension(width, height), textureType, basePath, collections.ToArray());
    }

    static TextureType ParseTextureType(string t, TextureType? defaultValue = null)
    {
      if (string.IsNullOrEmpty(t))
      {
        return defaultValue ?? throw new Exception("Texture type missing");
      }
      if (!Enum.TryParse(t, out TextureType result))
      {
        throw new TexturePackLoaderException("Texture type invalid.");
      }
      return result;
    }

    static string CombinePath(string p1, string p2)
    {
      if (p1 == null)
      {
        return new Uri(p2).LocalPath;
      }
      return new Uri(Path.Combine(p1, p2)).LocalPath;
    }

    static IEnumerable<ITextureFile> ReadInclude(XElement includeDirective, TexturePackLoaderContext context, HashSet<string> path)
    {
      var file = (string) includeDirective.AttributeLocal("file");
      if (file == null)
      {
        return new List<ITextureFile>();
      }

      var targetPath = CombinePath(context.BasePath, file);
      if (path.Contains(targetPath))
      {
        throw new TexturePackLoaderException($"Circular reference in include files or duplicate include while evaluating path {targetPath}", includeDirective);
      }

      path.Add(targetPath);
      var doc = XDocument.Load(targetPath);
      var directoryInfo = Directory.GetParent(targetPath);
      var basePath = directoryInfo.FullName;
      
      var root = doc.Root;
      var width = (int?) root.AttributeLocal("width") ?? context.Width;
      var height = (int?) root.AttributeLocal("height") ?? context.Height;
      var textureType = ParseTextureType((string) root.AttributeLocal("type"), context.TextureType);

      if (textureType != context.TextureType)
      {
        throw new TexturePackLoaderException($"Include file '{targetPath}' is using a '{textureType}' tile type.", includeDirective);
      }
      return ReadContent(root, context.Create(basePath, width, height), path);
    }

    static IEnumerable<ITextureFile> ReadContent(XElement root, TexturePackLoaderContext context, HashSet<string> path)
    {
      var includes =
        from e in root.Elements()
        where e.Name.LocalName == "include"
        where e.AttributeLocal("file") != null
        select ReadInclude(e, context, path);

      var c = 
        from e2 in root.Elements()
        where e2.Name.LocalName == "collection"
        select ReadCollection(e2, context);

      var retval = new List<ITextureFile>();
      foreach (var include in includes)
      {
        retval.AddRange(include);
      }
      retval.AddRange(c);
      return retval;
    }

    static ITextureFile ReadCollection(XElement c, TexturePackLoaderContext context)
    {
      var image = c.AttributeLocal("id");
      if (image == null)
      {
        throw new Exception();
      }

      var textureName = image.Value;

      var grids = 
        from e in c.Elements()
        where e.Name.LocalName == "grid"
        select ParseGrid(e, context);

      return new GridTextureFile(textureName, grids.ToArray());
    }

    static TileGrid ParseGrid(XElement grid, TexturePackLoaderContext context)
    {
      var x = (int) grid.AttributeLocal("x");
      var y = (int) grid.AttributeLocal("y");
      var width = (int?)grid.AttributeLocal("cell-width")?? (int?) grid.AttributeLocal("width") ?? context.Width;
      var height = (int?)grid.AttributeLocal("cell-height") ?? (int?) grid.AttributeLocal("height") ?? context.Height;

      var anchorX = (int?) grid.AttributeLocal("anchor-x") ?? width - (context.Width / 2);
      var anchorY = (int?) grid.AttributeLocal("anchor-y") ?? height - (context.Height / 2);

      var border = (int?) grid.AttributeLocal("cell-spacing") ??(int?) grid.AttributeLocal("border") ?? 0;

      var tiles =
        from e in grid.Elements()
        where e.Name.LocalName == "tile"
        select ParseTile(e);

      return new TileGrid(width, height, x, y, anchorX, anchorY, border, border, tiles.ToArray());
    }

    static GridTileDefinition ParseTile(XElement tile)
    {
      var x = (int)tile.AttributeLocal("x");
      var y = (int)tile.AttributeLocal("y");
      var anchorX = (int?)tile.AttributeLocal("anchor-x");
      var anchorY = (int?)tile.AttributeLocal("anchor-y");
      var name = (string) tile.ElementLocal("name");

      var tags = 
        from e in tile.Elements()
        where e.Name.LocalName == "tag"
        select (string)e;

      var tagsAsList = tags.ToList();
      if (name != null && tagsAsList.Count == 0)
      {
        tagsAsList.Add(name);
      }
      return new GridTileDefinition(name, x, y, anchorX, anchorY, tagsAsList.ToArray());
    }
  }
}