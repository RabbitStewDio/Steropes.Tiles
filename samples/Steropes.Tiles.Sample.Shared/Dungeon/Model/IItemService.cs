﻿namespace Steropes.Tiles.Demo.Core.GameData.Dungeon.Model
{
    public interface IItemService
    {
        IItem Create(IItemType type);

        bool TraitFor<T>(IItem c, out T trait)
            where T : IItemTrait;

        bool Destroy(IItem item);
    }
}
