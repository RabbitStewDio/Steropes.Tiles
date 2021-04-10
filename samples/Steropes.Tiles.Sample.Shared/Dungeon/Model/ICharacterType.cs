namespace Steropes.Tiles.Sample.Shared.Dungeon.Model
{
    /// <summary>
    ///   Provides default values for character creation.
    /// </summary>
    public interface ICharacterType : IItemType
    {
        int Strength { get; }
        int Vitality { get; }
        int HitPoints { get; }
    }
}
