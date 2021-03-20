using System;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.TexturePack.Grids;
using Steropes.Tiles.TexturePack.Operations;

namespace Steropes.Tiles.TexturePack.Blending
{
    /// <summary>
    ///   A blended tile generator that tries to find predefined blended tiles,
    ///   and if that fails generates one for each direction by combining the
    ///   terrain texture with a dithering mask (stored as 't.dither_tile').
    ///   <para />
    ///   This generator first checks whether a manually defined blending tile
    ///   exists for the given base tag and direction by querying for a tile
    ///   named '{prefix}{tag}_{direction}'. Unless defined otherwise, Prefix
    ///   defaults to 't.blend.gen.'.
    /// </summary>
    public class BlendingTileGenerator<TTile, TTexture, TColor>
        where TTile : ITexturedTile<TTexture>
        where TTexture : ITexture
    {
        readonly TTile ditherMask;
        readonly string prefix;
        readonly RenderType renderType;
        readonly ITileRegistry<TTile> tileRegistry;
        readonly IDerivedTileProducer<TTile, TTexture> tileProducer;
        readonly IntDimension tileSize;
        readonly ITextureOperations<TTexture, TColor> textureOperations;
        readonly CardinalIndexTileRegistry<TTile> cardinalMaskRegistry;
        readonly CacheEntry[] maskCache;

        public BlendingTileGenerator(ITileRegistry<TTile> tileRegistry,
                                     ITextureOperations<TTexture, TColor> textureOperations,
                                     IDerivedTileProducer<TTile, TTexture> tileProducer,
                                     RenderType renderType,
                                     IntDimension tileSize,
                                     TTile ditherMask,
                                     string prefix = "t.blend.gen.")
        {
            this.tileRegistry = tileRegistry ?? throw new ArgumentNullException(nameof(tileRegistry));
            this.tileProducer = tileProducer;
            this.renderType = renderType;
            this.tileSize = tileSize;
            this.ditherMask = ditherMask;
            this.prefix = prefix ?? "t.blend.gen";
            this.textureOperations = textureOperations ?? throw new ArgumentNullException(nameof(textureOperations));

            cardinalMaskRegistry = CardinalIndexTileRegistry<TTile>.CreateLong(tileRegistry);

            this.maskCache = PopulateCache();
        }


        public bool TryGenerate(string tag, CardinalIndex direction, out TTile tile)
        {
            var etag = $"{prefix}{tag}_{direction}";
            if (tileRegistry.TryFind(etag, out var predefined))
            {
                tile = predefined;
                return true;
            }

            var sourceArea = textureOperations.TileAreaForCardinalDirection(tileSize, direction);
            if (!TryFindSourceMask(direction, out var effectiveMask, out var anchor))
            {
                tile = default;
                return false;
            }

            BoundedTextureData<TColor> result;
            if (!tileRegistry.TryFind(tag, out var terrain) ||
                !terrain.HasTexture)
            {
                tile = default;
                return false;
            }

            var data = textureOperations.ExtractData(terrain.Texture, sourceArea);
            result = textureOperations.CombineMask(data, effectiveMask);

            var wrappedTextureSize = new IntDimension(tileSize.Width, tileSize.Height);
            var wrappedTexture = textureOperations.CreateTexture(etag, wrappedTextureSize);
            textureOperations.ApplyTextureData(wrappedTexture, result, sourceArea.Origin);

            tile = tileProducer.Produce(tileSize, anchor, etag, wrappedTexture);
            return true;
        }


        bool TryFindSourceMask(CardinalIndex dir, out BoundedTextureData<TColor> colorData, out IntPoint anchor)
        {
            var idx = (int) dir;
            var ce = maskCache[idx];
            colorData = ce.ColorData;
            anchor = ce.Anchor;
            return ce.Exists;
        }

        readonly struct CacheEntry
        {
            public readonly bool Exists;
            public readonly BoundedTextureData<TColor> ColorData;
            public readonly IntPoint Anchor;

            public CacheEntry(bool exists, BoundedTextureData<TColor> colorData, IntPoint anchor)
            {
                Exists = exists;
                ColorData = colorData;
                Anchor = anchor;
            }
        }


        CacheEntry[] PopulateCache()
        {
            var maskCache = new CacheEntry[4];
            for (int i = 0; i < 4; i += 1)
            {
                var direction = (CardinalIndex) i;
                maskCache[i] = CacheTryFindSourceMask(direction);
            }

            return maskCache;
        }

        CacheEntry CacheTryFindSourceMask(CardinalIndex dir)
        {
            if (cardinalMaskRegistry.TryFind("mask.blending", dir, out var providedTile) &&
                providedTile.HasTexture)
            {
                return new CacheEntry(true,
                    textureOperations.ExtractData(providedTile.Texture, providedTile.Texture.Bounds),
                    providedTile.Anchor);
            }

            var sourceArea = textureOperations.TileAreaForCardinalDirection(tileSize, dir);
            if (renderType.IsIsometric())
            {
                if (ditherMask.HasTexture)
                {
                    return new CacheEntry(true,
                        textureOperations.ExtractData(ditherMask.Texture, sourceArea),
                        ditherMask.Anchor);
                }
            }

            return new CacheEntry(false, null, default);
        }
    }
}