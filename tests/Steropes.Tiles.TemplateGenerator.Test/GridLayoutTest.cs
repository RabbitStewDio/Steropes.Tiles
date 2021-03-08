using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using Steropes.Tiles.TemplateGenerator.Layout;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Test
{
  public class GridLayoutTest
  {
    TextureCollection collection;

    [SetUp]
    public void SetUp()
    {
      collection = new TextureCollection
      {
        Parent = new TextureFile
        {
          Width = 64,
          Height = 32
        }
      };
    }

    [Test]
    public void Test4x4()
    {
      collection.Grids.Add(new TextureGrid
      {
        MatcherType = MatcherType.CardinalFlags,
        Name = "Grid-A"
      });
      collection.Grids.Add(new TextureGrid
      {
        MatcherType = MatcherType.CardinalFlags,
        Name = "Grid-B"
      });
      collection.Grids.Add(new TextureGrid
      {
        MatcherType = MatcherType.CardinalFlags,
        Name = "Grid-C"
      });
      collection.Grids.Add(new TextureGrid
      {
        MatcherType = MatcherType.CardinalFlags,
        Name = "Grid-D"
      });

      GridGenerator g = new GridGenerator();
      g.Regenerate(collection);
      GridLayouter l = new GridLayouter(new GeneratorPreferences());
      l.ArrangeGrids(collection);

      collection.Grids[0].Position.Should().Be(new Point());
      collection.Grids[1].Position.Should().Be(new Point(0, 128));
      collection.Grids[2].Position.Should().Be(new Point(0, 256));
      collection.Grids[3].Position.Should().Be(new Point(256, 0));
    }

    [Test]
    public void TestSplitQuad()
    {
      ArrangeNode<string> root = new ArrangeNode<string>(100, 100);
      root.Insert(new Size(10, 10), "A");
      root.Insert(new Size(10, 10), "B");
      root.Insert(new Size(10, 10), "C");

      var ip = root.FindInsertPosition(new Size(10, 10));
      ip.Content.Should().BeNull();
      ip.X.Should().Be(10);
      ip.Y.Should().Be(10);

    }

    [Test]
    public void TestSplitLandscape()
    {
      ArrangeNode<string> root = new ArrangeNode<string>(100, 90);
      var split = root.Split(new Size(10, 5));
      var left = split.Item1;
      var bottom = split.Item2;

      Console.WriteLine(root);
      Console.WriteLine("  left   = " + left);
      Console.WriteLine("  bottom = " + bottom);

      left.SpaceAvailableY.Should().Be(5);
      bottom.SpaceAvailableX.Should().Be(100);
    }

    [Test]
    public void TestSplitPortrait()
    {
      ArrangeNode<string> root = new ArrangeNode<string>(90, 100);
      var split = root.Split(new Size(10, 5));
      var left = split.Item1;
      var bottom = split.Item2;

      left.SpaceAvailableY.Should().Be(5);
      bottom.SpaceAvailableX.Should().Be(90);
    }

    [Test]
    public void TestSplitSquare()
    {
      ArrangeNode<string> root = new ArrangeNode<string>(100, 100);
      var split = root.Split(new Size(10, 5));
      var left = split.Item1;
      var bottom = split.Item2;

      left.SpaceAvailableY.Should().Be(5);
      bottom.SpaceAvailableX.Should().Be(100);
    }
  }
}