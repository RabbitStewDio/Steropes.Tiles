using System;
using Steropes.Tiles.Demo.Core.GameData.Dungeon.Model;
using Steropes.Tiles.Demo.Core.Util;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Demo.Core.GameData.Dungeon.Simple
{
    public class ContainerReferenceProvider : IContainerReferenceProvider<MapCoordinate>,
                                              IContainerReferenceProvider<IItem>,
                                              IContainerReferenceProvider<ICharacter>
    {
        readonly LRUCache<MapCoordinate, IContainerReference> mapCache;
        readonly LRUCache<Tuple<int, ContainerType>, IContainerReference> itemCache;

        public ContainerReferenceProvider()
        {
            mapCache = new LRUCache<MapCoordinate, IContainerReference>(32000);
            itemCache = new LRUCache<Tuple<int, ContainerType>, IContainerReference>(32000);
        }

        public IContainerReference From(MapCoordinate mc)
        {
            if (mapCache.TryGet(mc, out IContainerReference retval))
            {
                return retval;
            }

            var r = new ContainerReference<MapCoordinate>(ContainerReferenceType.Map, mc, ContainerType.Unlimited);
            mapCache.Add(mc, r);
            return r;
        }

        public IContainerReference From(IItem item)
        {
            var key = Tuple.Create(item.Id, item.ItemType.Container);
            if (itemCache.TryGet(key, out IContainerReference retval))
            {
                return retval;
            }

            var r = new ContainerReference<int>(ContainerReferenceType.Item, item.Id, item.ItemType.Container);
            itemCache.Add(key, r);
            return r;
        }

        public IContainerReference From(ICharacter item)
        {
            return new ContainerReference<int>(ContainerReferenceType.Character, item.Id, ContainerType.Character);
        }

        class ContainerReference<T> :
            IContainerReference,
            IEquatable<ContainerReference<T>>
            where T : struct, IEquatable<T>
        {
            readonly T entityId;

            public ContainerReference(ContainerReferenceType referenceType, T entityId, ContainerType containerType)
            {
                this.entityId = entityId;
                ReferenceType = referenceType;
                ContainerType = containerType;
            }

            public ContainerReferenceType ReferenceType { get; }
            public ContainerType ContainerType { get; }

            public bool Equals(ContainerReference<T> other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return entityId.Equals(other.entityId) && ReferenceType == other.ReferenceType;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }

                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                if (obj.GetType() != GetType())
                {
                    return false;
                }

                return Equals((ContainerReference<T>)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (entityId.GetHashCode() * 397) ^ (int)ReferenceType;
                }
            }

            public static bool operator ==(ContainerReference<T> left, ContainerReference<T> right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(ContainerReference<T> left, ContainerReference<T> right)
            {
                return !Equals(left, right);
            }
        }
    }
}
