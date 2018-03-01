using System;

namespace Steropes.Tiles.Navigation
{
  public class RotationGridNavigator : IMapNavigator<GridDirection>
  {
    readonly IMapNavigator<GridDirection> parent;
    readonly GridDirection[] directions;
    int rotationSteps;

    public RotationGridNavigator(IMapNavigator<GridDirection> parent, int rotationSteps)
    {
      this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
      this.directions = new GridDirection[9];
      RotationSteps = Math.Sign(rotationSteps) * (Math.Abs(rotationSteps) % 8);
    }

    public int RotationSteps
    {
      get { return rotationSteps; }
      set
      {
        rotationSteps = value;
        for (var i = 0; i < directions.Length; i += 1)
        {
          directions[i] = RotateBy((GridDirection)i, rotationSteps);
        }
      }
    }

    public bool NavigateTo(GridDirection direction, MapCoordinate source, out MapCoordinate result, int steps = 1)
    {
      var d = directions[(int)direction];
      return parent.NavigateTo(d, source, out result, steps);
    }

    public static GridDirection RotateBy(GridDirection direction, int steps)
    {
      if (direction == GridDirection.None)
      {
        return direction;
      }

      var directionAsInt = (int) direction - 1; // between 0 and 7
      directionAsInt += 8; // between 8 and 15
      directionAsInt += steps; // rotated, now anywhere between 1 and 22
      directionAsInt = directionAsInt % 8 + 1;
      return (GridDirection) directionAsInt;
    }
  }

  public class FixedOffsetRotationGridNavigator : IMapNavigator<GridDirection>
  {
    readonly GridDirection[] directions;
    readonly IMapNavigator<GridDirection> parent;

    public FixedOffsetRotationGridNavigator(IMapNavigator<GridDirection> parent, int rotationSteps)
    {
      this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
      this.directions = new GridDirection[9];
      for (var i = 0; i < directions.Length; i += 1)
      {
        directions[i] = RotationGridNavigator.RotateBy((GridDirection) i, rotationSteps);
      }
    }

    public bool NavigateTo(GridDirection direction, MapCoordinate source, out MapCoordinate result, int steps = 1)
    {
      var d = directions[(int) direction];
      return parent.NavigateTo(d, source, out result, steps);
    }
  }
}