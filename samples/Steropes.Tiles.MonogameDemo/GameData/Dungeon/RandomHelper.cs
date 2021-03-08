using System;

namespace Steropes.Tiles.MonogameDemo.GameData.Dungeon
{
  public static class RandomHelper
  {
    public static double Next(this Random r, double range)
    {
      return r.NextDouble() * range;
    }
  }
}