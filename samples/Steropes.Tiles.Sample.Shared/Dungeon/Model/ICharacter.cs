﻿namespace Steropes.Tiles.Sample.Shared.Dungeon.Model
{
    /// <summary>
    ///  A handle to the character.
    /// </summary>
    public interface ICharacter
    {
        int Id { get; }

        IItem Avatar { get; }
        string Name { get; }
    }
}
