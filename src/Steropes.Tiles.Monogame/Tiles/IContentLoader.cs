using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Steropes.Tiles.Monogame.Tiles
{
  public interface IContentLoader
  {
    Texture2D LoadTexture(string name);
  }

  public class MonogameContentLoader : IContentLoader
  {
    readonly ContentManager mgr;
    readonly string basePath;

    public MonogameContentLoader(ContentManager mgr, string basePath = null)
    {
      this.mgr = mgr;
      this.basePath = basePath;
    }

    public Texture2D LoadTexture(string name)
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

      return mgr.Load<Texture2D>(path);
    }
  }
}