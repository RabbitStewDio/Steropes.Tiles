using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NUnit.Framework;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Monogame.Tiles;

namespace Steropes.Tiles.Monogame.Test
{
  public class LoaderTest
  {
    class LoggingContentLoader : IContentLoader
    {
      readonly Game game;
      List<string> loadedFiles;

      public LoggingContentLoader(Game game)
      {
        this.game = game;
        this.loadedFiles = new List<string>();
      }

      public Texture2D LoadTexture(string name)
      {
        return new Texture2D(game.GraphicsDevice, 10, 10);
      }
    }

    class TestGame: Game
    {
      public TestGame()
      {
        Graphics = new GraphicsDeviceManager(this);
      }

      public GraphicsDeviceManager Graphics { get; }
    }

    string path = "terrain.tiles";
    Game game;

    [SetUp]
    public void SetUp()
    {
      game = new TestGame();
      game.RunOneFrame();
    }

    [TearDown]
    public void TearDown()
    {
      game.Dispose();
    }

    [Test]
    public void TestLoading()
    {
      var test = TestContext.CurrentContext.TestDirectory;

      var tp = TexturePackLoader.Read(Path.Combine(test, path));
      var pack = tp.LoadTexturePack(new LoggingContentLoader(game)) as BasicTileRegistry<ITexturedTile>;
      var textures = pack.ToList();
      textures.Count.Should().Be(84);
    }

  }
}