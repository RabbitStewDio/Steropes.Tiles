using System;

namespace Steropes.Tiles.TemplateGenerator.Editors
{
    public interface IValidator
    {
        void Validate();
        event EventHandler ValidationStateChanged;
        event EventHandler InputReceived;
        bool Valid { get; }
    }
}
