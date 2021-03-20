using System;
using System.Collections.Generic;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.TexturePack.Grids;
using Steropes.Tiles.TexturePack.Operations;

namespace Steropes.Tiles.TexturePack.Blending
{
    public static class BlendingTileGeneratorRegistry
    {
        public static bool TryCreate<TTile, TTexture, TColor>(
            out BlendingTileGeneratorRegistry<TTile, TTexture, TColor> result,
            ITileRegistry<TTile> tileRegistry,
            RenderType renderType,
            ITextureOperations<TTexture, TColor> textureOperations,
            IDerivedTileProducer<TTile, TTexture> tileProducer,
            IntDimension tileSize,
            string prefix = null,
            string ditherMaskName = null)
            where TTile : ITexturedTile<TTexture>
            where TTexture : ITexture
        {
            if (!TryFindDitherMask(out var ditherMask, tileRegistry, ditherMaskName))
            {
                result = default;
                return false;
            }

            var generator = new BlendingTileGenerator<TTile, TTexture, TColor>(tileRegistry,
                                                                               textureOperations, tileProducer, renderType, tileSize, ditherMask, prefix);
            result = new BlendingTileGeneratorRegistry<TTile, TTexture, TColor>(generator);
            return true;
        }

        public static bool TryFindDitherMask<TTile>(out TTile ditherMask,
                                                    ITileRegistry<TTile> tileRegistry,
                                                    string ditherTileName = null)
        {
            var ditherTileNameResolved = ditherTileName ?? "t.dither_tile";
            return tileRegistry.TryFind(ditherTileNameResolved, out ditherMask);
        }
    }

    public class BlendingTileGeneratorRegistry<TTile, TTexture, TColor> : ITileRegistryEx<CardinalIndex, TTile>
        where TTile : ITexturedTile<TTexture>
        where TTexture : ITexture
    {
        readonly BlendingTileGenerator<TTile, TTexture, TColor> generator;
        readonly Dictionary<TupleKey<string, CardinalIndex>, TupleKey<bool, TTile>> tiles;

        public BlendingTileGeneratorRegistry(BlendingTileGenerator<TTile, TTexture, TColor> generator)
        {
            this.tiles = new Dictionary<TupleKey<string, CardinalIndex>, TupleKey<bool, TTile>>();
            this.generator = generator ?? throw new ArgumentNullException(nameof(generator));
        }

        public BlendingTileGeneratorRegistry<TTile, TTexture, TColor> Populate(IEnumerable<string> tags)
        {
            foreach (var tag in tags)
            {
                TryFind(tag, CardinalIndex.North, out var _);
                TryFind(tag, CardinalIndex.East, out var _);
                TryFind(tag, CardinalIndex.South, out var _);
                TryFind(tag, CardinalIndex.West, out var _);
            }

            return this;
        }

        public bool TryFind(string tag, CardinalIndex selector, out TTile tile)
        {
            var key = new TupleKey<string, CardinalIndex>(tag, selector);
            if (tiles.TryGetValue(key, out var maybeTile))
            {
                if (maybeTile.Item1)
                {
                    tile = maybeTile.Item2;
                    return true;
                }

                tile = default;
                return false;
            }

            var hasTile = generator.TryGenerate(tag, selector, out var generatedTile);
            if (!hasTile)
            {
                tiles[key] = new TupleKey<bool, TTile>(false, default);
                tile = default;
                return false;
            }
            else
            {
                tiles[key] = new TupleKey<bool, TTile>(true, generatedTile);
                tile = generatedTile;
                return true;
            }
        }
    }
}
