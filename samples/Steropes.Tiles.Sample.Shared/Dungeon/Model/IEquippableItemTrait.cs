namespace Steropes.Tiles.Demo.Core.GameData.Dungeon.Model
{
  public interface IEquippableItemTrait : IItemTrait
  {
    int StrengthRequirement { get; }
    int Durability { get; }
  }
}