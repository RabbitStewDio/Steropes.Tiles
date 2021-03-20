namespace Steropes.Tiles.Demo.Core.GameData.Util
{
  public interface IMap2D<out TEntity>
  {
    TEntity this[int x, int y] { get; }
  }
}