using System;
using Steropes.Tiles.Demo.Core.GameData.Dungeon.Model;
using Steropes.Tiles.Demo.Core.GameData.Util;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Demo.Core.GameData.Dungeon.Simple
{
  public class ItemService : BaseTraitService<IItem>, IItemService
  {
    readonly IContainerService containerService;
    readonly IItemContainerService itemContainerService;
    readonly IContainerReferenceProvider<MapCoordinate> mapReferenceProvider;
    readonly IdGenerator nextId;

    public ItemService(IContainerService containerService,
                       IItemContainerService itemContainerService,
                       IContainerReferenceProvider<MapCoordinate> mapReferenceProvider)
    {
      this.containerService = containerService;
      this.itemContainerService = itemContainerService;
      this.mapReferenceProvider = mapReferenceProvider;
      nextId = new IdGenerator();
    }

    public IItem Create(IItemType type)
    {
      var item = new ItemHandle(nextId.Next(), type);
      AddTrait<ILocationTrait>(item, new InternalLocationTrait(this, item));
      return item;
    }

    public bool Destroy(IItem item)
    {
      var container = itemContainerService.ContainerOf(item);
      if (container == null)
      {
        return true;
      }

      if (containerService.Remove(container, item))
      {
        RemoveAllTraits(item);
        return true;
      }
      return false;
    }

    bool Relocate(IItem item, MapCoordinate target)
    {
      var containerReference = itemContainerService.ContainerOf(item);
      if (containerReference == null)
      {
        return containerService.Add(mapReferenceProvider.From(target), item);
      }
      if (containerReference.ReferenceType == ContainerReferenceType.Map)
      {
        return containerService.Transfer(containerReference, mapReferenceProvider.From(target), item);
      }
      if (containerReference.ReferenceType == ContainerReferenceType.Item)
      {
        // movement within an item is a no-op, but valid.
        // could be used for moving inside a transporter relative to the transport's location on the map.
        return true;
      }
      return false;
    }

    class InternalLocationTrait : LocationTrait
    {
      readonly IItem handle;
      readonly ItemService service;

      public InternalLocationTrait(ItemService service, IItem handle)
      {
        this.handle = handle ?? throw new ArgumentNullException(nameof(handle));
        this.service = service ?? throw new ArgumentNullException(nameof(service));
      }

      protected override bool OnLocationChange()
      {
        var c = OccupiedTile;
        return service.Relocate(handle, c);
      }
    }
  }
}