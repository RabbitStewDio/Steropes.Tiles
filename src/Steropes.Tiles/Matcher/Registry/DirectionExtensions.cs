using System;
using Steropes.Tiles.Matcher.Sprites;

namespace Steropes.Tiles.Matcher.Registry
{
    public static class DirectionExtensions
    {
        public const int Size = 4;

        public static SpritePosition MapToPosition(this Direction d)
        {
            switch (d)
            {
                case Direction.Up:
                    return SpritePosition.Up;
                case Direction.Right:
                    return SpritePosition.Right;
                case Direction.Down:
                    return SpritePosition.Down;
                case Direction.Left:
                    return SpritePosition.Left;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
