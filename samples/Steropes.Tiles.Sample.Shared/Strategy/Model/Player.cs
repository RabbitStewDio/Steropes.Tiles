using System;

namespace Steropes.Tiles.Demo.Core.GameData.Strategy.Model
{
    public enum PlayerColor
    {
        Red,
        Green,
        Blue
    }

    public interface IPlayer
    {
        string Name { get; }
        PlayerColor PlayerColor { get; }
        Culture Culture { get; }
    }

    public enum Culture
    {
        Asian = 0,
        Tropical = 1,
        Celtic = 2,
        Classical = 3,
        Babylonian = 4
    }

    public class Player : IPlayer
    {
        public Player(string name, PlayerColor playerColor, Culture culture)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            PlayerColor = playerColor;
        }

        public string Name { get; }
        public PlayerColor PlayerColor { get; }
        public Culture Culture { get; }
    }
}
