using System;
using Microsoft.Xna.Framework;

namespace Steropes.Tiles.MonogameDemo.GameData.Strategy.Model
{
  public interface IPlayer
  {
    string Name { get; }
    Color Color { get; }
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

  public class Player: IPlayer
  {
    public Player(string name, Color color, Culture culture)
    {
      Name = name ?? throw new ArgumentNullException(nameof(name));
      Color = color;
    }

    public string Name { get; }
    public Color Color { get; }
    public Culture Culture { get; }
  }
}