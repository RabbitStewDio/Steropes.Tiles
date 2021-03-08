namespace Steropes.Tiles.MonogameDemo.GameData.Dungeon.Model
{
  public interface IItemContainerService
  {
    IContainerReference ContainerOf(IItem item);
  }
}