namespace Steropes.Tiles.Demo.Core.GameData.Dungeon.Model
{
  public static class CharacterExtensions
  {
    public static int MaxHitPoints(this ICharacterTraits t)
    {
      return t.Vitality * 2000;
    }
  }
}