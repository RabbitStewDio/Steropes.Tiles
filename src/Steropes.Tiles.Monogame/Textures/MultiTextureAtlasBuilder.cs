using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steropes.Tiles.Monogame.Tiles;

namespace Steropes.Tiles.Monogame.Textures
{
  public class MultiTextureAtlasBuilder
  {
    readonly GraphicsDevice device;
    readonly int size;
    readonly List<TextureAtlasBuilder> builders;

    public MultiTextureAtlasBuilder(GraphicsDevice device, int size = TextureAtlasBuilder.MaxTextureSize)
    {
      this.device = device;
      this.size = size;
      builders = new List<TextureAtlasBuilder>();
    }

    public ITexture Insert(ITexture tile)
    {
      foreach (var b in builders)
      {
        if (b.Insert(tile, out ITexture result))
        {
          return result;
        }
      }
    
      var rt = new RenderTarget2D(device, size, size, false, device.PresentationParameters.BackBufferFormat, DepthFormat.None, 1, RenderTargetUsage.PreserveContents);
      device.SetRenderTarget(rt);
      device.Clear(Color.Transparent);
      device.SetRenderTarget(null);

      var b2 = new TextureAtlasBuilder(rt, rt.Bounds);
      if (b2.Insert(tile, out ITexture result2))
      {
        builders.Add(b2);
      }
      return result2;
    }
  }
}