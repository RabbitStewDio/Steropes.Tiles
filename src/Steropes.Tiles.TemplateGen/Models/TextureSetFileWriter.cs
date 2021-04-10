using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Steropes.Tiles.TemplateGen.Models
{
    public static class TextureSetFileWriter
    {
        static readonly XNamespace Namespace = "http://www.steropes-ui.org/namespaces/tiles/1.0";

        public static XDocument GenerateXml(TextureSetFile file)
        {
            return new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), GenerateRoot(file));
        }

        public static XElement GenerateRoot(TextureSetFile file)
        {
            var e = new XElement(Namespace + "tileset");
            if (!string.IsNullOrEmpty(file.Name))
            {
                e.Add(new XAttribute("name", file.Name));
            }
            
            e.Add(new XAttribute("width", file.Width));
            e.Add(new XAttribute("height", file.Height));
            e.Add(new XAttribute("type", file.TileType));

            var metaData = GenerateMetaData(file.FormattingMetaData);
            foreach (var pair in file.Properties)
            {
                if (!string.IsNullOrEmpty(pair.Key) && !string.IsNullOrEmpty(pair.Value))
                {
                    metaData.Add(new XElement(Namespace + "property", new XAttribute("name", pair.Key), pair.Value));
                }
            }

            e.Add(metaData);

            foreach (var includeFile in file.IncludeFiles)
            {
                var fn = includeFile.SourcePath;
                if (fn != null)
                {
                    e.Add(new XElement(Namespace + "include", new XAttribute("file", Path.GetRelativePath(file.BasePath, fn))));
                }
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

            m.BackgroundColor.AsText().ForNotEmpty(c => me.Add(new XAttribute("background-color", c)));
            m.TextColor.AsText().ForNotEmpty(c => me.Add(new XAttribute("text-color", c)));
            m.BorderColor.AsText().ForNotEmpty(c => me.Add(new XAttribute("border-color", c)));

            m.Border.ForNonNull(b => me.Add(new XAttribute("border", b)));
            m.Padding.ForNonNull(b => me.Add(new XAttribute("padding", b)));
            m.Margin.ForNonNull(b => me.Add(new XAttribute("margin", b)));
            return me;
        }

        public static XElement GenerateCollection(TileTextureCollection tileTextureCollection)
        {
            var collectionElement = new XElement(Namespace + "collection");
            var metaData = GenerateMetaData(tileTextureCollection.FormattingMetaData);
            if (!string.IsNullOrEmpty(tileTextureCollection.LastExportLocation))
            {
                metaData.Add(new XAttribute("last-export-location", tileTextureCollection.LastExportLocation ?? ""));
            }

            if (metaData.HasAttributes || metaData.HasElements)
            {
                collectionElement.Add(metaData);
            }

            collectionElement.Add(new XAttribute("id", tileTextureCollection.Id ?? ""));
            collectionElement.AddRange(tileTextureCollection.Grids.Select(GenerateGrid));
            return collectionElement;
        }

        public static XElement GenerateGrid(TextureGrid grid)
        {
            var gridElement = new XElement(Namespace + "grid");

            var me = GenerateMetaData(grid.FormattingMetaData);
            grid.Width.ForNonNull(w => me.Add(new XAttribute("grid-width", w)));
            grid.Height.ForNonNull(h => me.Add(new XAttribute("grid-height", h)));
            if (!string.IsNullOrWhiteSpace(grid.CellMapElements))
            {
                me.Add(new XAttribute("cell-map-elements", grid.CellMapElements));
            }

            if (!string.IsNullOrWhiteSpace(grid.Pattern))
            {
                me.Add(new XAttribute("pattern", grid.Pattern));
            }

            me.Add(new XAttribute("matcher-type", grid.MatcherType));
            gridElement.Add(me);

            gridElement.Add(new XAttribute("name", grid.Name ?? ""));
            gridElement.Add(new XAttribute("x", grid.X));
            gridElement.Add(new XAttribute("y", grid.Y));
            gridElement.Add(new XAttribute("half-cell-hint", grid.MatcherType == MatcherType.Corner));
            gridElement.Add(new XAttribute("cell-spacing", grid.CellSpacing));
            grid.CellWidth.ForNonNull(w => gridElement.Add(new XAttribute("cell-width", w)));
            grid.CellHeight.ForNonNull(w => gridElement.Add(new XAttribute("cell-height", w)));


            grid.AnchorX.ForNonNull(x => gridElement.Add(new XAttribute("anchor-x", x)));
            grid.AnchorY.ForNonNull(y => gridElement.Add(new XAttribute("anchor-y", y)));

            gridElement.AddRange(grid.Tiles.Select(GenerateTile));
            return gridElement;
        }

        public static XElement GenerateTile(TextureTile tile)
        {
            var tileElement = new XElement(Namespace + "tile");
            tileElement.Add(new XAttribute("x", tile.X));
            tileElement.Add(new XAttribute("y", tile.Y));
            tile.AnchorX.ForNonNull(a => tileElement.Add(new XAttribute("anchor-x", a)));
            tile.AnchorY.ForNonNull(a => tileElement.Add(new XAttribute("anchor-y", a)));

            var md = new XElement(Namespace + "metadata");
            if (tile.AutoGenerated)
            {
                md.Add(new XAttribute("auto-generated", tile.AutoGenerated));
            }
            if (!string.IsNullOrEmpty(tile.SelectorHint))
            {
                md.Add(new XAttribute("selector-hint", tile.SelectorHint));
            }

            if (md.HasElements || md.HasAttributes)
            {
                tileElement.Add(md);
            }
            
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

        public static void ForNonNull<T>(this T? t, Action<T> a)
            where T : struct
        {
            if (t.HasValue)
            {
                a(t.Value);
            }
        }

        public static void ForNotEmpty(this string? t, Action<string> a)
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
