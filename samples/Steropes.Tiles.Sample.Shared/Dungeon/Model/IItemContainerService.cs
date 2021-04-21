namespace Steropes.Tiles.Sample.Shared.Dungeon.Model
{
    public interface IItemContainerService
    {
        IContainerReference ContainerOf(IItem item);
    }
}
