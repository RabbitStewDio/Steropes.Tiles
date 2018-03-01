namespace Steropes.Tiles.MonogameDemo.GameData.Dungeon.Model
{
  public interface IEquippableItemTrait : IItemTrait
  {
    int StrengthRequirement { get; }
    int Durability { get; }
  }
}