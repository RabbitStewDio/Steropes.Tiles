using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Steropes.Tiles.Monogame.Tiles
{
  public class MonogameContentLoader : IContentLoader
  {
    readonly ContentManager mgr;
    readonly string basePath;

    public MonogameContentLoader(ContentManager mgr, string basePath = null)
    {
      this.mgr = mgr;
      this.basePath = basePath;
    }

    public ITexture LoadTexture(string name)
    {
      string path;
      if (basePath != null)
      {
        path = $"{basePath}/{name}";
      }
      else
      {
        path = name;
      }

      
      return new TextureHandle(path, mgr.Load<Texture2D>(path));
    }

    class TextureHandle: ITexture
    {
      public TextureHandle(string name, Texture2D texture)
      {
        Tex2D = texture ?? throw new ArgumentNullException(nameof(texture));
        Name = name ?? throw new ArgumentNullException(nameof(name));
      }

      public Texture2D Tex2D { get; }
      public Rectangle Bounds => Tex2D.Bounds;

      public string Name { get; }
    }
  }
}