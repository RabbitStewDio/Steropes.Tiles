using Steropes.Tiles.Sample.Shared.Dungeon.Model;
using System;

namespace Steropes.Tiles.Sample.Shared.Dungeon.Simple
{
    public class CharacterType : ICharacterType
    {
        public CharacterType(ItemTypeId id, string name, Weight weight, int strength, int vitality, int hitPoints)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Weight = weight;
            Strength = strength;
            Vitality = vitality;
            HitPoints = hitPoints;
        }

        public ItemTypeId Id { get; }
        public string Name { get; }
        public Weight Weight { get; }
        public int Strength { get; }
        public int Vitality { get; }
        public int HitPoints { get; }

        public int MaxStackSize => 1;
        public ContainerType Container => ContainerType.Character;
        public ItemClass ItemClass => ItemClass.Avatar;
    }
}
