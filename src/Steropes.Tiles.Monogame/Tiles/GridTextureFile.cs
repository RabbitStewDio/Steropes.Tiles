using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Steropes.Tiles.Monogame.Tiles
{
  /// <summary>
  ///   A texture file that contains one ore more regular grids. All tiles inside the
  ///   grid have a uniform size and anchor point.
  ///   FreeCiv uses this schema for its graphics packs.
  /// </summary>
  public class GridTextureFile : ITextureFile
  {
    public GridTextureFile(string name, params TileGrid[] grids)
    {
      if (grids == null)
      {
        throw new ArgumentNullException(nameof(grids));
      }
      Name = name ?? throw new ArgumentNullException(nameof(name));
      Grids = new List<TileGrid>(grids);
    }

    public string Name { get; }
    public List<TileGrid> Grids { get; }
  
    IEnumerator<ITexturedTile> ProduceTexturesIt(IContentLoader loader)
    {
      var texture = loader.LoadTexture(Name);
      foreach (var grid in Grids)
      {
        var width = grid.TileWidth;
        var height = grid.TileHeight;
        foreach (var tile in grid.Tiles)
        {
          var tileX = grid.OffsetX + tile.GridX * (width + grid.BorderX);
          var tileY = grid.OffsetY + tile.GridY * (height + grid.BorderY);
          var anchor = new Vector2(tile.AnchorX ?? grid.AnchorX, tile.AnchorY ?? grid.AnchorY);
          var bounds = new Rectangle(tileX, tileY, width, height);
          var boundedTexture = new TileTexture(Name, texture.Tex2D, bounds);
          foreach (var tileTag in tile.Tags)
          {
            yield return new TexturedTile(tileTag, boundedTexture, anchor);
          }
        }
      }
    }

    public IEnumerable<ITexturedTile> ProduceTextures(IContentLoader loader)
    {
      return new InternalEnumerable(loader, this);
    }

    class InternalEnumerable: IEnumerable<ITexturedTile>
    {
      readonly IContentLoader loader;
      readonly GridTextureFile textureFile;

      public InternalEnumerable(IContentLoader loader, GridTextureFile textureFile)
      {
        this.loader = loader ?? throw new ArgumentNullException(nameof(loader));
        this.textureFile = textureFile ?? throw new ArgumentNullException(nameof(textureFile));
      }


      public IEnumerator<ITexturedTile> GetEnumerator()
      {
        return textureFile.ProduceTexturesIt(loader);
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return GetEnumerator();
      }
    }
  }
}