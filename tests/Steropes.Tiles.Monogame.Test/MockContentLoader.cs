using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steropes.Tiles.Monogame.Tiles;

namespace Steropes.Tiles.Monogame.Test
{
  class MockContentLoader : IContentLoader
  {
    readonly Game game;

    public MockContentLoader(Game game)
    {
      this.game = game;
    }

    public ITexture LoadTexture(string name)
    {
      return new TileTexture(name,
                             new Texture2D(game.GraphicsDevice, 10, 10),
                             new Rectangle(0, 0, 10, 10));
    }
  }
}