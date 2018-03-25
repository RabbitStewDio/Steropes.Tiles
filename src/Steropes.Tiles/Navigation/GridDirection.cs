namespace Steropes.Tiles.Navigation
{
  public enum GridDirection
  {
    None = 0,
    North = 1,
    NorthEast = 2,
    East = 3,
    SouthEast = 4,
    South = 5,
    SouthWest = 6,
    West = 7,
    NorthWest = 8
  }

  public enum CardinalIndex
  {
    North = 0,
    East = 1,
    South = 2,
    West = 3,
  }

  public enum DiagonalIndex
  {
    NorthWest = 0,
    NorthEast = 1,
    SouthEast = 2,
    SouthWest = 3,
  }

  public enum NeighbourIndex
  {
    North = 0,
    NorthEast = 1,
    East = 2,
    SouthEast = 3,
    South = 4,
    SouthWest = 5,
    West = 6,
    NorthWest = 7
  }

  public static class IndexConversion
  {
    public static int AsInt(this CardinalIndex c)
    {
      return (int) c;
    }

    public static int AsInt(this NeighbourIndex c)
    {
      return (int) c;
    }
  }
}