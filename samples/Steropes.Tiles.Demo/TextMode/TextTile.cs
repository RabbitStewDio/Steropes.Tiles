using System;

namespace Steropes.Tiles.Demo.TextMode
{
    /// <summary>
    /// This represents a single rendered map element. Think if this as your
    /// texture that renders for a single tile.
    /// </summary>
    public class TextTile
    {
        public string Name { get; }
        public char Rendering { get; }
        public ConsoleColor Color { get; }

        public TextTile(string name, char rendering, ConsoleColor color)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Rendering = rendering;
            Color = color;
        }
    }
}
