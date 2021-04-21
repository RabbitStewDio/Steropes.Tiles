using Steropes.Tiles.Navigation;
using System;

namespace Steropes.Tiles.Sample.Shared.Strategy.Model
{
    public interface ISettlement
    {
        string Name { get; }
        MapCoordinate Location { get; }
        byte Owner { get; }
        long Population { get; }
        bool Walled { get; }
    }

    public class Settlement : ISettlement
    {
        public Settlement(string name, MapCoordinate location, byte owner, long population, bool walled)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Location = location;
            Owner = owner;
            Population = population;
            Walled = walled;
        }

        public string Name { get; }
        public MapCoordinate Location { get; }
        public byte Owner { get; set; }
        public long Population { get; set; }
        public bool Walled { get; }
    }
}
