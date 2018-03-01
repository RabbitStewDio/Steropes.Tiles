using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Monogame.Textures;
using Steropes.Tiles.Monogame.Tiles;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Monogame.Blending
{
  /// <summary>
  ///   A blended tile generator that tries to find predefined blended tiles,
  ///   and if that fails generates one for each direction by combining the
  ///   terrain texture with a dithering mask (stored as 't.dither_tile').
  ///   <para />
  ///   This generator first checks whether a manually defined blending
  ///   tile exists for the given base tag and direction by querying for
  ///   a tile named '{prefix}{tag}_{direction}'. Unless defined otherwise,
  ///   Prefix defaults to 't.blend.gen.'.
  /// </summary>
  public class BlendingTileGenerator
  {
    readonly Color[] blank;
    readonly MultiTextureAtlasBuilder builder;
    readonly ITexturedTile ditherMask;
    readonly GraphicsDevice graphicsDevice;
    readonly string prefix;
    readonly RenderType renderType;
    readonly ITileRegistry<ITexturedTile> tileRegistry;
    readonly IntDimension tileSize;

    public BlendingTileGenerator(GraphicsDevice graphicsDevice,
                                 ITileRegistry<ITexturedTile> tileRegistry,
                                 RenderType renderType,
                                 IntDimension tileSize,
                                 string prefix = "t.blend.gen.",
                                 string ditherTileName = "t.dither_tile")
    {
      this.graphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));
      this.tileRegistry = tileRegistry ?? throw new ArgumentNullException(nameof(tileRegistry));
      this.renderType = renderType;
      this.tileSize = tileSize;
      this.prefix = prefix ?? "t.blend.gen";

      var ditherTileNameResolved = ditherTileName ?? "t.dither_tile";
      ditherMask = tileRegistry.Find(ditherTileNameResolved) ??
                   throw new IndexOutOfRangeException(
                     $"Expected to find tile '{ditherTileNameResolved}' in base registry.");
      blank = CreateBlank(tileSize);
      builder = new MultiTextureAtlasBuilder(graphicsDevice);
    }

    static Color[] CreateBlank(IntDimension rect)
    {
      var data = new Color[rect.Width * rect.Height];
      for (var i = 0; i < data.Length; i++)
      {
        data[i] = Color.TransparentBlack;
      }

      return data;
    }

    public ITexturedTile Generate(string tag, CardinalIndex direction)
    {
      var etag = $"{prefix}{tag}_{direction}";
      if (tileRegistry.TryFind(etag, out var predefined))
      {
        return predefined ?? throw new ArgumentNullException();
      }

      var sourceArea = TextureOperations.TileAreaForCardinalDirection(tileSize, direction);
      var effectiveMask = FindSourceMask(direction);
      var texture = new Texture2D(graphicsDevice, tileSize.Width, tileSize.Height);
      tileRegistry.TryFind(tag, out var terrain);
      if (terrain?.Texture == null)
      {
        texture.SetData(blank);
      }
      else
      {
        var data = TextureOperations.ExtractData(terrain.Texture, sourceArea);
        texture.Apply(TextureOperations.Combine(data, effectiveMask));
      }

      var tileTexture =
        builder.Insert(new TileTexture(etag, texture, new Rectangle(0, 0, tileSize.Width, tileSize.Height)));
      return new TexturedTile(etag, tileTexture, new Vector2(tileSize.Width / 2f, tileSize.Height / 2f));
    }

    BoundedTextureData FindSourceMask(CardinalIndex dir)
    {
      var cardReg = CardinalIndexTileRegistry<ITexturedTile>.CreateLong(tileRegistry);
      if (cardReg.TryFind("mask.blending", dir, out var providedTile))
      {
        var tex = providedTile.Texture;
        if (tex != null)
        {
          return TextureOperations.ExtractData(providedTile.Texture, providedTile.Texture.Bounds.ToTileRect());
        }
      }

      var sourceArea = TextureOperations.TileAreaForCardinalDirection(tileSize, dir);
      if (renderType.IsIsometric())
      {
        if (ditherMask?.Texture != null)
        {
          return TextureOperations.ExtractData(ditherMask.Texture, sourceArea);
        }
      }

      return new BoundedTextureData(sourceArea, CreateBlank(tileSize));
    }
  }
}