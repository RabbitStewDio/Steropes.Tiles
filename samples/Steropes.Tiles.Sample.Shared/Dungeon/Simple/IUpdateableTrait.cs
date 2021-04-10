using Steropes.Tiles.Sample.Shared.Dungeon.Model;

namespace Steropes.Tiles.Sample.Shared.Dungeon.Simple
{
    public interface IUpdateableTrait : IItemTrait
    {
        void Update(float t);
    }
}
