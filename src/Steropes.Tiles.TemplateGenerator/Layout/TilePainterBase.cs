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



    public TextureGrid Grid { get; }

    public abstract void Draw(Graphics g, TextureTile tile);

    protected void DrawCellFrame(Graphics g)
    {
      var bg = Grid.FormattingMetaData.BackgroundColor;
      if (bg != null)
      {
        var size = Grid.EffectiveCellSize;
        var pen = new Pen(bg.Value);
        g.DrawRectangle(pen, new Rectangle(size.Width / 2, size.Height / 2, size.Width, size.Height));
        pen.Dispose();
      }
    }

    protected virtual void DrawIndexedDirection(Graphics g, NeighbourIndex idx)
    {
    }

    protected virtual void DrawSubCell(Graphics g, Direction direction, Color c)
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
          DrawSubCell(g, (Direction) i, c);
        }
      }
    }

    void DrawCornerMap(Graphics g, TextureTile t)
    {
      var hint = (t.SelectorHint ?? "").PadLeft(6);

      void IfFlagSet(int pos, Action a)
      {
        var c = hint[pos];
        if (c == '1')
        {
          a();
        }
      }

      var direction = hint[0];
      if (direction == 'U')
      {
        IfFlagSet(2, () => DrawIndexedDirection(g, NeighbourIndex.West));
        IfFlagSet(3, () => DrawIndexedDirection(g, NeighbourIndex.NorthWest));
        IfFlagSet(4, () => DrawIndexedDirection(g, NeighbourIndex.North));
      }
      else if (direction == 'L')
      {
        IfFlagSet(2, () => DrawIndexedDirection(g, NeighbourIndex.North));
        IfFlagSet(3, () => DrawIndexedDirection(g, NeighbourIndex.NorthEast));
        IfFlagSet(4, () => DrawIndexedDirection(g, NeighbourIndex.East));
      }
      else if (direction == 'B')
      {
        IfFlagSet(2, () => DrawIndexedDirection(g, NeighbourIndex.East));
        IfFlagSet(3, () => DrawIndexedDirection(g, NeighbourIndex.SouthEast));
        IfFlagSet(4, () => DrawIndexedDirection(g, NeighbourIndex.South));
      }
      else if (direction == 'R')
      {
        IfFlagSet(2, () => DrawIndexedDirection(g, NeighbourIndex.South));
        IfFlagSet(3, () => DrawIndexedDirection(g, NeighbourIndex.SouthWest));
        IfFlagSet(4, () => DrawIndexedDirection(g, NeighbourIndex.West));
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

      IfFlagSet(CardinalIndex.North, () => DrawIndexedDirection(g, NeighbourIndex.North));
      IfFlagSet(CardinalIndex.East, () => DrawIndexedDirection(g, NeighbourIndex.East));
      IfFlagSet(CardinalIndex.South, () => DrawIndexedDirection(g, NeighbourIndex.South));
      IfFlagSet(CardinalIndex.West, () => DrawIndexedDirection(g, NeighbourIndex.West));
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

      IfFlagSet(DiagonalIndex.NorthWest, () => DrawIndexedDirection(g, NeighbourIndex.NorthWest));
      IfFlagSet(DiagonalIndex.NorthEast, () => DrawIndexedDirection(g, NeighbourIndex.NorthEast));
      IfFlagSet(DiagonalIndex.SouthEast, () => DrawIndexedDirection(g, NeighbourIndex.SouthEast));
      IfFlagSet(DiagonalIndex.SouthWest, () => DrawIndexedDirection(g, NeighbourIndex.SouthWest));
    }

    void DrawIndexedDirectionHint(Graphics g, TextureTile t)
    {
      var text = t.SelectorHint ?? "";
      // I intentionally parse the NeighbourMatchIndex value as NeighbourIndex so that the parsing 
      // ignores the "isolated" tile. 
      if (Enum.TryParse(text, out NeighbourIndex idx))
      {
        DrawIndexedDirection(g, idx);
      }
    }
  }
}