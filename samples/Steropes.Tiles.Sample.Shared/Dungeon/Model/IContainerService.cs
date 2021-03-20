using Steropes.Tiles.Util;

namespace Steropes.Tiles.Demo.Core.GameData.Dungeon.Model
{
  public interface IContainerService
  {
    bool Add(IContainerReference container, IItem entity);
    bool Remove(IContainerReference container, IItem entity);
    ReadOnlyListWrapper<IItem> Contents(IContainerReference container);
    bool Transfer(IContainerReference sourceContainer, IContainerReference targetContainer, IItem entity);
  }

  public interface IContainerReferenceProvider<T>
  {
    IContainerReference From(T item);
  }
}