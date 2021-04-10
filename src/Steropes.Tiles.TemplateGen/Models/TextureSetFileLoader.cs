using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Steropes.Tiles.TemplateGen.Models
{
    public static class TextureSetFileLoader
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

        public static TextureSetFile Read(string path)
        {
            var element = XDocument.Load(path).Root;
            if (element == null)
            {
                throw new InvalidOperationException();
            }

            return ReadInternal(element, path);
        }

        public static TextureSetFile Read(XDocument document, string? path)
        {
            var element = document.Root;
            if (element == null)
            {
                throw new InvalidOperationException();
            }

            return ReadInternal(element, path);
        }

        static TextureSetFile ReadInternal(XElement root, string? documentPath, HashSet<string>? previousReads = null)
        {
            previousReads ??= new HashSet<string>();
            if (documentPath != null)
            {
                if (previousReads.Contains(documentPath))
                {
                    throw new InvalidOperationException("Circular reference in include files detected");
                }
            }

            var defaultName = "Unnamed";
            string basePath;
            if (!string.IsNullOrWhiteSpace(documentPath))
            {
                defaultName = Path.GetFileNameWithoutExtension(Path.GetFileName(documentPath));
                basePath = Path.GetDirectoryName(documentPath) ?? Environment.CurrentDirectory;
            }
            else
            {
                basePath = Environment.CurrentDirectory;
            }

            var width = (int?)root.AttributeLocal("width") ?? 32;
            var height = (int?)root.AttributeLocal("height") ?? 32;
            var textureType = TextureParserExtensions.ParseTextureType(root, (string?)root.AttributeLocal("type"), TileType.Grid);


            var name = root.AttributeLocal("name")?.Value ?? defaultName;
            var loaderContext = new TexturePackLoaderContext(basePath, textureType, width, height);
            var collections = ReadContent(root, loaderContext);
            var includes = ParseIncludeFiles(root);

            var metaData = root.ElementLocal("metadata");

            var file = new TextureSetFile
            {
                Name = name,
                Width = width,
                Height = height,
                TileType = textureType,
                Properties = ParseMetaDataProperties(metaData)
            };

            var x = includes.Select(f => ParseIncludeFile(previousReads, f, basePath));
            foreach (var includedFile in x)
            {
                if (includedFile != null)
                {
                    file.IncludeFiles.Add(includedFile);
                }
            }
            
            file.Collections.AddRange(collections);
            file.SourcePath = documentPath;
            ParseFormattingInfo(root, file.FormattingMetaData);
            return file;
        }

        static TextureSetFile? ParseIncludeFile(HashSet<string>? ctx, string fileName, string basePath)
        {
            if (!Path.IsPathRooted(fileName))
            {
                fileName = Path.Combine(basePath, fileName);
            }

            if (!File.Exists(fileName))
            {
                return null;
            }
            
            var element = XDocument.Load(fileName).Root;
            if (element == null)
            {
                return null;
            }

            return ReadInternal(element, fileName, ctx);
        }

        static IReadOnlyDictionary<string, string> ParseMetaDataProperties(XElement? e)
        {
            var dictionary = new Dictionary<string, string>();
            if (e == null)
            {
                return new ReadOnlyDictionary<string, string>(dictionary);
            }

            var properties = e.Elements().Where(el => el.Name.LocalName == "property");
            foreach (var property in properties)
            {
                string? value = (string?)property;
                string? key = (string?)property.AttributeLocal("name");
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

            target.Title = (string?)metaData.AttributeLocal("title") ?? target.Title;
            target.TextColor = metaData.AttributeLocal("text-color").AsColor() ?? target.TextColor;
            target.BorderColor = metaData.AttributeLocal("border-color").AsColor() ?? target.BorderColor;
            target.BackgroundColor = metaData.AttributeLocal("background-color").AsColor() ?? target.BackgroundColor;

            target.Margin = (int?)metaData.AttributeLocal("margin") ?? target.Margin;
            target.Padding = (int?)metaData.AttributeLocal("padding") ?? target.Padding;
            target.Border = (int?)metaData.AttributeLocal("border") ?? target.Border;
        }


        static IEnumerable<TileTextureCollection> ReadContent(XElement root, TexturePackLoaderContext context)
        {
            var collections =
                from e in root.Elements()
                where e.Name.LocalName == "collection"
                select ReadCollection(e, context);

            return collections;
        }

        public static TileTextureCollection ReadCollection(XElement root, TexturePackLoaderContext context)
        {
            var id = (string?)root.AttributeLocal("id") ?? "Unnamed Collection";

            var grids =
                from e in root.Elements()
                where e.Name.LocalName == "grid"
                select ReadGrid(e, context);

            var retval = new TileTextureCollection();
            retval.Id = id;
            retval.Grids.AddRange(grids);
            ParseFormattingInfo(root, retval.FormattingMetaData);

            var lastExportLocation = (string?)root.ElementLocal("metadata")?.Attribute("last-export-location");
            retval.LastExportLocation = lastExportLocation;
            return retval;
        }

        public static TextureGrid ReadGrid(XElement grid, TexturePackLoaderContext context)
        {
            var name = (string?)grid.AttributeLocal("name") ?? "Unnamed Grid";
            var x = (int?)grid.AttributeLocal("x") ?? throw new XmlParseException("Required attribute 'x' not found", grid);
            var y = (int?)grid.AttributeLocal("y") ?? throw new XmlParseException("Required attribute 'y' not found", grid);
            var width = (int?)grid.AttributeLocal("cell-width") ?? (int?)grid.AttributeLocal("width");
            var height = (int?)grid.AttributeLocal("cell-height") ?? (int?)grid.AttributeLocal("height");
            var border = (int?)grid.AttributeLocal("cell-spacing") ?? (int?)grid.AttributeLocal("border") ?? 0;

            var anchorX = (int?)grid.AttributeLocal("anchor-x");
            var anchorY = (int?)grid.AttributeLocal("anchor-y");

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
                gridValue.MatcherType = ParseMatchType(metadata, (string?)metadata.AttributeLocal("matcher-type"), MatcherType.Basic);
                gridValue.Pattern = (string?)metadata.AttributeLocal("pattern");

                gridValue.Width = (int?)metadata.AttributeLocal("grid-width");
                gridValue.Height = (int?)metadata.AttributeLocal("grid-height");
                gridValue.CellMapElements = (string?)metadata.AttributeLocal("cell-map-elements");
            }

            var tiles =
                from e in grid.Elements()
                where e.Name.LocalName == "tile"
                select ReadTiles(e);

            gridValue.Tiles.AddRange(tiles);
            ParseFormattingInfo(grid, gridValue.FormattingMetaData);

            return gridValue;
        }

        static MatcherType ParseMatchType(XElement lineInfo, string? t, MatcherType? defaultValue = null)
        {
            if (string.IsNullOrEmpty(t))
            {
                return defaultValue ?? throw new XmlParseException("Match type missing", lineInfo);
            }

            if (!Enum.TryParse(t, out MatcherType result))
            {
                throw new XmlParseException("Match type invalid.", lineInfo);
            }

            return result;
        }

        public static TextureTile ReadTiles(XElement tile)
        {
            var x = (int?)tile.AttributeLocal("x") ?? throw new XmlParseException("Required attribute 'y' not found", tile);
            var y = (int?)tile.AttributeLocal("y") ?? throw new XmlParseException("Required attribute 'y' not found", tile);
            var anchorX = (int?)tile.AttributeLocal("anchor-x");
            var anchorY = (int?)tile.AttributeLocal("anchor-y");
            var name = (string?)tile.ElementLocal("name");

            var tags =
                from e in tile.Elements()
                where e.Name.LocalName == "tag"
                select (string)e;

            var tagsAsList = tags.ToList();
            if (name != null && tagsAsList.Count == 0)
            {
                tagsAsList.Add(name);
            }

            var tag = (string?)tile.AttributeLocal("tag");

            var autoGenerated = (bool?)tile.ElementLocal("metadata")?.Attribute("auto-generated") ?? false;
            var selectorHint = (string?)tile.ElementLocal("metadata")?.Attribute("selector-hint");

            var tileValue = new TextureTile(autoGenerated)
            {
                X = x,
                Y = y,
                AnchorX = anchorX,
                AnchorY = anchorY,
                SelectorHint = selectorHint
            };
            if (!string.IsNullOrWhiteSpace(tag) && !tagsAsList.Contains(tag))
            {
                tileValue.Tags.Add(tag);
            }

            tileValue.Tags.AddRange(tagsAsList);
            return tileValue;
        }

        static IEnumerable<string> ParseIncludeFiles(XElement root)
        {
            var includes =
                from e in root.Elements()
                where e.Name.LocalName == "include"
                where !string.IsNullOrEmpty(e.AttributeLocal("file")?.Value)
                select e.AttributeLocal("file")?.Value;
            return includes.ToList();
        }
    }
}
