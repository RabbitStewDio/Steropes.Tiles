namespace Steropes.Tiles.Sample.Shared.Util
{
    public interface IMap2D<out TEntity>
    {
        TEntity this[int x, int y] { get; }
    }
}
