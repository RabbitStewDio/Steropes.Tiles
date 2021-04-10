namespace Steropes.Tiles.Sample.Shared.Dungeon.Model
{
    public interface ICharacterTraits : IItemTrait
    {
        int Strength { get; set; }
        int Vitality { get; set; }
        int HitPoints { get; set; }
    }
}
