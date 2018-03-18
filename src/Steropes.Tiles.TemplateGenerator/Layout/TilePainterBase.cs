using System;
using System.Drawing;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout
{
  public abstract class TilePainterBase : ITilePainter
  {
    public GeneratorPreferences Preferences { get; }

    protected TilePainterBase(GeneratorPreferences preferences, TextureGrid grid)
    {
      this.Preferences = preferences;
      Grid = grid ?? throw new ArgumentNullException(nameof(grid));
    }

    public Rectangle GetTileArea(TextureTile tile)
    {
      var tileSize = Grid.EffectiveTileSize;
      return new Rectangle(0, 0, tileSize.Width, tileSize.Height);
    }

    public TextureGrid Grid { get; }

    public abstract void Draw(Graphics g, TextureTile tile);

    protected void DrawCellFrame(Graphics g, TextureTile tile)
    {
      var bg = Grid.FormattingMetaData.BackgroundColor ?? Preferences.DefaultBackgroundColor;
      if (bg != null)
      {
        var pen = new SolidBrush(bg.Value);
        g.FillRectangle(pen, GetTileArea(tile));
        pen.Dispose();
      }
    }

    protected virtual void DrawIndexedDirection(Graphics g, TextureTile tile, NeighbourIndex idx)
    {
    }

    protected virtual void DrawSubCell(Graphics g, TextureTile tile, Direction direction, Color c)
    {
    }

    void DrawCellMap(Graphics g, TextureTile t)
    {
      var hint = (t.SelectorHint ?? "").Split(',');
      for (var i = 0; i < hint.Length; i++)
      {
        if (int.TryParse(hint[i], out var colorIndex) &&
            colorIndex >= 0 &&
            colorIndex < Preferences.TileColors.Count)
        {
          var c = Preferences.TileColors[colorIndex];
          DrawSubCell(g, t, (Direction) i, c);
        }
      }
    }

    protected static int Lerp(int left, int right, int quarterFraction)
    {
      return left + (right - left) * quarterFraction / 4;
    }

    protected static Rectangle FromEdges(int left, int top, int right, int bottom)
    {
      return new Rectangle(left, top, right - left, bottom - top);
    }

    void DrawCornerMap(Graphics g, TextureTile t)
    {
      var hint = (t.SelectorHint ?? "").PadLeft(5);

      void IfFlagSet(int pos, Action a)
      {
        var c = hint[pos];
        if (c == '1')
        {
          a();
        }
      }

      var highlightColor = t.Parent?.TextureTileFormattingMetaData?.TileHighlightColor ?? Preferences.DefaultTileHighlightColor;
      var direction = hint[0];
      if (direction == 'U')
      {
        IfFlagSet(2, () => DrawSubCell(g, t, Direction.Left, highlightColor));
        IfFlagSet(3, () => DrawSubCell(g, t, Direction.Up, highlightColor));
        IfFlagSet(4, () => DrawSubCell(g, t, Direction.Right, highlightColor));
      }
      else if (direction == 'L')
      {
        IfFlagSet(2, () => DrawSubCell(g, t, Direction.Up, highlightColor));
        IfFlagSet(3, () => DrawSubCell(g, t, Direction.Right, highlightColor));
        IfFlagSet(4, () => DrawSubCell(g, t, Direction.Down, highlightColor));
      }
      else if (direction == 'D')
      {
        IfFlagSet(2, () => DrawSubCell(g, t, Direction.Right, highlightColor));
        IfFlagSet(3, () => DrawSubCell(g, t, Direction.Down, highlightColor));
        IfFlagSet(4, () => DrawSubCell(g, t, Direction.Left, highlightColor));
      }
      else if (direction == 'R')
      {
        IfFlagSet(2, () => DrawSubCell(g, t, Direction.Down, highlightColor));
        IfFlagSet(3, () => DrawSubCell(g, t, Direction.Left, highlightColor));
        IfFlagSet(4, () => DrawSubCell(g, t, Direction.Up, highlightColor));
      }
    }

    protected void DrawSelectorHint(Graphics g, TextureTile t)
    {
      var matcherType = t.Parent?.MatcherType ?? MatcherType.Basic;
      switch (matcherType)
      {
        case MatcherType.Basic:
          break;
        case MatcherType.CardinalFlags:
          DrawCardinalFlagHint(g, t);
          break;
        case MatcherType.CardinalIndex:
          DrawIndexedDirectionHint(g, t);
          break;
        case MatcherType.CellMap:
          DrawCellMap(g, t);
          break;
        case MatcherType.Corner:
          DrawCornerMap(g, t);
          break;
        case MatcherType.DiagonalFlags:
          DrawDiagonalFlagHint(g, t);
          break;
        case MatcherType.NeighbourIndex:
          DrawIndexedDirectionHint(g, t);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    void DrawCardinalFlagHint(Graphics g, TextureTile t)
    {
      var text = (t.SelectorHint ?? "").PadLeft(4);

      void IfFlagSet(CardinalIndex pos, Action a)
      {
        var c = text[(int) pos];
        if (c == '1')
        {
          a();
        }
      }

      IfFlagSet(CardinalIndex.North, () => DrawIndexedDirection(g, t, NeighbourIndex.North));
      IfFlagSet(CardinalIndex.East, () => DrawIndexedDirection(g, t, NeighbourIndex.East));
      IfFlagSet(CardinalIndex.South, () => DrawIndexedDirection(g, t, NeighbourIndex.South));
      IfFlagSet(CardinalIndex.West, () => DrawIndexedDirection(g, t, NeighbourIndex.West));
    }

    void DrawDiagonalFlagHint(Graphics g, TextureTile t)
    {
      var text = (t.SelectorHint ?? "").PadLeft(4);

      void IfFlagSet(DiagonalIndex pos, Action a)
      {
        var c = text[(int) pos];
        if (c == '1')
        {
          a();
        }
      }

      IfFlagSet(DiagonalIndex.NorthWest, () => DrawIndexedDirection(g, t, NeighbourIndex.NorthWest));
      IfFlagSet(DiagonalIndex.NorthEast, () => DrawIndexedDirection(g, t, NeighbourIndex.NorthEast));
      IfFlagSet(DiagonalIndex.SouthEast, () => DrawIndexedDirection(g, t, NeighbourIndex.SouthEast));
      IfFlagSet(DiagonalIndex.SouthWest, () => DrawIndexedDirection(g, t, NeighbourIndex.SouthWest));
    }

    void DrawIndexedDirectionHint(Graphics g, TextureTile t)
    {
      var text = t.SelectorHint ?? "";
      // I intentionally parse the NeighbourMatchIndex value as NeighbourIndex so that the parsing 
      // ignores the "isolated" tile. 
      if (Enum.TryParse(text, out NeighbourIndex idx))
      {
        DrawIndexedDirection(g, t, idx);
      }
    }
  }
}