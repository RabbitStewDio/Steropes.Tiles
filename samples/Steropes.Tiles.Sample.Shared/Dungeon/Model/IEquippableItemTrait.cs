namespace Steropes.Tiles.Sample.Shared.Dungeon.Model
{
    public interface IEquippableItemTrait : IItemTrait
    {
        int StrengthRequirement { get; }
        int Durability { get; }
    }
}
