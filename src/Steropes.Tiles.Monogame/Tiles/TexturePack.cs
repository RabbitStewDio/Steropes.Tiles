using System;
using System.Collections.Generic;
using Steropes.Tiles.DataStructures;

namespace Steropes.Tiles.Monogame.Tiles
{
  public class TexturePack : ITexturePack
  {
    public string Name { get; }
    public List<ITextureFile> TextureFiles { get; }
    public IntDimension TileSize { get; }
    public TextureType TextureType { get; }
    public string BasePath { get; }

    public TexturePack(string name,
                       IntDimension tileSize,
                       TextureType textureType,
                       string basePath,
                       params ITextureFile[] textureFiles)
    {
      TileSize = tileSize;
      TextureType = textureType;
      BasePath = basePath;
      if (textureFiles == null)
      {
        throw new ArgumentNullException(nameof(textureFiles));
      }
      Name = name ?? throw new ArgumentNullException(nameof(name));
      TextureFiles = new List<ITextureFile>(textureFiles);
    }
  }
}