namespace Steropes.Tiles.Demo.Core.GameData.Dungeon.Model
{
    public interface IItemContainerService
    {
        IContainerReference ContainerOf(IItem item);
    }
}
