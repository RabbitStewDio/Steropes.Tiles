namespace Steropes.Tiles.MonogameDemo.GameData.Dungeon.Model
{
  public interface ICharacterTraits : IItemTrait
  {
    int Strength { get; set; }
    int Vitality { get; set; }
    int HitPoints { get; set; }
  }
}