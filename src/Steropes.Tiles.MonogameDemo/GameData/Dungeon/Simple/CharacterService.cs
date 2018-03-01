using System;
using Steropes.Tiles.MonogameDemo.GameData.Dungeon.Model;
using Steropes.Tiles.MonogameDemo.GameData.Util;

namespace Steropes.Tiles.MonogameDemo.GameData.Dungeon.Simple
{
  public class CharacterService: BaseTraitService<ICharacter>, ICharacterService
  {
    readonly IItemService itemService;
    readonly IdGenerator generator;

    public CharacterService(IItemService itemService)
    {
      this.itemService = itemService;
      generator = new IdGenerator();
    }

    public ICharacter Create(string name, ICharacterType avatar)
    {
      var avatarHandle = itemService.Create(avatar);
      var c = new Character(generator.Next(), avatarHandle, name);
      AddTrait<ICharacterTraits>(c, new CharacterTraits(avatar));
      return c;
    }

    public bool Use(ICharacter character, IItem usable)
    {
      throw new System.NotImplementedException();
    }

    public bool Attack(ICharacter character, IItem weapon, IItem target)
    {
      throw new System.NotImplementedException();
    }

    public bool Destroy(ICharacter character)
    {
      if (itemService.Destroy(character.Avatar))
      {
        RemoveAllTraits(character);
        return true;
      }
      return false;
    }

    class CharacterTraits: ICharacterTraits
    {
      public CharacterTraits(ICharacterType avatar): this(avatar.Strength, avatar.Vitality, avatar.HitPoints)
      {
      }

      public CharacterTraits(int strength, int vitality, int hitPoints)
      {
        Strength = strength;
        Vitality = vitality;
        HitPoints = hitPoints;
      }

      public int Strength { get; set; }
      public int Vitality { get; set; }
      public int HitPoints { get; set; }
    }

    class Character : ICharacter, IEquatable<Character>
    {
      public Character(int id, IItem avatar, string name)
      {
        Id = id;
        Avatar = avatar ?? throw new ArgumentNullException(nameof(avatar));
        Name = name ?? throw new ArgumentNullException(nameof(name));
      }

      public int Id { get; }
      public IItem Avatar { get; }
      public string Name { get; }

      public bool Equals(Character other)
      {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id;
      }

      public override bool Equals(object obj)
      {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Character) obj);
      }

      public override int GetHashCode()
      {
        return Id;
      }

      public static bool operator ==(Character left, Character right)
      {
        return Equals(left, right);
      }

      public static bool operator !=(Character left, Character right)
      {
        return !Equals(left, right);
      }
    }
  }
 
}