using Steropes.Tiles.Sample.Shared.Dungeon.Model;
using Steropes.Tiles.Sample.Shared.Dungeon.Simple;
using Steropes.Tiles.Sample.Shared.Util;

namespace Steropes.Tiles.Sample.Shared.Dungeon
{
    /// <summary>
    ///   All of that should ideally come from external data files so that
    ///   tweaks to the system do not require a recompile. I leave that part
    ///   as an exercise to the reader.
    /// </summary>
    public class DungeonGameRules : IDungeonGameRules
    {
        public DungeonGameRules()
        {
            Floors = new DefinedFloors();
            FloorTypes = TypeRegistry.CreateFrom(Floors, f => f.Mud);

            Walls = new DefinedWalls();
            WallTypes = TypeRegistry.CreateFrom(Walls, w => w.None);

            Decorations = new DefinedDecorations();
            DecorationTypes = TypeRegistry.CreateFrom(Decorations, d => d.None);

            var idGenerator = new IdGenerator(1);
            Characters = new DefinedCharacters(idGenerator);

            Items = new DefinedItems(idGenerator);
            ItemTypes = TypeRegistry.CreateFrom<IItemType>(Items).AppendFrom(Characters);
        }

        public DefinedFloors Floors { get; }
        public DefinedWalls Walls { get; }
        public DefinedDecorations Decorations { get; }
        public DefinedItems Items { get; }
        public DefinedCharacters Characters { get; }

        public ITypeRegistry<IFloorType> FloorTypes { get; }
        public ITypeRegistry<IWallType> WallTypes { get; }
        public ITypeRegistry<IDecorationType> DecorationTypes { get; }
        public ITypeRegistry<IItemType> ItemTypes { get; }

        public class DefinedCharacters
        {
            internal DefinedCharacters(IdGenerator itemIdGenerator)
            {
                Player = new CharacterType(itemIdGenerator.Next(), "character.player", Weight.Kilo(15), 10, 10, 15);
                Jester = new CharacterType(itemIdGenerator.Next(), "character.jester", Weight.Kilo(70), 10, 10, 15);
                Farmer = new CharacterType(itemIdGenerator.Next(), "character.farmer", Weight.Kilo(70), 10, 8, 10);
                Drunk = new CharacterType(itemIdGenerator.Next(), "character.drunk", Weight.Kilo(70), 5, 5, 10);
                Peasant = new CharacterType(itemIdGenerator.Next(), "character.peasant", Weight.Kilo(70), 8, 10, 15);
                Fighter = new CharacterType(itemIdGenerator.Next(), "character.fighter", Weight.Kilo(70), 15, 15, 50);
                Thief = new CharacterType(itemIdGenerator.Next(), "character.thief", Weight.Kilo(70), 5, 10, 25);
            }

            public ICharacterType Player { get; }
            public ICharacterType Jester { get; }
            public ICharacterType Farmer { get; }
            public ICharacterType Drunk { get; }
            public ICharacterType Peasant { get; }
            public ICharacterType Fighter { get; }
            public ICharacterType Thief { get; }
        }

        public class DefinedItems
        {
            internal DefinedItems(IdGenerator idGenerator)
            {
                Chest = new ItemType(idGenerator.Next(), "item.chest-closed", 1, Weight.Kilo(1), ContainerType.Chest,
                                     ItemClass.Misc);
                Bag = new ItemType(idGenerator.Next(), "item.bag", 1, Weight.Gram(250), ContainerType.Bag, ItemClass.Misc);
                Gold = new ItemType(idGenerator.Next(), "item.gold", 1000, Weight.Gram(10), ContainerType.None, ItemClass.Misc);
                Sword = new ItemType(idGenerator.Next(), "item.sword", 1, Weight.Kilo(7), ContainerType.None, ItemClass.Weapon);
                Chainmail = new ItemType(idGenerator.Next(),
                                         "item.chainmail", 120, Weight.Kilo(12), ContainerType.None, ItemClass.Equipment);
                PlateArmour = new ItemType(idGenerator.Next(),
                                           "item.plate", 220, Weight.Kilo(30), ContainerType.None, ItemClass.Equipment);
                Rations = new ItemType(idGenerator.Next(),
                                       "item.rations", 10, Weight.Kilo(0.5f), ContainerType.None, ItemClass.Consumable);
            }

            public IItemType Chest { get; }
            public IItemType Bag { get; }
            public IItemType Gold { get; }
            public IItemType Sword { get; }
            public IItemType Chainmail { get; }
            public IItemType PlateArmour { get; }
            public IItemType Rations { get; }
        }

        /// <summary>
        ///   Floors are a static part of the map and form the lowest rendering layer.
        ///   Floors do not change (at least in this demo game).
        /// </summary>
        public class DefinedFloors
        {
            internal DefinedFloors()
            {
                Mud = new FloorType(0, "floor.gras", 30, 0);
                Stone = new FloorType(1, "floor.stone", 30, 0);
                Water = new FloorType(2, "floor.water", 60, 10);
            }

            public IFloorType Mud { get; }
            public IFloorType Stone { get; }

            public IFloorType Water { get; }
        }

        /// <summary>
        ///   Walls define a static part of the map. Doors are modelled in two layers:
        ///   the wall-map contains the passage-indicator. This allows us to diferentiate
        ///   between empty tiles and tiles that can contain doors and is used when
        ///   matching walls to their cardinal neighbours. For matching purposes, a
        ///   passage is considered a wall.
        /// </summary>
        public class DefinedWalls
        {
            internal DefinedWalls()
            {
                None = new WallType(0, "", false, false);
                Passage = new WallType(1, "wall.passage", true, true);
                Stone = new WallType(2, "wall.stone", true, true);
            }

            public IWallType None { get; }
            public IWallType Stone { get; }

            public IWallType Passage { get; }
        }


        public class DefinedDecorations
        {
            public DefinedDecorations()
            {
                None = new DecorationType(0, "", 0, 0);
                Shelf = new DecorationType(1, "decoration.shelf", 0, 1);
                Window = new DecorationType(2, "decoration.window", 0, 0);
            }

            public IDecorationType None { get; }
            public IDecorationType Shelf { get; }
            public IDecorationType Window { get; }
        }
    }
}
