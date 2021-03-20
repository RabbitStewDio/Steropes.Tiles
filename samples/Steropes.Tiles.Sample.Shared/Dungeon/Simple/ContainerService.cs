using System;
using System.Collections.Generic;
using Steropes.Tiles.Demo.Core.GameData.Dungeon.Model;
using Steropes.Tiles.Util;

namespace Steropes.Tiles.Demo.Core.GameData.Dungeon.Simple
{
    public class ContainerService : IContainerService, IItemContainerService
    {
        readonly Dictionary<IContainerReference, Container> containers;
        readonly Dictionary<IItem, IContainerReference> itemParents;

        public ContainerService()
        {
            containers = new Dictionary<IContainerReference, Container>();
            itemParents = new Dictionary<IItem, IContainerReference>();
        }

        public IContainerReference ContainerOf(IItem item)
        {
            if (itemParents.TryGetValue(item, out IContainerReference container))
            {
                return container;
            }

            return null;
        }

        public bool Add(IContainerReference container, IItem entity)
        {
            if (ContainerOf(entity) != null)
            {
                throw new ArgumentException("This item is already part of an collection.");
            }

            if (!containers.TryGetValue(container, out Container c))
            {
                c = new Container(container);
                c.Add(entity);
                containers[container] = c;
                itemParents[entity] = container;
            }
            else
            {
                c.Add(entity);
                itemParents[entity] = container;
            }

            return true;
        }

        public bool Remove(IContainerReference container, IItem entity)
        {
            if (!containers.TryGetValue(container, out Container c))
            {
                return false;
            }

            var retval = c.Remove(entity);
            itemParents.Remove(entity);
            if (c.IsEmpty)
            {
                containers.Remove(container);
            }

            return retval;
        }

        static readonly ReadOnlyListWrapper<IItem> EmptySingleton = new ReadOnlyListWrapper<IItem>(new List<IItem>());

        // public List<IItem> Contents(IContainerReference container, List<IItem> collector)
        // {
        //     if (!containers.TryGetValue(container, out Container c))
        //     {
        //         if (collector == null)
        //         {
        //             collector = new List<IItem>();
        //         }
        //         else
        //         {
        //             collector.Clear();
        //         }
        //
        //         return collector;
        //     }
        //
        //     return c.Contents(collector);
        // }

        public ReadOnlyListWrapper<IItem> Contents(IContainerReference container)
        {
            if (!containers.TryGetValue(container, out Container c))
            {
                return EmptySingleton;
            }

            return c.Contents();
        }

        public bool Transfer(IContainerReference sourceContainer, IContainerReference targetContainer, IItem entity)
        {
            if (!containers.ContainsKey(sourceContainer))
            {
                return false;
            }

            if (Equals(sourceContainer, targetContainer))
            {
                // nothing to do.
                return true;
            }

            if (CanAdd(targetContainer, entity))
            {
                if (Remove(sourceContainer, entity))
                {
                    if (Add(targetContainer, entity))
                    {
                        return true;
                    }

                    // try to restore the broken entity ..
                    if (!Add(sourceContainer, entity))
                    {
                        throw new Exception("Whoha!");
                    }
                }
            }

            return false;
        }

        public bool CanAdd(IContainerReference container, IItem entity)
        {
            if (!containers.TryGetValue(container, out Container c))
            {
                return Container.CanAddPrecheck(container, entity);
            }

            return c.CanAdd(entity);
        }

        class Container
        {
            public Container(IContainerReference id)
            {
                Id = id ?? throw new ArgumentNullException(nameof(id));
                Items = new List<IItem>();
            }

            public IContainerReference Id { get; }
            public List<IItem> Items { get; }
            public Weight Weight { get; private set; }

            public bool IsEmpty
            {
                get { return Items.Count == 0; }
            }

            public void Add(IItem item)
            {
                Items.Add(item);
                Weight += item.ItemType.Weight;
            }

            public bool Remove(IItem item)
            {
                return Items.Remove(item);
            }

            public ReadOnlyListWrapper<IItem> Contents()
            {
                return Items;
            }

            public bool CanAdd(IItem item)
            {
                return Id.ContainerType >= item.ItemType.Container;
            }

            public static bool CanAddPrecheck(IContainerReference id, IItem item)
            {
                return id.ContainerType >= item.ItemType.Container;
            }

            internal List<IItem> Contents(List<IItem> collector)
            {
                if (collector == null)
                {
                    collector = new List<IItem>(Items.Count);
                }
                else
                {
                    collector.Clear();
                }

                collector.AddRange(Items);
                return collector;
            }
        }
    }
}