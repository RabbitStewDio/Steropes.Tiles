using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Monogame.Tiles;

namespace Steropes.Tiles.Monogame.Test
{
  public class LoaderTest
  {
    readonly string path = "terrain.tiles";
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
      var pack = tp.LoadTexturePack(new MockContentLoader(game)) as BasicTileRegistry<ITexturedTile>;
      var textures = pack.ToList();
      textures.Count.Should().Be(84);
    }

    class TestGame : Game
    {
      public TestGame()
      {
        Graphics = new GraphicsDeviceManager(this);
      }

      public GraphicsDeviceManager Graphics { get; }
    }
  }
  

  class DummyMain
  {
    public static void Main()
    {

    }
  }
}