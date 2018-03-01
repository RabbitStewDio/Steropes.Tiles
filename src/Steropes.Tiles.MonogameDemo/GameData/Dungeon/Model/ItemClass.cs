using System;

namespace Steropes.Tiles.MonogameDemo.GameData.Dungeon.Model
{
  /// <summary>
  ///   A crude classification system on how items can be used. An item can
  ///   be part of multiple classes (ie character and weapon for talking swords).
  /// </summary>
  /// Items without special roles have no flag set.
  [Flags]
  public enum ItemClass
  {
    /// <summary>
    ///   A generic thing.
    /// </summary>
    Misc = 0,

    /// <summary>
    ///   Armour and clothing.
    /// </summary>
    Equipment = 1,

    /// <summary>
    ///   Something to wield or shoot with.
    /// </summary>
    Weapon = 2,

    /// <summary>
    ///   Potion, scrolls, food.
    /// </summary>
    Consumable = 4,

    /// <summary>
    ///   An avatar for an character. Characters are mapped into the world using 
    ///   avatar items. The avatar item represents the physical body of the 
    ///   character entity, while the character handle represents the soul, memory
    ///   and personality aspect.
    /// </summary>
    /// <para>
    ///   Warning: There is usually a 1:n relationship between avatar types and souls.
    ///   Therefore you cannot necesarily deduct the character based on the avatar
    ///   item found on the map. Uniformed soldiers will all use the same avatar body, 
    ///   but may have different minds.
    /// </para>
    Avatar = 8
  }
}