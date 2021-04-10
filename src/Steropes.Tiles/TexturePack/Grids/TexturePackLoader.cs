using JetBrains.Annotations;
using Steropes.Tiles.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Steropes.Tiles.TexturePack.Grids
{
    public class TexturePackLoader<TTile, TTexture, TRawTexture>
        where TTile : ITexturedTile<TTexture>
        where TTexture : ITexture
        where TRawTexture : IRawTexture<TTexture>
    {
        class TexturePackLoaderContext
        {
            public FilePath BasePath { get; }
            public TextureType TextureType { get; }
            public int Width { get; }
            public int Height { get; }

            public TexturePackLoaderContext(FilePath basePath,
                                            TextureType textureType,
                                            int width,
                                            int height)
            {
                BasePath = basePath;
                TextureType = textureType;
                Width = width;
                Height = height;
            }

            public TexturePackLoaderContext Create(int width, int height)
            {
                return new TexturePackLoaderContext(BasePath, TextureType, width, height);
            }
        }

        readonly IContentLoader<TRawTexture> contentLoader;
        readonly ITileProducer<TTile, TTexture, TRawTexture> tileProducer;
        readonly IFileSystemAdapter fileSystem;

        public TexturePackLoader([NotNull] IContentLoader<TRawTexture> contentLoader,
                                 [NotNull] ITileProducer<TTile, TTexture, TRawTexture> tileProducer,
                                 IFileSystemAdapter fileSystem = null)
        {
            this.contentLoader = contentLoader ?? throw new ArgumentNullException(nameof(contentLoader));
            this.tileProducer = tileProducer ?? throw new ArgumentNullException(nameof(tileProducer));
            this.fileSystem = fileSystem ?? new DefaultFileSystemAdapter();
        }

        public ITexturePack<TTile> Read(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            var absPath = fileSystem.MakeAbsolute(path);
            using (var stream = fileSystem.Read(absPath))
            {
                var root = XDocument.Load(stream).Root;
                return Read(root, absPath, new HashSet<FilePath>());
            }
        }

        public ITexturePack<TTile> Read(XDocument document, string path)
        {
            var absPath = fileSystem.MakeAbsolute(path);
            return Read(document.Root, absPath, new HashSet<FilePath>());
        }

        ITexturePack<TTile> Read(XElement root, FilePath documentPath, HashSet<FilePath> path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            path.Add(documentPath);

            var width = (int?)root.AttributeLocal("width") ??
                        throw new TexturePackLoaderException("Texture pack requires width", root);
            var height = (int?)root.AttributeLocal("height") ??
                         throw new TexturePackLoaderException("Texture pack requires height", root);
            var textureType = ParseTextureType((string)root.AttributeLocal("type"));

            var name = root.AttributeLocal("name")?.Value ?? "unnamed";
            var basePath = fileSystem.GetBasePath(documentPath);
            // Debug.Log("Reading file " + basePath + " " + documentPath);
            var collections =
                ReadContent(root, new TexturePackLoaderContext(basePath, textureType, width, height), path);
            return new TexturePack<TTile>(name, new IntDimension(width, height), textureType, collections.ToArray());
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

        IEnumerable<ITextureFile<TTile>> ReadInclude(XElement includeDirective,
                                                     TexturePackLoaderContext context,
                                                     HashSet<FilePath> path)
        {
            var file = (string)includeDirective.AttributeLocal("file");
            if (file == null)
            {
                return new List<ITextureFile<TTile>>();
            }

            var targetPath = fileSystem.CombinePath(context.BasePath, file);
            if (path.Contains(targetPath))
            {
                throw new TexturePackLoaderException(
                    $"Circular reference in include files or duplicate include while evaluating path {targetPath}",
                    includeDirective);
            }

            path.Add(targetPath);
            using (var stream = fileSystem.Read(targetPath))
            {
                var doc = XDocument.Load(stream);

                var root = doc.Root;
                var width = (int?)root.AttributeLocal("width") ?? context.Width;
                var height = (int?)root.AttributeLocal("height") ?? context.Height;
                var textureType = ParseTextureType((string)root.AttributeLocal("type"), context.TextureType);

                if (textureType != context.TextureType)
                {
                    throw new TexturePackLoaderException(
                        $"Include file '{targetPath}' is using a '{textureType}' tile type.", includeDirective);
                }

                var loaderContext = context.Create(width, height);
                return ReadContent(root, loaderContext, path);
            }
        }

        IEnumerable<ITextureFile<TTile>> ReadContent(XElement root,
                                                     TexturePackLoaderContext context,
                                                     HashSet<FilePath> path)
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

            var retval = new List<ITextureFile<TTile>>();
            foreach (var include in includes)
            {
                retval.AddRange(include);
            }

            retval.AddRange(c);
            return retval;
        }

        ITextureFile<TTile> ReadCollection(XElement c, TexturePackLoaderContext context)
        {
            var image = c.AttributeLocal("id");
            if (image == null)
            {
                throw new Exception();
            }

            var textureName = (string)fileSystem.CombinePath(context.BasePath, image.Value);

            var grids =
                from e in c.Elements()
                where e.Name.LocalName == "grid"
                select ParseGrid(e, context);

            return new GridTextureFile<TTile, TTexture, TRawTexture>(textureName,
                                                                     new IntDimension(context.Width, context.Height),
                                                                     tileProducer,
                                                                     contentLoader,
                                                                     grids.ToArray());
        }

        TileGrid ParseGrid(XElement grid, TexturePackLoaderContext context)
        {
            var halfCell = (bool?)grid.Attribute("half-cell-hint") ?? false;
            var defaultWidth = halfCell ? context.Width / 2 : context.Width;
            var defaultHeight = halfCell ? context.Height / 2 : context.Height;

            var x = (int)grid.AttributeLocal("x");
            var y = (int)grid.AttributeLocal("y");
            var width = (int?)grid.AttributeLocal("cell-width") ?? (int?)grid.AttributeLocal("width") ?? defaultWidth;
            var height = (int?)grid.AttributeLocal("cell-height") ??
                         (int?)grid.AttributeLocal("height") ?? defaultHeight;

            var anchorX = (int?)grid.AttributeLocal("anchor-x") ?? width / 2;
            var anchorY = (int?)grid.AttributeLocal("anchor-y") ?? height - defaultHeight / 2;

            var border = (int?)grid.AttributeLocal("cell-spacing") ?? (int?)grid.AttributeLocal("border") ?? 0;

            var tiles =
                from e in grid.Elements()
                where e.Name.LocalName == "tile"
                select ParseTile(e);

            return new TileGrid(width, height, x, y, anchorX, anchorY, border, border, tiles.ToArray());
        }

        static GridTileDefinition ParseTile(XElement tile)
        {
            var x = (int?)tile.AttributeLocal("x") ??
                    throw new TexturePackLoaderException("Mandatory attribute x is missing", tile);
            var y = (int?)tile.AttributeLocal("y") ??
                    throw new TexturePackLoaderException("Mandatory attribute y is missing", tile);
            var anchorX = (int?)tile.AttributeLocal("anchor-x");
            var anchorY = (int?)tile.AttributeLocal("anchor-y");
            var name = (string)tile.AttributeLocal("tag") ?? (string)tile.AttributeLocal("name");

            var tags =
                from e in tile.Elements()
                where e.Name.LocalName == "tag"
                select (string)e;

            var tagsAsList = tags.ToList();
            if (name != null && tagsAsList.Count == 0)
            {
                tagsAsList.Add(name);
            }

            if (tagsAsList.Count == 0)
            {
                throw new TexturePackLoaderException("Tiles must have at least one tag name");
            }

            return new GridTileDefinition(name, x, y, anchorX, anchorY, tagsAsList.ToArray());
        }
    }
}
