using System;
using System.Collections.Generic;
using Steropes.Tiles.DataStructures;

namespace Steropes.Tiles.TexturePack
{
  public class TexturePack<TTile> : ITexturePack<TTile> 
  {
    public string Name { get; }
    public List<ITextureFile<TTile>> TextureFiles { get; }
    public IntDimension TileSize { get; }
    public TextureType TextureType { get; }
    public TTile Empty { get; }

    public TexturePack(string name,
                       IntDimension tileSize,
                       TextureType textureType,
                       params ITextureFile<TTile>[] textureFiles)
    {
      TileSize = tileSize;
      TextureType = textureType;
      if (textureFiles == null)
      {
        throw new ArgumentNullException(nameof(textureFiles));
      }
      Name = name ?? throw new ArgumentNullException(nameof(name));
      TextureFiles = new List<ITextureFile<TTile>>(textureFiles);
    }

    public IEnumerable<TTile> Tiles => ProduceTiles();

    IEnumerable<TTile> ProduceTiles()
    {
        foreach (var f in TextureFiles)
        {
            foreach (var x in f.ProduceTiles())
            {
                yield return x;
            }

        }
    }
  }
}