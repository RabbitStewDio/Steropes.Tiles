using System;
using Steropes.Tiles.MonogameDemo.GameData.Dungeon.Model;

namespace Steropes.Tiles.MonogameDemo.GameData.Dungeon.Simple
{
  /// <summary>
  ///  Defines the configurable behaviour of floor types. All floors have a certain movement cost and
  ///  may have a health-cost (walking over lava is never fun!). Use Integer.MAX for floors that do
  ///  not allow any movement. 
  /// </summary>
  /// <para>
  ///  The name of a floor type is used as part of the debugging information and for matching the
  ///  map data to the tiles from the graphics pack. 
  /// </para>
  public class FloorType : IFloorType
  {
    public FloorTypeId Id { get; }

    /// <summary>
    /// This is the graphic tag. This string connects the map data to the graphic pack tile.
    /// </summary>
    public string Name { get; }

    public int MovementCost { get; }
    public int HealthCost { get; }

    public FloorType(FloorTypeId id, string name, int movementCost, int healthCost)
    {
      if (movementCost <= 0)
      {
        throw new ArgumentException("Moving at infinite speed is fun, but impossible.");
      }

      Id = id;
      Name = name ?? throw new ArgumentNullException(nameof(name));
      MovementCost = movementCost;
      HealthCost = healthCost;
    }
  }
}