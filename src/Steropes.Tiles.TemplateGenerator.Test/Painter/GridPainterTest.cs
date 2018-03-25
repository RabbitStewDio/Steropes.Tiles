using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.TemplateGenerator.Layout;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Test.Painter
{
  public class GridPainterTest
  {
    Color border = Color.Red;
    Color background = Color.Blue;
    Color outline = Color.Gray;
    TextureGrid grid;

    [SetUp]
    public void SetUp()
    {
      var parent = new TextureCollection()
      {
        Parent = new TextureFile()
        {
          Width = 8,
          Height = 4,
          TileType = TileType.Isometric
        },
      };

      grid = new TextureGrid()
      {
        MatcherType = MatcherType.Basic
      };
      grid.Tiles.Add(new TextureTile(false, 0, 0));
      grid.Tiles.Add(new TextureTile(false, 1, 0));
      grid.FormattingMetaData.Border = 1;
      grid.FormattingMetaData.BorderColor = border;
      grid.FormattingMetaData.Padding = 1;
      grid.FormattingMetaData.BackgroundColor = background;
      grid.TextureTileFormattingMetaData.TileOutlineColor = outline;
      grid.CellSpacing = 1;

      parent.Grids.Add(grid);
    }

    readonly GeneratorPreferences prefs = new GeneratorPreferences();

    [Test]
    public void ValidateBitmapSize()
    {
      GridCollectionPainter p = new GridCollectionPainter(prefs);
      var btmp = p.Produce(grid.Parent);

      btmp.Width.Should().Be(16 + 5);
      btmp.Height.Should().Be(4 + 4);
    }

    [Test]
    public void ValidateBitmapBorder()
    {
      GridCollectionPainter p = new GridCollectionPainter(prefs);
      var btmp = p.Produce(grid.Parent);

      btmp.Width.Should().Be(16 + 5);
      btmp.Height.Should().Be(4 + 4);

      btmp.GetPixel(0, 5).Should().BeSameColor(border);
      btmp.GetPixel(btmp.Width - 1, 5).Should().BeSameColor(border);
      btmp.GetPixel(5, 0).Should().BeSameColor(border);
      btmp.GetPixel(5, btmp.Height - 1).Should().BeSameColor(border);
    }

    [Test]
    public void ValidateNoOverdrawEvenWidth()
    {
      GridCollectionPainter p = new GridCollectionPainter(prefs);
      var btmp = p.Produce(grid.Parent);

      var asText = Print(btmp);
      Console.WriteLine(asText);

      btmp.Width.Should().Be(16 + 5);
      btmp.Height.Should().Be(4 + 4);

      var goldenSample = "ccccccccccccccccccccc\n" +
                         "c...................c\n" +
                         "c.###dd###.###dd###.c\n" +
                         "c.#dd##dd#.#dd##dd#.c\n" +
                         "c.#dd##dd#.#dd##dd#.c\n" +
                         "c.###dd###.###dd###.c\n" +
                         "c...................c\n" +
                         "ccccccccccccccccccccc\n";

      asText.Should().Be(goldenSample);
    }

    [Test]
    public void ValidateHighlightPositions()
    {
      grid.Parent.Parent.Width = 32;
      grid.Parent.Parent.Height = 16;
      grid.MatcherType = MatcherType.CardinalFlags;
      grid.Tiles[0].SelectorHint = "0101";
      grid.Tiles[1].SelectorHint = "1010";

      var painter = new IsoTilePainter(prefs, grid);
      var area = painter.GetTileArea(grid.Tiles[0]);
      var baseShape = IsoTilePainter.CreateShape(area);
      baseShape.GetHighlightFor(NeighbourIndex.North).Should().BeEquivalentTo(new Point(15, 0), new Point(30, 7));
      baseShape.GetHighlightFor(NeighbourIndex.West).Should().BeEquivalentTo(new Point(1, 7), new Point(16, 0));

      var highlight = baseShape.ToHighlight();
      highlight.GetHighlightFor(NeighbourIndex.North).Should().BeEquivalentTo(new Point(15, 2), new Point(26, 7));
      highlight.GetHighlightFor(NeighbourIndex.West).Should().BeEquivalentTo(new Point(5, 7), new Point(16, 2));
    }

    [Test]
    public void ValidateHighlightsEvenWidth_Cardinals()
    {
      GridCollectionPainter p = new GridCollectionPainter(prefs);
      grid.Parent.Parent.Width = 16;
      grid.Parent.Parent.Height = 8;
      grid.MatcherType = MatcherType.CardinalFlags;
      grid.Tiles[0].SelectorHint = "0101";
      grid.Tiles[1].SelectorHint = "1010";

      var btmp = p.Produce(grid.Parent);

      var asText = Print(btmp);
      Console.WriteLine(asText);

      var goldenSample =
        "ccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccc\n" +
        "c.....................................................................c\n" +
        "c.#######dd#######.#######dd#######...................................c\n" +
        "c.#####dd##dd#####.#####dd##dd#####...................................c\n" +
        "c.###dd##ee##dd###.###dd##ee##dd###...................................c\n" +
        "c.#dd##ee######dd#.#dd######ee##dd#...................................c\n" +
        "c.#dd######ee##dd#.#dd##ee######dd#...................................c\n" +
        "c.###dd##ee##dd###.###dd##ee##dd###...................................c\n" +
        "c.#####dd##dd#####.#####dd##dd#####...................................c\n" +
        "c.#######dd#######.#######dd#######...................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "c.....................................................................c\n" +
        "ccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccc\n";

      asText.Should().Be(goldenSample);
    }

    [Test]
    public void ValidateNoOverdrawOddWidth()
    {
      GridCollectionPainter p = new GridCollectionPainter(prefs);

      grid.Parent.Parent.Width = 7;
      grid.Parent.Parent.Height = 5;
      grid.CellWidth = 10;
      grid.CellHeight = 10;
      var btmp = p.Produce(grid.Parent);

      var asText = Print(btmp);
      Console.WriteLine(asText);

      btmp.Width.Should().Be(20 + 5);
      btmp.Height.Should().Be(10 + 4);

      var goldenSample =
        "ccccccccccccccccccccccccc\n" +
        "c.......................c\n" +
        "c.....d..........d......c\n" +
        "c...dd.dd......dd.dd....c\n" +
        "c..d.....d....d.....d...c\n" +
        "c..ddd.ddd....ddd.ddd...c\n" +
        "c..d..d..d....d..d..d...c\n" +
        "c..d##d##d....d##d##d...c\n" +
        "c..ddddddd....ddddddd...c\n" +
        "c..d##d##d....d##d##d...c\n" +
        "c..#ddddd#....#ddddd#...c\n" +
        "c..###d###....###d###...c\n" +
        "c.......................c\n" +
        "ccccccccccccccccccccccccc\n";

      asText.Should().Be(goldenSample);
    }

    public string Print(Bitmap b)
    {
      var colorMappings = new Dictionary<int, char>
      {
        [Color.Empty.ToArgb()] = '.',
        [background.ToArgb()] = '#'
      };

      var sb = new StringBuilder();
      for (int y = 0; y < b.Height; y += 1)
      {
        for (int x = 0; x < b.Width; x += 1)
        {
          var c = b.GetPixel(x, y);
          if (colorMappings.TryGetValue(c.ToArgb(), out var p))
          {
            sb.Append(p);
          }
          else
          {
            var px = (char) ('a' + colorMappings.Count);
            colorMappings[c.ToArgb()] = px;
            sb.Append(px);
          }
        }

        sb.Append("\n");
      }

      return sb.ToString();
    }

    [Test]
    public void ValidateCellSize()
    {
      grid.Parent.Parent.Width = 32;
      grid.Parent.Parent.Height = 16;

      var painter = new IsoTilePainter(prefs, grid);
      var area = painter.GetTileArea(grid.Tiles[0]);
      area.Should().Be(new Rectangle(0, 0, 32, 16));
    }

    [Test]
    public void ValidateIsoShape()
    {
      grid.Parent.Parent.Width = 32;
      grid.Parent.Parent.Height = 16;

      var painter = new IsoTilePainter(prefs, grid);
      var area = painter.GetTileArea(grid.Tiles[0]);
      var shape = IsoTilePainter.CreateShape(area);
      shape.Should().BeAssignableTo<PixelPerfectIsoShape>();

      shape.Top.Should().Be(new Point(16, 0));
      shape.Left.Should().Be(new Point(0, 8));
      shape.Right.Should().Be(new Point(31, 8));
      shape.Bottom.Should().Be(new Point(16, 15));
    }

    [Test]
    public void ValidateIsoShapeNonStandard()
    {
      grid.Parent.Parent.Width = 30;
      grid.Parent.Parent.Height = 16;

      var painter = new IsoTilePainter(prefs, grid);
      var area = painter.GetTileArea(grid.Tiles[0]);
      var shape = IsoTilePainter.CreateShape(area);

      shape.Top.Should().Be(new Point(15, 0));
      shape.Left.Should().Be(new Point(0, 8));
      shape.Right.Should().Be(new Point(29, 8));
      shape.Bottom.Should().Be(new Point(15, 15));
    }
  }
}