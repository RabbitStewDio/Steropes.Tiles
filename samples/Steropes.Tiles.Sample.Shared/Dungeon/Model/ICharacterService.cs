namespace Steropes.Tiles.Sample.Shared.Dungeon.Model
{
    public interface ICharacterService
    {
        ICharacter Create(string name, ICharacterType avatar);
        bool Use(ICharacter character, IItem usable);
        bool Attack(ICharacter character, IItem weapon, IItem target);

        bool TraitFor<T>(ICharacter c, out T trait)
            where T : IItemTrait;
    }
}
