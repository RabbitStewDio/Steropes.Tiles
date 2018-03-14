using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using Steropes.Tiles.TemplateGenerator.Layout;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Test.Model
{
  public class LayoutNodeTest
  {
    GeneratorPreferences prefs = new GeneratorPreferences();

    [Test]
    public void TextureGridEffectiveSize_When_Corner()
    {
      TextureGrid g = new TextureGrid()
      {
        Parent = new TextureCollection()
        {
          Parent = new TextureFile()
          {
            Width = 64,
            Height = 32,
            TileType = TileType.Isometric
          },
        },
        MatcherType = MatcherType.Corner
      };

      g.EffectiveTileSize.Should().Be(new Size(32, 16));
      g.EffectiveCellSize.Should().Be(new Size(32, 16));
    }

    [Test]
    public void TextureGridEffectiveSize_When_NotCorner()
    {
      TextureGrid g = new TextureGrid()
      {
        Parent = new TextureCollection()
        {
          Parent = new TextureFile()
          {
            Width = 64,
            Height = 32,
            TileType = TileType.Isometric
          },
        },
        MatcherType = MatcherType.Basic
      };

      g.EffectiveTileSize.Should().Be(new Size(64, 32));
      g.EffectiveCellSize.Should().Be(new Size(64, 32));
    }

    [Test]
    public void SizeValidation_WhenCorner()
    {
      TextureGrid g = new TextureGrid()
      {
        Parent = new TextureCollection()
        {
          Parent = new TextureFile()
          {
            Width = 64,
            Height = 32,
            TileType = TileType.Isometric
          },
        },
        MatcherType = MatcherType.Corner
      };

      var size = g.EffectiveCellSize;
      var node = new LayoutNode(prefs, g);
      node.Size.Should().Be(new Size(8 * size.Width, 4 * size.Height));
    }
  }
}